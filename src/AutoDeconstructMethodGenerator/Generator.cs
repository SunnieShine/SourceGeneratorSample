namespace AutoDeconstructMethodGenerator;

/// <summary>
/// 解构函数的源代码生成器。
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class Generator : IIncrementalGenerator
{
	/// <inheritdoc/>
	public void Initialize(IncrementalGeneratorInitializationContext context)
		=> context.RegisterSourceOutput(
			context.SyntaxProvider
				.ForAttributeWithMetadataName(
					"SourceGeneratorSample.Models.DeconstructAttribute",
					static (_, _) => true,
					static (gasc, _) =>
					{
						if (gasc is not
							{
								TargetNode: MethodDeclarationSyntax
								{
									Modifiers: var methodModifiers and not [],
									Parent: TypeDeclarationSyntax { Modifiers: var typeModifiers and not [] }
								},
								TargetSymbol: IMethodSymbol
								{
									Name: "Deconstruct",
									ReturnsVoid: true,
									Parameters: { Length: >= 2 } parameters,
									IsGenericMethod: false,
									ContainingType:
									{
										Name: var typeName,
										IsRecord: var isRecord,
										TypeKind: var typeKind,
										ContainingNamespace: var @namespace
									}
								}
							})
						{
							return null;
						}

						if (!methodModifiers.Any(SyntaxKind.PartialKeyword)
							|| !typeModifiers.Any(SyntaxKind.PartialKeyword))
						{
							return null;
						}

						if (!parameters.All(static parameter => parameter.RefKind == RefKind.Out))
						{
							return null;
						}

						var namespaceString = @namespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
						namespaceString = namespaceString["global::".Length..];

						return new GatheredData(typeName, isRecord, typeKind, namespaceString, parameters.ToArray(), methodModifiers);
					}
				)
				.Collect(),
			static (spc, data) =>
			{
				foreach (var (fileName, gatheredDataArray) in
					from tuple in data
					group tuple by $"{tuple.Namespace}.{tuple.TypeName}" into @group
					let fileName = $"{@group.Key}{SourceGeneratorFileNameShortcut.AutoDeconstructMethodGenerator}"
					select (FileName: fileName, Data: @group.ToArray()))
				{
					var deconstructMethodsCode = new List<string>();
					foreach (var (_, _, _, _, parameters, methodModifiers) in gatheredDataArray)
					{
						var parametersString = string.Join(
							", ",
							from parameter in parameters
							let type = parameter.Type
							let typeString = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
							select $"out {typeString} {parameter.Name}"
						);

						var assigns = string.Join(
							Environment.NewLine,
							from parameter in parameters
							let parameterName = parameter.Name
							let pascalParameterName = parameterName.ToPascalString()
							let method = (IMethodSymbol)parameter.ContainingSymbol
							let type = method.ContainingType
							let properties = type.GetMembers().OfType<IPropertySymbol>()
							let foundProperty = properties.First(
								property => property.Name == pascalParameterName
									&& SymbolEqualityComparer.Default.Equals(property.Type, parameter.Type)
							)
							select $"\t\t{parameterName} = {foundProperty.Name};"
						);

						deconstructMethodsCode.Add(
							$$"""
							[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
								[global::System.CodeDom.Compiler.GeneratedCodeAttribute("{{nameof(AutoDeconstructMethodGenerator)}}", "{{SourceGeneratorVersion.Value}}")]
								{{methodModifiers}} void Deconstruct({{parametersString}})
								{
							{{assigns}}
								}
							"""
						);
					}

					var typeKindString = gatheredDataArray[0] switch
					{
						{ IsRecord: true, TypeKind: TypeKind.Class } => "record",
						{ IsRecord: true, TypeKind: TypeKind.Struct } => "record struct",
						{ TypeKind: TypeKind.Class } => "class",
						{ TypeKind: TypeKind.Struct } => "struct",
						{ TypeKind: TypeKind.Interface } => "interface",
						_ => throw new InvalidOperationException("解构函数不支持。")
					};

					spc.AddSource(
						fileName,
						$$"""
						// <auto-generated />

						#nullable enable
						namespace {{gatheredDataArray[0].Namespace}};

						partial {{typeKindString}} {{gatheredDataArray[0].TypeName}}
						{
							{{string.Join("\r\n\r\n\t", deconstructMethodsCode)}}
						}
						"""
					);
				}
			}
		);
}

file sealed record GatheredData(
	string TypeName,
	bool IsRecord,
	TypeKind TypeKind,
	string Namespace,
	IParameterSymbol[] Parameters,
	SyntaxTokenList MethodModifiers
);

/// <summary>
/// 只用于这个文件的扩展方法。
/// </summary>
file static class Extensions
{
	/// <summary>
	/// 将指定的字符串按帕斯卡命名法书写。
	/// </summary>
	/// <param name="this">字符串。</param>
	/// <returns>帕斯卡命名法后的字符串。</returns>
	public static string ToPascalString(this string @this) => $"{char.ToUpper(@this[0])}{@this[1..]}";
}
