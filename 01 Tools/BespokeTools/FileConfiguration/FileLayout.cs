using System;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using Protime.Bespoke.Tools.Helpers;

namespace Protime.Bespoke.Tools.FileConfiguration
{
    public static class FileLayoutFactory
    {
        public static FileLayout Create(string jsonConfiguration)
        {
            if (string.IsNullOrEmpty(jsonConfiguration))
                throw new ConfigurationErrorsException("No filelayout configuration");

            return JsonConvert.DeserializeObject<FileLayout>(jsonConfiguration);
        }
    }

    public class FileLayout 
    {
        public FileType FileType;

        public string FileEncoding;

        public char Delimiter;

        public bool HasHeader;

        public bool TrimFields;

        public FileMapping FileMapping;
    }

    public class FileMapping
    {
        public Field[] Fields;

        public int MaxFieldsInRecord()
        {
            return Fields.Max(f => f.Start)+1;
        }

        public string GetFieldValue(string[] values, string fieldName)
        {
            string value = string.Empty;
            if (FieldExists(fieldName))
            {
                Field field = GetField(fieldName);

                if (field.Start >= 0)
                    value = values[field.Start];

                if (field.Merge != null)
                    StringExtensions.TryFormat(field.Merge, out value, values);

                if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(field.Default))
                    value = field.Default;
            }

            return value;
        }

        public string GetFieldDefaultValue(string[] values, string fieldName)
        {
            string value = string.Empty;
            if (FieldExists(fieldName))
            {
                Field field = GetField(fieldName);
                
                if (!string.IsNullOrEmpty(field.Default))
                    value = field.Default;
            }

            return value;
        }


        public DateTime GetFieldValueDateTime(string[] values, string fieldName)
        {
            if (!FieldExists(fieldName))
                return DateTime.MinValue;

            string value = GetFieldValue(values, fieldName);

            return value.ParseToDateTime(GetField(fieldName).Format ?? "");
        }

        public int GetFieldValueInteger(string[] values, string fieldName)
        {
            if (!FieldExists(fieldName))
                return 0;

            string value = GetFieldValue(values, fieldName);

            return value.ParseToInt();
        }

        public bool GetFieldValueBoolean(string[] values, string fieldName)
        {
            if (!FieldExists(fieldName))
                return false;

            string value = GetFieldValue(values, fieldName);
            return value.ParseToBool();
        }

        public int GetFieldValueDuration(string[] values, string fieldName)
        {
            if (!FieldExists(fieldName))
                return 0;

            string value = GetFieldValue(values, fieldName);
            
            return value.ParseToDuration(GetField(fieldName).Format ?? "");
        }

        public double GetFieldValueAmount(string[] values, string fieldName)
        {
            if (!FieldExists(fieldName))
                return 0;

            var value = GetFieldValue(values, fieldName);
            
            return value.ParseToDouble(GetField(fieldName).Format ?? "0.00");
        }

        public string GetFieldIdentifier(string fieldName)
        {
            if (!FieldExists(fieldName))
                return "";

            Field field = GetField(fieldName);
            return field.Identifier ?? "";
        }

        public string GetFieldFormat(string fieldName)
        {
            if (!FieldExists(fieldName))
                return "";

            Field field = GetField(fieldName);
            return field.Format ?? "";
        }

        public int GetFieldDefaultValue(string fieldName)
        {
            if (!FieldExists(fieldName))
                return 0;

            Field field = GetField(fieldName);
            return field.Default?.ParseToInt() ?? 0;
        }

        public string[] GetFieldMapperValues(string fieldName)
        {
            if (!FieldExists(fieldName))
                return new string[0];

            Field field = GetField(fieldName);

            return field.Mapper?? new string[0];
        }

        private Field GetField(string fieldName)
        {
            return Fields.FirstOrDefault(field => field.Name.ToUpper().Equals(fieldName.ToUpper()));
        }

        public bool FieldExists(string fieldName)
        {
            return Fields.Any(field => field.Name.ToUpper().Equals(fieldName.ToUpper()));
        }
    }

    public class Field
    {
        public string Name;

        public int Start;

        public int Length;

        public int Type;

        public string Format;

        public string Identifier;

        public string Merge;

        public string Default;

        public string Filter;

        public string[] Mapper;
    }
}
