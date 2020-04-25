# NewAlgorithm

Sample shows step-by-step instructions from implementing simple algorithm to adding it to the `AlgorithmAnalysis` system.
At the end of this sample you will be able to extend default algorithm set and run analysis on your own implementations.

## Getting started

Prepare environment to work with project.
See requirements section [here](https://github.com/Vasar007/algorithm_analysis/tree/master/Examples).
Open your favorite IDE or any other program to work with source code.

You can download source code via cloning repository: `git clone https://github.com/Vasar007/algorithm_analysis.git`.

## Implementing algorithm

Let's start with minimal sample to add own algorithm module.
I'd like to notice that I try to cut the corners and keep things as simple as possible.
So, you should pay more attention to module architecture and your algorithm performance.

### Defining the algorithm

**Note:** Bubble Sort has already included in `AlgorithmAnalysis` system but this example shoes how you can add your own algorithm module.

I choose Bubble Sort to show you basic approach how `AlgorithmAnalysis` system works with external modules and how you can include own algorithm module in the system.
Suppose that we should perform empirical analysis for Bubble Sort.
Input data for out algorithm is one-dimensional arrays of arbitrary length.
Assume the main purpose is determining complexity function behavior on segment of array lengths [10, 500].
Also step is equal to 10.

As you may know, `AlgorithmAnalysis` system uses probabilistic approach to analyze algorithms.
Read more about this approach [here](https://github.com/Vasar007/algorithm_analysis/blob/master/Article/Application%20of%20probabilistic%20analysis%20to%20the%20problem%20of%20finding%20the%20shortest%20route/article.pdf) (original [article](http://www.isa.ru/jitcs/images/stories/2009/02/23_37.pdf)).

Well, let's allow system analyze algorithm on subsegment [10, 100] and then extrapolate results on the whole segment [10, 500].

### Input arguments

The `AlgorithmAnalysis` system will execute algorithm module and pass following positional input arguments:

`<algorithm_type> <start_value> <end_value> <launches_number> <step> <output_filename_pattern>`

**Note:** now the `AlgorithmAnalysis` system uses only positional arguments, named arguments are not supported.

Meaning of arguments:

- `algorithm_type`: integer value which defined in configuration file.
  System reads configuration file and gives the you a choice to select and execute any algorithm defined there.
  We return to this parameter later.
- `start_value`: integer value which defined in UI.
  It specifies lower bound of input data size of the segment to analyze.
- `end_value`: integer value which defined in UI.
  It specifies upper bound of input data size of the segment to analyze _(not the extrapolation upper bound)_.
  Notice that `end_value` ≥ `start_value`.
- `launches_number`: integer value which defined in UI.
  It specifies launches number of the algorithm.
- `step`: integer value which defined in UI.
  It specifies increment value which we will use to advance from `start_value` to `end_value`.
  Notice that _system doesn't check_ that `end_value` = `start_value` + `step` * i, so you can use any step value as you want.
- `output_filename_pattern`: string value which defined in configuration file.
  It specifies pattern which helps system to find and parse file with experiments results.
  System expects that our module will save results in file with names `{output_filename_pattern}{input_data_size}.txt` (I added `{}` for readability), where `input_data_size` is value from `start_value` to `end_value` segment.

**Note:** you can change extension of output files in configuration through `OutputFileExtension` key.

Example of argument pack: `0 10 100 200 10 output_`.

Values:

- `algorithm_type`: 0
- `start_value`: 10
- `end_value`: 100
- `launches_number`: 200
- `step`: 10
- `output_filename_pattern`: output_

To sum up, our module should:

- accept and parse input arguments;
- iterate through [`start_value`, `end_value`] segment with `step` on every iteration:
  - execute algorithm exactly `launches_number` times with `input_data_size` from segment;
  - calculate operations number which algorithm spend;
  - save values from previous step to the file with name `{output_filename_pattern}{input_data_size}.txt`;
- exit and release all acquired resources.

Sounds complicated?
Well, I don't think so but don't try to understand everything at once.
I suggest to move further and see how it looks in real life.

Finally, notice that we will ignore `algorithm_type` parameter because our module implements only one algorithm.
By design, this parameter allows authors include several algorithms in the same module for simplicity.

### Minimal viable sample

Enough of words, jump to opened IDE and start coding.

Create project folder and enter into it.
Create C# console app project using command `dotnet new console`:

![dotnet new console](https://github.com/Vasar007/algorithm_analysis/blob/master/Media/dotnet_new_console.jpg?raw=true)

Open file `Program.cs` and paste follow code:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace AlgorithmAnalysis.Examples.NewAlgorithm
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Module started.");
                ProcessArgs(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred:{Environment.NewLine}{ex}");
                Console.WriteLine("Press any key to close...");
                Console.ReadKey();
            }
            finally
            {
                Console.WriteLine("Module closed.");
            }
        }

        private static void ProcessArgs(IReadOnlyList<string> args)
        {
            // args: <algorithm_type> <start_value> <end_value> <launches_number> <step> <output_filename_pattern>
            // Ignore <algorithm_type> because our module implements only one algorithm.

            int startValue = int.Parse(args[1]);
            int endValue = int.Parse(args[2]);
            int launchesNumber = int.Parse(args[3]);
            int step = int.Parse(args[4]);
            string outputFilenamePattern = args[5];

            // Iterate through segment
            for (int inputDataSize = startValue; inputDataSize <= endValue; inputDataSize += step)
            {
                Console.WriteLine($"Size: {inputDataSize.ToString()}");
                // Launch specified number of times algorithm and save operations numbers.
                var results = new List<int>();
                for (int launch = 1; launch <= launchesNumber; ++launch)
                {
                    int[] array = GenerateArray(inputDataSize);
                    int operationsNumber = BubbleSort(array);
                    results.Add(operationsNumber);

                    Console.WriteLine($"Execution: {launch.ToString()}; Operations number: {operationsNumber.ToString()}");
                }

                // Create filename and save all results on current iteration.
                string filename = $"{outputFilenamePattern}{inputDataSize.ToString()}.txt";
                SaveResults(results, filename);
            }
        }

        private static int[] GenerateArray(int arrayLength)
        {
            var random = new Random();
            return Enumerable.Range(1, arrayLength)
                .Select(i => random.Next())
                .ToArray();
        }

        private static int BubbleSort(int[] array)
        {
            int operationNumber = 0;
            for (int j = 0; j <= array.Length - 2; ++j)
            {
                for (int i = 0; i <= array.Length - 2; ++i)
                {
                    if (array[i] > array[i + 1])
                    {
                        int temp = array[i + 1];
                        array[i + 1] = array[i];
                        array[i] = temp;

                        // Here we increment operations number.
                        ++operationNumber;
                    }
                }
            }

            return operationNumber;
        }

        private static void SaveResults(IReadOnlyList<int> results, string filename)
        {
            var stringBuilder = new StringBuilder(results.Count).AppendLine("Operations number");

            foreach (int result in results)
            {
                stringBuilder.AppendLine(result.ToString());
            }

            File.AppendAllText(filename, stringBuilder.ToString());
        }
    }
}

