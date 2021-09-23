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

            var content = File.ReadAllLines(input).ToList();
            var noDupesContent = content.Distinct().ToList();
            File.WriteAllLines(output, noDupesContent);
        }
    }
}
