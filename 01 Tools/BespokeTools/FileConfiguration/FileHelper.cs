using System;

namespace Protime.Bespoke.Tools.FileConfiguration
{
    public static class FileHelper
    {
        public static FileType ParseToFileType(this string value)
        {
            try
            {
                FileType type = (FileType)Enum.Parse(typeof(FileType), value);

                if (Enum.IsDefined(typeof(FileType), type))
                    return type;
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"{value} is not an underlying value of the FileType enumeration.", exception);
            }

            return FileType.NoTypeDefined;
        }
    }
}