```

Build and create module app using command `dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true`:

![dotnet publish](https://github.com/Vasar007/algorithm_analysis/blob/master/Media/dotnet_publish.jpg?raw=true)

**Note:** I build module for OS Windows and x64 platform!

Well, now we have built module in `bin\Release\netcoreapp3.1\win-x64\publish\` folder.
Executable file will have the same name as folder where `Program.cs` exists.
I rename final module to "AlgorithmAnalysis.Examples.NewAlgorithm.exe" but you can name module as you want.
But do not forget to set correct "Analysis program name" (or "AnalysisProgramName" in configuration file) value when you will add new algorithm!

## Use module

So, we almost finished our way.
Copy "BubbleSort.exe" module to the folder with `AlgorithmAnalysis` app.

Start `AlgorithmAnalysis` app and select `Algorithm` combobox.
You can see the picture:

![UI before adding](https://github.com/Vasar007/algorithm_analysis/blob/master/Media/before_adding.png?raw=true)

There are 2 ways to add new algorithm implementation:

- open app settings and add algorithm through UI;
- manually add new algorithm through configuration file.

### Add algorithm through UI (easy way)

1) Click on three dots in right corner, in popup menu select "Settings":
![Open settings](https://github.com/Vasar007/algorithm_analysis/blob/master/Media/open_settings.png?raw=true)

2) Click "View" button on the "Analysis" settings panel to open algorithm settings:
![View algorithms](https://github.com/Vasar007/algorithm_analysis/blob/master/Media/view_algorithms.png?raw=true)

3) Click "Add" button to create algorithm template:
![Add algorithm](https://github.com/Vasar007/algorithm_analysis/blob/master/Media/add_algorithm.png?raw=true)

4) Fill algorithm parameters and click "Ok" button to save added algorithm:
![View algorithm](https://github.com/Vasar007/algorithm_analysis/blob/master/Media/save_added_algorithm.png?raw=true)

5) Click "Apply" button to save and apply new settings:
![View algorithm](https://github.com/Vasar007/algorithm_analysis/blob/master/Media/save_settings.png?raw=true)

Open `config.json` and paste code below.

### Add algorithm through configuration file (hard way)

App has configuration file with specified or default options.
All this options are saved to configuration file.
Although Algorithm Analysis can work without configuration file (in that case app will use default options) almost always this file exists.
You can open configuration file though UI:

1) Click on three dots in right corner, in popup menu select "Settings":

![Open settings](https://github.com/Vasar007/algorithm_analysis/blob/master/Media/open_settings.png?raw=true)

2) Click "Open configuration file" button:

![Open config](https://github.com/Vasar007/algorithm_analysis/blob/master/Media/open_config.png?raw=true)

Open `config.json` and paste code below.

<details>
<summary><strong>Content of config file [click to expand]</strong></summary>
<p>

```json
{
  "AppearanceOptions": {
    "Theme": "Light"
  },

  "AnalysisOptions": {
    "AvailableAlgorithms": [
      {
        "Description": "Pallottino's algorithm",
        "MinFormula": "x",
        "AverageFormula": "x * x * (x - 1) / 2",
        "MaxFormula": "x * x * x * (x - 1) / 2",
        "AnalysisProgramName": "algorithm_analysis.exe",
        "OutputFilenamePattern": "{SpecialFolder.CommonApplicationData}/AlgorithmAnalysis/data/tests_average_"
      },
      {
        "Description": "Insertion sort",
        "MinFormula": "x",
        "AverageFormula": "x^2",
        "MaxFormula": "x^2",
        "AnalysisProgramName": "algorithm_analysis.exe",
        "OutputFilenamePattern": "{SpecialFolder.CommonApplicationData}/AlgorithmAnalysis/data/tests_average_"
      },
      {
        "Description": "Bubble sort",
        "MinFormula": "x",
        "AverageFormula": "x^2",
        "MaxFormula": "x^2",
        "AnalysisProgramName": "AlgorithmAnalysis.Examples.NewAlgorithm.exe",
        "OutputFilenamePattern": "{SpecialFolder.CommonApplicationData}/AlgorithmAnalysis/data/tests_average_"
      }
    ],
    "CommonAnalysisFilenameSuffix": "series",
    "OutputFileExtension": ".txt"
  },

  "ReportOptions": {
    "CellCreationMode": "Centerized",
    "LibraryProvider": "EPPlus",
    "ExcelVersion": "V2007",
    "OutputReportFilePath": "{SpecialFolder.CommonApplicationData}/AlgorithmAnalysis/results/results.xlsx"
  },

  "LoggerOptions": {
    "LogFolderPath": "{SpecialFolder.CommonApplicationData}/AlgorithmAnalysis/logs",
    "EnableLogForExcelLibrary": "false",
    "LogFilesExtension": ".log",
    "LogFilenameSeparator": "-",
    "UseFullyQualifiedEntityNames": "false"
  }
}

```

</p>
</details>

Pay attention that section "AvailableAlgorithms" contains "Bubble sort" algorithm and its options.

When you finished with configuration file, you should restart app or click "Reload" button on notification at the bottom of window:

![Reload changes](https://github.com/Vasar007/algorithm_analysis/blob/master/Media/reload_changes.png?raw=true)

### See the results

Start `AlgorithmAnalysis` app and select `Algorithm` combobox again.
Now you can select "Bubble sort" in algorithms:

![UI after adding](https://github.com/Vasar007/algorithm_analysis/blob/master/Media/after_adding.png?raw=true)

Well, you can specify parameters, execute analysis and see the results.
