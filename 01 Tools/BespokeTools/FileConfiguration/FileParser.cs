using System.Collections.Generic;

namespace Protime.Bespoke.Tools.FileConfiguration
{
    public enum FileType { NoTypeDefined = 0, TextDelimited = 1, TextFixedPositions = 2 }

    public interface IFileParser
    {
        List<string[]> ReadRecords(string[] fileLines, FileLayout layout);
    }

    public class CsvFileParser : IFileParser
    {
        public List<string[]> ReadRecords(string[] fileLines, FileLayout layout)
        {
            List<string[]> records = new List<string[]>();

            for (int i = 0; i < fileLines.Length; i++)
            {
                if (layout.HasHeader && i.Equals(0))
                    continue;

                string[] lineValues = fileLines[i].Split(layout.Delimiter);

                if (layout.TrimFields)
                {
                    for (int f = 0; f < lineValues.Length; f++)
                    {
                        lineValues[f] = lineValues[f].TrimEnd();
                    }
                }

                records.Add(lineValues);
            }

            return records;
        }
    }

    public class TextFixedPositionFileParser : IFileParser
    {
        public List<string[]> ReadRecords(string[] fileLines, FileLayout layout)
        {
            List<string[]> records = new List<string[]>();
            for (int i = 0; i < fileLines.Length; i++)
            {
                if (layout.HasHeader && i.Equals(0))
                    continue;

                List<string> lineValues = new List<string>();
                foreach (var field in layout.FileMapping.Fields)
                {
                    if (field.Start >= 0)
                        lineValues.Add(fileLines[i].Substring(field.Start, field.Length));
                }

                if (layout.TrimFields)
                {
                    for (int f = 0; f < lineValues.Count; f++)
                    {
                        lineValues[f] = lineValues[f].TrimEnd();
                    }
                }

                records.Add(lineValues.ToArray());
            }

            return records;
        }
    }
}
