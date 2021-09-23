using System;
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
            var input = cmdParams.GetStringValueByKey(@"\INPUTFILE", true);
            var output = cmdParams.GetStringValueByKey(@"\OUTPUTFILE", true);
            var hasHeader = cmdParams.GetBoolValueByKey(@"\HASHEADER");

            var content = File.ReadAllLines(input);
            if (hasHeader)
            {
                var header = content[0];
                string[] headerArray = { header };
                content = content.Skip(1).ToArray();
                Array.Sort(content);
                var fullContent = headerArray.Concat(content).ToArray();
                File.WriteAllLines(output, fullContent);
            }
            else
            {
                Array.Sort(content);
                File.WriteAllLines(output, content);
            }
        }
    }
}
