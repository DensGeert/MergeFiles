using System;
using System.Collections.Generic;
using System.Globalization;

namespace Protime.Bespoke.Tools.Configuration
{
    public class CommandParams
    {
        private readonly Dictionary<string, string> _params;

        public CommandParams(string[] args)
        {
            _params = new Dictionary<string, string>();
            foreach (var arg in args)
            {
                var argItem = arg.Split('=');
                _params.Add(argItem[0], argItem.Length >= 2 ? argItem[1] : "true");
            }
        }

        public bool ParamExists(string key)
        {
            return _params.ContainsKey(key);
        }

        public string GetStringValueByKey(string key)
        {
            return _params.TryGetValue(key, out string value) ? value : string.Empty;
        }

        public string GetStringValueByKey(string key, bool required)
        {
            var value = GetStringValueByKey(key);

            if (string.IsNullOrEmpty(value) && required)
                throw new CommandLineArgsException($"Param {key} not found!");

            return value;
        }

        public bool GetBoolValueByKey(string key)
        {
            var stringValue = GetStringValueByKey(key);

            return bool.TryParse(stringValue, out bool value) && value;
        }

        public DateTime GetDateTimeValueByKey(string key, string dateFormat)
        {
            var stringValue = GetStringValueByKey(key);

            return DateTime.TryParseExact(stringValue, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datetimeValue) ? datetimeValue : DateTime.MinValue;
        }

        public DateTime GetDateTimeValueByKey(string key, string dateFormat, bool required)
        {
            var stringValue = GetStringValueByKey(key, required);

            return DateTime.TryParseExact(stringValue, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datetimeValue) ? datetimeValue : DateTime.MinValue;
        }

    }

    public class CommandLineArgsException : Exception
    {
        public CommandLineArgsException(string message) : base(message)
        {
        }
    }
}
