using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Protime.Bespoke.Tools.Logging;

namespace Protime.Bespoke.Tools.FileConfiguration
{
    public class FileWriter
    {
        private string _fileName;
        private string _lineHeader;
        private List<string> _lines;
        private readonly IPtLog _log;

        public FileWriter(string fileName, IPtLog log)
        {
            _fileName = fileName;
            _lines = new List<string>();
            _log = log;
        }

        public void AddHeader(string line)
        {
            _lineHeader = line;
        }

        public void AddLine(string line)
        {
            _lines.Add(line);

        }

        public void WriteLinesToFile()
        {
            if (_lines.Count.Equals(0))
            {
                _log?.Trace("No lines to write");
                return;
            }

            try
            {
                if (_fileName.Contains("{0}"))
                    _fileName = string.Format(_fileName, DateTime.Now.ToString("yyyyMMddHHmmss"));

                List<string> outputLines = new List<string>();
                if (!string.IsNullOrEmpty(_lineHeader))
                    outputLines.Add(_lineHeader);

                outputLines.AddRange(_lines);

                File.AppendAllLines(_fileName, outputLines, new UTF8Encoding(true));
                _log?.Info($"file {_fileName} [Header: {!string.IsNullOrEmpty(_lineHeader)}] created with {outputLines.Count} lines");
            }
            catch (Exception exception)
            {
                _log?.Error(
                    $"Couldn't create file {_fileName} [{_lines.Count} lines] => {exception.Message}");
            }
        }

        public void SortLines()
        {
            _log?.Trace("Sort lines");
            _lines = _lines.OrderBy(s => s).ToList();
        }
    }
}
