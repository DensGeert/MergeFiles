using System.IO;
using System.Linq;
using Protime.Bespoke.Tools.Configuration;

namespace Protime.Bespoke
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmdParams = new CommandParams(args.ToArray());
            var sourceDirectory = cmdParams.GetStringValueByKey(@"\SOURCEDIRECTORY", true);
            var outputFile = cmdParams.GetStringValueByKey(@"\OUTPUTFILE", true);
            var extension = cmdParams.GetStringValueByKey(@"\EXTENSION", true);
            var hasHeader = cmdParams.GetBoolValueByKey(@"\HASHEADER");

            var csvDirectory = new DirectoryInfo(sourceDirectory);
            var files = csvDirectory.GetFiles($"*.{extension}");

            using (var writer = new StreamWriter(outputFile))
            {
                foreach (var file in files)
                {
                    using (var reader = new StreamReader(file.OpenRead()))
                    {
                        if (hasHeader)
                            if (file != files[0])
                                reader.ReadLine();
                        

                        while (!reader.EndOfStream)
                            writer.WriteLine(reader.ReadLine());
                    }
                }
            }

            //var sb = new StringBuilder();
            //foreach (var csvFile in csvFiles)
            //    using (var sr = new StreamReader(csvFile.OpenRead()))
            //    {
            //        sr.ReadLine(); // Discard header line
            //        while (!sr.EndOfStream)
            //            sb.AppendLine(sr.ReadLine());
            //    }
            //File.AppendAllText(Path.Combine(sourceDirectory, "output.csv"), sb.ToString());
        }
    }
}
