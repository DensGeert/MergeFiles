using System;
using System.Globalization;
using System.IO;

namespace Protime.Bespoke.Tools.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// parse string to integer value [default: 0]
        /// </summary>
        public static int ParseToInt(this string value)
        {
            return int.TryParse(value, out int result) ? result : 0;
        }

        /// <summary>
        /// parse string to double value [default: 0.0]
        /// </summary>
        public static double ParseToDouble(this string value)
        {
            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : 0;
        }
        
        /// <summary>
        /// parse string to double value [default: 0.0]
        /// </summary>
        public static double ParseToDouble(this string value, string format)
        {
            if (format.Contains(","))
                value = value.Replace(",", ".");
            
            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : 0;
        }


        /// <summary>
        /// parse string to bool value [default: false]
        /// </summary>
        public static bool ParseToBool(this string value)
        {
            return bool.TryParse(value, out bool result) && result;
        }

        /// <summary>
        /// parse string to char value [default: ';']
        /// </summary>
        public static char ParseToChar(this string value)
        {
            return char.TryParse(value, out char result) ? result : ';';
        }

        /// <summary>
        /// parse string to DateTime value for given format [default: DateTime.MinValue]
        /// </summary>
        public static DateTime ParseToDateTime(this string value, string format)
        {
            return DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result) ? result : DateTime.MinValue;
        }

        /// <summary>
        /// parse string to TotalMinutes[int] of TimeSpan with formats "HH.MM", "HH:MM", "HH.CC" or "HH:CC" [default: MMM]
        /// </summary>
        public static int ParseToDuration(this string value, string format)
        {
            var timespan = value.ParseToTimeSpan(format);
            return (int)timespan.TotalMinutes;
        }

        /// <summary>
        /// parse string to TimeSpan with formats "HH.MM", "HH:MM", "HH.CC" or "HH:CC" [default: MMM]
        /// </summary>
        public static TimeSpan ParseToTimeSpan(this string value, string format)
        {
            string[] vStrings;
            int hours = 0;
            int minutes = 0;

            switch (format.ToUpper())
            {
                case "HH.MM":
                    vStrings = value.Split('.');
                    if (vStrings.Length.Equals(2))
                    {
                        hours = vStrings[0].ParseToInt();
                        minutes = vStrings[1].ParseToInt();
                    }
                    break;
                case "HH:MM": 
                    vStrings = value.Split(':');
                    if (vStrings.Length.Equals(2))
                    {
                        hours = vStrings[0].ParseToInt();
                        minutes = vStrings[1].ParseToInt();
                    }
                    break;
                //Todo:not tested yet
                case "HHMM":
                    if (value.Length.Equals(4))
                    {
                        hours = int.Parse(value.Substring(0, 2));
                        minutes = int.Parse(value.Substring(2, 2));
                    }
                    break;
                case "HH.CC": 
                    vStrings = value.Split('.');
                    if (vStrings.Length.Equals(2))
                    {
                        hours = vStrings[0].ParseToInt();
                        vStrings[1] = vStrings[1].Length.Equals(1) ? $"{vStrings[1]}0" : vStrings[1];
                        minutes = (int) Math.Round((vStrings[1].ParseToInt() / 100.0) * 60,0);
                    }
                    break;
                case "HH:CC": 
                    vStrings = value.Split(':');
                    if (vStrings.Length.Equals(2))
                    {
                        hours = vStrings[0].ParseToInt();
                        vStrings[1] = vStrings[1].Length.Equals(1) ? $"{vStrings[1]}0" : vStrings[1];
                        minutes = (int) Math.Round((vStrings[1].ParseToInt() / 100.0) * 60,0);
                    }
                    break;
                //Todo:not tested yet
                case "HHCC":
                    if (value.Length.Equals(4))
                    {
                        hours = int.Parse(value.Substring(0, 2));
                        minutes = (int)Math.Round((int.Parse(value.Substring(2, 2)) / 100.0) * 60);
                    }
                    break;
                default: 
                    hours = 0;
                    minutes = value.ParseToInt();
                    break;
            }

            return new TimeSpan(hours, minutes, 0);
        }

        public static bool TryFormat(string format, out string result, params Object[] args)
        {
            try
            {
                result = string.Format(format, args);
                return true;
            }
            catch (FormatException)
            {
                result = string.Empty;
                return false;
            }
        }

        public static FileInfo GetFileInfo(this string fileValueText)
        {
            if (string.IsNullOrEmpty(fileValueText))
                return null;

            try
            {
                return new FileInfo(fileValueText);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
