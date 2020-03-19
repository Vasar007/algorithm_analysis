# NewAlgorithm

Sample shows step-by-step instructions from implementing simple algorithm to adding it to the `AlgorithmAnalysis` system.
At the end of this sample you will be able to extend default algorithm set and run analysis on your own implementations.

## Getting started

Prepare environment to work with project.
See requirements section [here](https://github.com/Vasar007/algorithm_analysis/tree/master/Examples).
Open your favorite IDE or any other program to work with source code.

Download source code via cloning repository: `git clone https://github.com/Vasar007/algorithm_analysis.git`.

## Implementing algorithm

Let's start with minimal sample to add own algorithm module.
I'd like to notice that I try to cut the corners and keep things as simple as possible.
So, you should pay more attention to module architecture and your algorithm performance.

### Defining the algorithm

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
Let's rename final module to "BubbleSort.exe".

## Use module

So, we almost finished our way.
But you should build `AlgorithmAnalysis` solution (TODO: add `AlgorithmAnalysis` installer).
Copy "BubbleSort.exe" module to the folder with `AlgorithmAnalysis` app.

Start `AlgorithmAnalysis` app and select `Algorithm` combobox.
You can see the following picture:

![UI before adding](https://github.com/Vasar007/algorithm_analysis/blob/master/Media/before_adding.jpg?raw=true)

Open `config.json` and paste following code:

```json
{
  "AnalysisOptions": {
    "AvailableAlgorithms": [
      {
        "Description": "Pallottino's algorithm",
        "Value": "0",
        "MinFormula": "x",
        "AverageFormula": "x * x * (x - 1) / 2",
        "MaxFormula": "x * x * x * (x - 1) / 2",
        "AnalysisProgramName": "algorithm_analysis.exe",
        "OutputFilenamePattern": "output_"
      },
      {
        "Description": "Insertion sort",
        "Value": "1",
        "MinFormula": "x",
        "AverageFormula": "x^2",
        "MaxFormula": "x^2",
        "AnalysisProgramName": "algorithm_analysis.exe",
        "OutputFilenamePattern": "output_"
      },
      {
        "Description": "Bubble sort",
        "Value": "2",
        "MinFormula": "x",
        "AverageFormula": "x^2",
        "MaxFormula": "x^2",
        "AnalysisProgramName": "BubbleSort.exe",
        "OutputFilenamePattern": "output_"
      }
    ],
    "CommonAnalysisFilenameSuffix": "series",
    "OutputFileExtension": ".txt"
  },

  "ExcelOptions": {
    "CellCreationMode": "Centerized",
    "LibraryProvider": "EPPlus",
    "Version": "V2007",
    "OutputExcelFilename": "results.xlsx"
  },

  "LoggerOptions": {
    "RelativeLogFolderPath": "logs",
    "EnableLogForExcelLibrary": "false"
  }
}

```

Start `AlgorithmAnalysis` app and select `Algorithm` combobox again.
Now you can select "Bubble sort" in algorithms and execute analysis:

![UI after adding](https://github.com/Vasar007/algorithm_analysis/blob/master/Media/after_adding.jpg?raw=true)

The end.
