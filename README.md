# 哔哩哔哩《源代码生成器》系列视频配套代码

该仓库列举的是我在 B 站发布的《源代码生成器》系列教学视频的配套代码。

## 使用方式

首先请使用 GitHub 自带的下载功能，下载本仓库的所有内容。你也可使用命令下载：

```bash
git clone https://github.com/SunnieShine/SourceGeneratorSample.git
```

> 如果下载失败，出现一些例如 502 的错误信息，请检查是否你的所在区域封锁了 GitHub。如果确实如此，请使用第三方工具（如 [FastGitHub](https://github.com/dotnetcore/FastGithub)）等来完成访问操作。

## 项目结构说明

本项目包含一个主要 `.sln` 文件，它为整个解决方案的基础文件；根目录下包含一个 `src` 文件夹，里面存放了一个一个的文件夹，而每一个文件夹都代表一个项目，每一个项目至少带有一个源代码生成器的源代码文件，并命名为 `什么什么Generator`。我们视频讲解期间，都是在说关于这个源代码生成器本身的内容。

另外，在主项目里，项目会使用 `xxxLesson` 来完成对源代码生成器的对接调用过程。请点进去看里面的代码。

## 额外说明

我的电脑是 2K 的显示器，所以视频大小在视频播放的时候看起来可能两侧会有黑条。这是正常的，因为 B 站的视频没有 2K 的显示器对应清晰度，它默认会被处理为 1080P，所以就没办法。但实际上不怎么会特别影响食用。当然，有一些地方，在录制视频的时候字比较小，但我忘记放大，导致不清晰的地方，还望多多包涵。这些地方应该也不是特别重要，或者是前面的视频分 p 已经说过了，就不再啰嗦重复的内容，所以没有放大。

另外，使用放大镜功能后，视频里会出现一个额外的小鼠标指针。这是我使用的 Bandicam 对视频分辨率的兼容性的问题，这不影响食用，但可能看着会有点难受，请你忽略那个小的指针。

## 开源协议

[Unlicense 协议](LICENSE)

\* 你可以肆无忌惮地使用此仓库里的源代码，来干你想干的任何事情。你甚至在使用本仓库的源代码期间无需声明版权信息。你只要下载下来就属于你了！

## 链接

[源代码生成器系列视频](https://b23.tv/QkbG1uC)

[`ISourceGenerator` 使用手册](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md)

[`IIncrementalGenerator` 使用手册](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md)

## 作者

[SunnieShine](https://space.bilibili.com/23736703)
