# nscript

nscript 是面向图形、图像、语音、视频、AI、RPA、生物医药计算的高性能脚本语言，它是 python + cpp 的替代方案。nscript 拥有脚本、jit、aot 三种运行模式，拥有媲美 python 的开发速度和接近 cpp 的性能，拥有强大的静态类型系统和丰富的类库。可以在 vscode 里编辑与调试。它可以用做胶水语言，可以方便的开发 webapi，也可以写简单的桌面程序，还可以进行云原生开发（实现中....）。它基于 [dotnet-script](https://github.com/filipw/dotnet-script) 发展而来。

## 语言的定位

有人会问，为什么要弄一门新的语言，是现有语言不好吗？

是的，现有开发语言确实不好。

Python 是一个极端，它是动态类型的语言，写起来很舒服，但是代码量一大，维护的难度就会变大，同时性能低下到有时能让你发狂。C++ 语言是另一个极端，可以做到最优秀的性能，但是开发速度慢，编译速度慢，一不小心就会出错。Python + C++ 是一个合理的搭配，然而团队中需要两者都懂的人，不然，当遇到无法通过调包来解决问题时，就会遇到瓶颈。

java 是另一大生态，这个生态中，jvm 以及语言自身设计的过于保守，虽然自 java 8 之后，java 语言已经比较好使了，然而一些基础设施的缺乏，直接用它来处理非结构化数据蹩手蹩脚，使用 java 处理过 rgb 颜色的童靴们有深刻的体验。而无法自定义值类型，处理大量的小对象会对 gc 造成过大的压力，使得在很多场景用不了。

nscript 源自 dotnet script，dotnet 是 C# 的脚本语言，那么为什么不直接用 C# 语言呢？主要是非技术原因。C# 语言虽然在海外发展的不错，在国内由于历史原因，几乎社会性死亡了。以其拯救它，还不如开辟一个新路线、新社区出来。

微软对 C# 语言的定位一直是有重大问题的，掺杂了众多的商业因素在其中，虽然自 .net core 的发布后，开始迷途知返，然而已经失去了程序员们太多的信任，导致在国内没办法流行。而按照微软的既定策略，再度流行的难度依然很低。国内的 Web 领域，目前是 java 的天下。人工智能，又是 Python / C++ 的领地。生态位被抢走了，再要恢复，难上加难。

这也是为什么改名 nscript 的原因 - 卸下历史的包袱，面向未来，将它改造成面向多媒体数据处理、AI、分布式计算的脚本语言。

由于支持 struct 和指针，以及对 simd 的支持，nscript 非常适合于密集计算，其CPU需求和内存需求大大低于 java，远低于 python，接近于 C++ 程序，而程序的开发速度和可维护性，远优于 C++。对于云原生来说，这是非常大的优势。随着国内的产业升级，对高性能分布式计算具有强烈的需求，这一领域具备成为王者的潜力，而 nscript 志在于此，是当下唯一可以实现快速开发、快速部署、快速启动、快速计算的云原生语言。

在传统企业中，存在相当大数量的 C# 程序员。随着企业数字化进程的加速，人工智能企业服务的全面推进，这部分生态是可以重建的。而对 AI 应用的支持，C# 理应具有一定的优势。然而，在国内传播的并不多。无它，没有人推广。可以说得上生态的，还有 Unity 独木撑起的 3D 部分。随着元宇宙概念的火热，这一部分生态会进一步扩张。

nscript 的定位：

- nscript 是面向云原生的语言，未来，对于密集型计算任务，只需要写一段脚本，一键就可以发布到上万个计算节点，启动镜像函数进行计算，只对计算所耗费的资源按需付费。

- nscript 是面向 AI 的语言，它能够非常容易集成常见的推理模型，让传统行业的程序员方便的进行AI应用的开发、集成和维护。

- nscript 是面向 3D 和元宇宙的语言，使用它，可以非常容易的创建3D内容和应用。

## 安装

需要先下载安装 [.NET 6.0+ SDK](https://www.microsoft.com/net/download/core). 对于低于 6.0 的版本，也许可以用，也许不可以用, 不再进行测试和兼容。

nscript 目前未提供安装包，请自行下载编译，设置环境变量，使得 nscript 可直接访问。windows 环境下，请将 nscript.exe 所在目录加入 PATH 环境变量中。linux, macosx 下未测试，应该不难，请自行测试。

## 使用

编辑文件 `helloworld.csx`，包含下面内容:

```cs
Console.WriteLine("Hello world!");
```

输入下列指令即可执行脚本:

```
nscript helloworld.csx
```

### 开发环境

VS Code 安装 C# 插件，可直接支持 nscript 脚本的开发和调试。不过，当前版本的 omnisharp 对 .net 6.0 下的脚本支持存在问题。我 fork 了一个分支 (omnisharp-roslyn-nex)[https://github.com/nscript-site/omnisharp-roslyn-next] 解决了这个问题。请自行下载编译，修改 VS Code 中设置项后重启 VS Code 即可：

```
    "omnisharp.useModernNet": true,
    "omnisharp.path": [PATH TO YOUT 'OmniSharp.dll'
```

有点麻烦是不？不想麻烦，就等待 omnisharp 的更新吧。

### 脚手架

通过 init 指令，可以生成脚手架脚本。

```shell
nscript init
```

这将生成 `main.csx` 文件，以及 launch 配置文件，方便在 VS Code 下调试脚本. 生成的文件结构如下：

```shell
.
├── .vscode
│   └── launch.json
├── main.csx
└── omnisharp.json
```

我们也可以指定创建的文件名称.

```shell
nscript init custom.csx
```

这样，创建的就不是 `main.csx` 文件, 而是 `custom.csx` 文件.

```shell
.
├── .vscode
│   └── launch.json
├── custom.csx
└── omnisharp.json
```

> 注: 执行 `nscript init` 时，不会覆盖目录中已有的文件.

### 运行脚本

脚本可通过 shell 直接执行: 

```bash
foo.csx arg1 arg2 arg3
```

> OSX/Linux
>
> 所有脚本一样，在OSX/Linux上，你需要有一个"#！"，并通过**chmod +x foo.csx**将文件标记为可执行文件。如果您使用 **nscript init** 来创建 csx，它将自动具有 '#！' 指令并被标记为可执行文件。

```cs
#!/usr/bin/env nscript
Console.WriteLine("Hello world");
```

"--"之后的所有参数都通过以下方式传递到脚本：

```shell
nscript foo.csx -- arg1 arg2 arg3
```

可以使用全局"Args"集合访问脚本上下文中的参数：

```c#
foreach (var arg in Args)
{
    Console.WriteLine(arg);
}
```

"--"之前的所有参数都由"ncript"处理。例如，以下命令行

```shell
nscript -d foo.csx -- -d
```
会将 "--" 之前的 "-d" 传递给 "nscript" 并启用调试模式，而 "--" 之后的 "-d" 将传递给脚本，以便自己解释参数。

### 引用 NuGet 包

可以从脚本中引用 NuGet 包t.

```c#
#r "nuget: AutoMapper, 6.1.0"
```

> 注意：Omnisharp 在添加新的包引用后需要重新启动

可以在脚本根文件夹中使用 `NuGet.Config` 文件定义包源。除了在脚本执行期间使用之外，它还将由 `OmniSharp` 使用，`OmniSharp` 为从这些包源解析的包提供语言服务。

作为维护本地 `NuGet.Config` 文件的替代方法，我们可以在用户级别或计算机级别全局定义这些包源，如 [Configuring NuGet Behaviour](https://docs.microsoft.com/en-us/nuget/reference/nuget-config-file) 中所述。

还可以在执行脚本时指定包源.

```shell
ncript foo.csx -s https://SomePackageSource
```

可以按如下方式指定多个包源：

```shell
nscript foo.csx -s https://SomePackageSource -s https://AnotherPackageSource
```

### 引用 dll 或其它脚本文件

可以通过 `#r` 指令直接引用 dll, 例如：

```
#r "../Geb.Image.dll"
```

dll 的路径可以是直接路径或相对路径。

### 引用其他脚本文件

可以通过 `#load` 指令加载其它脚本文件, 例如：

```
#load "base.csx"
```

### snippets

dotnet script 写简单的脚本比较方便，但是写桌面程序或web程序相关的脚本非常不方便。nscript 提供了 snippets 指令，运行 `nscript snippets`，可以生成常用 snippets. 生成的文件如下：

```shell
.
├── snippets
│   └── aspnet.csx
│   └── eserver.csx
│   └── httpserver.csx
│   └── winui.csx
```

其中，aspnet.csx 和 winui.csx 分别是写 asp.net 程序和 wpf/winform 程序所需要引用的 dll，可直接 load 来写相关脚本。httpserver.csx 是一个 asp.net webapi 脚本示例。eserver.csx 是使用 [EmbedIO](https://github.com/unosquare/embedio) 的 webapi 脚本示例。[EmbedIO](https://github.com/unosquare/embedio) 是一款跨平台、轻量级 Web Server 库，觉得 asp.net 过于重的，可以试试使用 [EmbedIO](https://github.com/unosquare/embedio).

下面是 `eserver.csx` 的内容，很简单不是？！

```c#
#r "nuget: EmbedIO, 3.4.3"

using EmbedIO;
using EmbedIO.WebApi;
using EmbedIO.Actions;
using EmbedIO.Files;

WebServer CreateWebServer(string url)
{
    var server = new WebServer(o => o
            .WithUrlPrefix(url)
            .WithMode(HttpListenerMode.EmbedIO))
        .WithLocalSessionManager()
        .WithStaticFolder("/static/", "./webroot", true, m => m
           .WithContentCaching(true)) // Add static files after other modules to avoid conflicts
        .WithModule(new ActionModule("/api", HttpVerbs.Get, HandleApi))
        .WithModule(new ActionModule("/", HttpVerbs.Any, ctx => ctx.SendDataAsync(new { Message = "Hello!" })))
        ;
    return server;
}

Task HandleApi(IHttpContext ctx)
{
    return ctx.SendDataAsync("api");
}

var url = "http://*:5000/";
var server = CreateWebServer(url);
server.RunAsync();
Console.ReadKey();
```

### 缓存

这里是 dotnet script 需要改进的地方。当 a.csx 引用 b.dll 时，当 b.dll 改变了，直接运行 a.csx 仍然引用的是改变前的 dll。可以通过传递 **--no-cache** 标记来覆盖自动缓存，这样每次执行脚本时都进行编译编译
。
```shell
nscript --no-cache a.csx
```

未来，nscript 会改进为判断引用的 dll 是否改动，若改动则重新编译，以减少对 --no-cache 的使用。

### 调试

可以在 VS Code 里面直接调试 `nscript` 脚本，只需在脚本文件中的任何位置设置断点，然后按F5（开始调试）。下面录屏是 `dotnet script` 的调试录屏，做为它的魔改版本，`nscript` 的调试操作也一样。

![debug](https://user-images.githubusercontent.com/1034073/30173509-2f31596c-93f8-11e7-9124-ca884cf6564e.gif)

### 发布

`dotnet script`的发布功能不强。nscript 未来会提供易用的发布功能(dotnet publish 的发布指令也比较复杂)，未来会支持以下发布模式：

- JIT：默认参数等价于 dotnet publish 下的 `-c release /p:PublishSingleFile=true /p:PublishTrimmed=true /p:EnableCompressionInSingleFile=true --self-contained`，经过测试，一个简单的 hello world 的发布尺寸为 5M 左右，使用 一个简单的 [EmbedIO](https://github.com/unosquare/embedio) webapi 程序，发布尺寸为 11-12M，这个尺寸已经非常小了，足以和 golang 媲美了；

- AOT：通过简单配置，能支持 AOT 发布，[zerosharp](https://github.com/MichalStrehovsky/zerosharp) 中，AOT程序可以做到数k的尺寸；

- 云原生：云原生会作为语言的基础设施，方便脚本的大规模部署和执行。

与 .net 和 dotnet script 不同，nscript 会侧重于对云原生及系统开发的支持，侧重于对无反射程序的支持。

### 远程脚本

还可以执行在 `http(s)`上的脚本。

```shell
nscript https://tinyurl.com/y8cda9zt
```

### 脚本位置

一个非常常见的情况是，我们有相对于脚本路径的逻辑。我们不希望要求用户位于某个目录中才能正确解析这些路径，因此下面介绍了如何提供脚本路径和脚本文件夹，而不管当前工作目录如何。

```c#
public static string GetScriptPath([CallerFilePath] string path = null) => path;
public static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);
```

> Tip: 将这些方法作为顶级方法放在单独的脚本文件中，并在需要访问脚本路径和/或文件夹的任何位置`#load`该文件。

## REPL

和 `dotnet script` 一样支持 REPL 使用。

### 基础使用

一旦 `nscript` 启动，您将看到输入提示。可以开始在此处键入 C# 代码。

```
~$ nscript
> var x = 1;
> x+x
2
```

如果将未终止的表达式提交到 REPL 中（末尾没有";"），则将对其进行计算，并且将使用格式化程序序列化结果并将其打印在输出中。这比仅仅在对象上调用"ToString（）"更有趣，因为它试图捕获对象的实际结构。例如：

```
~$ nscript
> var x = new List<string>();
> x.Add("foo");
> x
List<string>(1) { "foo" }
> x.Add("bar");
> x
List<string>(2) { "foo", "bar" }
>
```

### 内联 Nuget 包

REPL also supports inline Nuget packages - meaning the Nuget packages can be installed into the REPL from _within the REPL_. This is done via our `#r` and `#load` from Nuget support and uses identical syntax.

REPL 还支持内联 Nuget 包。这意味着可以在 REPL 中直接安装 Nuget 包。这是通过 Nuget 支持的"#r"和"#load"完成的。

```
~$ nscript
> #r "nuget: Automapper, 6.1.1"
> using AutoMapper;
> typeof(MapperConfiguration)
[AutoMapper.MapperConfiguration]
> #load "nuget: simple-targets-csx, 6.0.0";
> using static SimpleTargets;
> typeof(TargetDictionary)
[Submission#0+SimpleTargets+TargetDictionary]
```

### 多行模式

使用Roslyn语法解析，我们还支持多行REPL模式。这意味着，如果您有未完成的代码块并按 <kbd>Enter</kbd> 键，我们将自动进入多行模式。该模式由"*"字符指示。这对于声明类和其他更复杂的构造特别有用。

```
~$ nscript
> class Foo {
* public string Bar {get; set;}
* }
> var foo = new Foo();
```

### REPL 命令

除了常规 C# 脚本代码之外，还可以从 REPL 中调用以下命令（指令）：

| 命令  | 描述                                                  |
| -------- | ------------------------------------------------------------ |
| `#load`  | 将脚本加载到 REPL 中（与 CSX 中的"#load"用法相同）   |
| `#r`     | 将程序集加载到 REPL 中（与 CSX 中的"#r"用法相同）  |
| `#reset` | 将 REPL 重置回初始状态（无需重新启动） |
| `#cls`   | 清除控制台屏幕而不重置 REPL 状态    |
| `#exit`  | 退出 REPL                                               |

### 使用脚本文件作为 REPL 的种子

您可以执行一个 CSX 脚本，并在脚本结束时将自己放入 REPL 的上下文中。这样，REPL 就会与你的代码"设定种子"——所有类、方法或变量在 REPL 上下文中都可用。这是通过运行带有"-i"标志的脚本来实现的。

距离来说，对于下面的脚本:

```csharp
var msg = "Hello World";
Console.WriteLine(msg);
```

当您使用"-i"标志运行此命令时，将打印"Hello World"，REPL启动，并且 "msg" 变量在REPL上下文中可用。

```
~$ nscript foo.csx -i
Hello World
>
```

您还可以通过调用指向特定文件的"#load"指令，在 REPL 中加载脚本文件。例如：

```
~$ dotnet script
> #load "foo.csx"
Hello World
>
```

## 管线

下面的示例演示如何通过管道传入和传出脚本的数据。

"UpperCase.csx"脚本只是将标准输入转换为大写，然后将其写回标准输出。

```csharp
using (var streamReader = new StreamReader(Console.OpenStandardInput()))
{
    Write(streamReader.ReadToEnd().ToUpper());
}
```

现在，我们可以像这样简单地将一个命令的输出通过管道传输到脚本中。

```shell
echo "This is some text" | ncript UpperCase.csx
THIS IS SOME TEXT
```

### 调试

我们需要做的第一件事是将以下内容添加到`launch.config`文件中，该文件允许VS Code调试正在运行的进程。

```JSON
{
    "name": ".NET Core Attach",
    "type": "coreclr",
    "request": "attach",
    "processId": "${command:pickProcess}"
}
```

若要调试此脚本，我们需要一种方法在 VS Code 中附加调试器，而我们在这里可以做的最简单的事情就是通过在某个位置添加此方法来等待调试器附加。

```c#
public static void WaitForDebugger()
{
    Console.WriteLine("Attach Debugger (VS Code)");
    while(!Debugger.IsAttached)
    {
    }
}
```

要在从命令行执行脚本时调试脚本，我们可以执行类似操作。

```c#
WaitForDebugger();
using (var streamReader = new StreamReader(Console.OpenStandardInput()))
{
    Write(streamReader.ReadToEnd().ToUpper()); // <- SET BREAKPOINT HERE
}
```

现在，当我们从命令行运行脚本时，我们将得到

```shell
$ echo "This is some text" | dotnet script UpperCase.csx
Attach Debugger (VS Code)
```

现在，这使我们有机会在单步执行脚本之前附加调试器，并从 VS Code 中选择".NET Core Attach"调试器，然后选择表示正在执行脚本的进程。

一旦完成，我们应该看到我们的断点被击中。

## 配置(Debug/Release)

默认情况下，将使用"Debug"配置编译脚本。这是为了确保我们可以在 VS Code 中调试脚本，并为长时间运行的脚本附加调试器。

但是，在某些情况下，我们可能需要执行使用"release"配置编译的脚本。例如，除非使用"release"配置编译脚本，否则无法使用 [BenchmarkDotNet]（http://benchmarkdotnet.org/） 运行基准测试。

我们可以在执行脚本时指定配置。

```shell
nscript foo.csx -c release
```

## nullable

可以通过预编译指令开启 nullable.

```csharp
#!/usr/bin/env nscript

#nullable enable

string name = null;
```

尝试执行脚本将导致以下错误

```shell
main.csx(5,15): error CS8625: Cannot convert null literal to non-nullable reference type.
```

在 VS Code 中也会直接看到错误提示。

![image](https://user-images.githubusercontent.com/1034073/65727087-0e982600-e0b7-11e9-8fa0-d16331ab948a.png)

## 食用佐料

NScript 的宗旨是轻量，简单，追求极致的生产力。推荐搭配下面类库使用：

- [EmbedIO](https://github.com/unosquare/embedio) , A tiny, cross-platform, module based web server for .NET
- [LiteDB](https://github.com/mbdavid/LiteDB), A .NET NoSQL Document Store in a single data file 
- [Requests.NET](https://github.com/KrystianD/Requests.NET), Fluent C# HTTP client with python-requests like interface
- [Modern.Forms](https://github.com/modern-forms/Modern.Forms), Cross-platform spiritual successor to Winforms for .NET 6

NScript 的目标是高性能计算，常见的类库有：

- [opencvsharp](https://github.com/shimat/opencvsharp), OpenCV wrapper for .NET
- [FFmpeg.AutoGen](https://github.com/Ruslan-B/FFmpeg.AutoGen), FFmpeg auto generated unsafe bindings for C#/.NET and Core (Linux, MacOS and Mono).
- [TorchSharp](https://github.com/dotnet/TorchSharp),.NET bindings for the Pytorch engine
- [PaddleSharp](https://github.com/sdcb/PaddleSharp), .NET/C# binding for Baidu paddle inference library and PaddleOCR

进一步的资源：

- [NativeAOT](https://github.com/dotnet/runtimelab/tree/feature/NativeAOT), 后续 nscript 会加强对 NativeAOT 的支持;
- [zerosharp](https://github.com/MichalStrehovsky/zerosharp), 这是个非常棒的项目，演示了采用 C# 进行系统开发的可能性，移除GC后，您甚至可以写出 8k 大小的可执行程序。

后续加强对 AOT 和系统编程的支持，目标是为了方便云原生开发，使 nscript 成为优秀的云原生开发语言。

## 团队

`nscript` 的团队:

- [nscript](https://github.com/nscript-site/nscript)

`nscript`  源自 `dotnet script` ，下面是 `dotnet script` 的团队:

- [Bernhard Richter](https://github.com/seesharper) ([@bernhardrichter](https://twitter.com/bernhardrichter))
- [Filip W](https://github.com/filipw) ([@filip_woj](https://twitter.com/filip_woj))

## License

[MIT License](https://github.com/nscript-site/nscript/blob/master/LICENSE)

## 说明

本文档中部分内容修改自 `dotnet-script`，特此说明。