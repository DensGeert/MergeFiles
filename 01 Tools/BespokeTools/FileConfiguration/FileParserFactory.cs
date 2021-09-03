using System.Text;

namespace Protime.Bespoke.Tools.FileConfiguration
{
    public static class FileParserFactory
    {
        public static IFileParser Create(FileType fileType)
        {
            if (fileType.Equals(FileType.TextDelimited))
                return new CsvFileParser();

            if (fileType.Equals(FileType.TextFixedPositions))
                return new TextFixedPositionFileParser();

            return null;
        }

        public static string[] ReadLines(string fileName, string encodingName)
        {
            if (string.IsNullOrEmpty(encodingName))
                return System.IO.File.ReadAllLines(fileName);
            
            var encoding = Encoding.GetEncoding(encodingName);
            return System.IO.File.ReadAllLines(fileName, encoding);
        }
    }
}