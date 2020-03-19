# algorithm_analysis

[![License](https://img.shields.io/hexpm/l/plug.svg)](https://github.com/Vasar007/algorithm_analysis/blob/master/LICENSE)
[![AppVeyor branch](https://img.shields.io/appveyor/ci/Vasar007/algorithm-analysis/master.svg)](https://ci.appveyor.com/project/Vasar007/algorithm-analysis)
[![Travis (.org)](https://travis-ci.com/Vasar007/algorithm_analysis.svg?branch=master)](https://travis-ci.com/Vasar007/algorithm_analysis)

[![GitHub wiki](https://img.shields.io/badge/Docs-GitHub%20wiki-brightgreen)](https://github.com/Vasar007/algorithm_analysis/wiki)

Empirical analysis of algorithms.
Now you may think about standard empirical analysis methods like calculating theoretical complexity function on average.
However, this approach has some downsides.
The latest stay-of-the-art solution provides more complex analysis method (see original [article](http://www.isa.ru/jitcs/images/stories/2009/02/23_37.pdf)).

Read more about this approach [here](https://github.com/Vasar007/algorithm_analysis/blob/master/Article/Application%20of%20probabilistic%20analysis%20to%20the%20problem%20of%20finding%20the%20shortest%20route/article.pdf).

In short, method includes two stages — the stage of preliminary research, the purpose of which is to test the hypothesis about the law of distribution of the algorithm’s labor intensity values as a discrete limited random variable, and the stage of the main study, which determines the values of the confidence labor intensity `𝑓𝛾(𝑛)` as a function of the input length of the algorithm.

So, this repository contains system that allow you to fully automate such empirical analysis.
Provide analysis to system, have a rest, relax, make a cup of coffee and get analysis result report upon completion!

## Usage examples

Check examples [here](https://github.com/Vasar007/algorithm_analysis/tree/master/Examples/).

## Dependencies

Target .NET Standard is 2.1 for libraries, .NET Core is 3.1 for desktop app.
Version of C# is 8.0.
You can install all .NET dependencies using NuGet package manager.

Target C++ part of project is used C++17 standard.
No external C++ dependencies are used.

## Set up project guide

You can read full instruction in project [Wiki](https://github.com/Vasar007/algorithm_analysis/wiki/Set-up-project).

## License information

This project is licensed under the terms of the [Apache License 2.0](LICENSE).

### Third party software and libraries used

<details>
<summary><strong>Table of content [click to expand]</strong></summary>
<p>

#### [Acolyte.NET](https://github.com/Vasar007/Acolyte.NET)

Copyright © 2020 Vasily Vasilyev (vasar007@yandex.ru)

License: [Apache License 2.0](https://github.com/Vasar007/Acolyte.NET/blob/master/LICENSE)

#### [NLog](https://github.com/NLog/NLog)

Copyright © 2004-2020 Jaroslaw Kowalski (jaak@jkowalski.net), Kim Christensen, Julian Verdurmen

License: [BSD 3-Clause](https://github.com/NLog/NLog/blob/dev/LICENSE.txt)

#### [Math.NET Numerics](https://github.com/mathnet/mathnet-numerics)

Copyright © 2002-2019 Math.NET

License: [MIT/X11](https://github.com/mathnet/mathnet-numerics/blob/master/LICENSE.md)

#### [EPPlus 5](https://github.com/EPPlusSoftware/EPPlus)

Copyright © EPPlus Software AB

License: [PolyForm Noncommercial License 1.0.0](https://polyformproject.org/licenses/noncommercial/1.0.0/)

#### [Material Design In XAML Toolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)

Copyright © James Willock, Mulholland Software and Contributors

License: [MIT License](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit/blob/master/LICENSE)

#### [Prism](https://github.com/PrismLibrary/Prism)

Copyright © .NET Foundation

License: [MIT License](https://github.com/PrismLibrary/Prism/blob/master/LICENSE)

#### [FileHelpers](https://github.com/MarcosMeli/FileHelpers)

Copyright © 2003-2015 Marcos Meli (www.filehelpers.net)

License: [MIT License](https://github.com/MarcosMeli/FileHelpers/blob/master/LICENSE.txt)

#### [Microsoft.Extensions.Configuration](https://github.com/aspnet/Extensions)

Copyright © .NET Foundation and Contributors

License: [Apache License 2.0](https://licenses.nuget.org/Apache-2.0)

#### [NPOI](https://github.com/tonyqus/npoi/blob/master/LICENSE)

Copyright © Tony Qu and Contributors

License: [Apache License 2.0](https://github.com/tonyqus/npoi/blob/master/LICENSE)

</p>
</details>
