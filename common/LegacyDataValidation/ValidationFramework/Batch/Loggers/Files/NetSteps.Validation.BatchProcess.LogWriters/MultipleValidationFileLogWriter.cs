using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.BatchProcess.Common;
using NetSteps.Validation.BatchProcess.LogWriters.Common;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.BatchProcess.LogWriters.Implementation
{
    public class MultipleValidationFileLogWriter : IMultipleValidationFileLogWriter
    {
        private readonly DateTime _startDate;
        private readonly int _maxRecordsPerFile;
        private readonly string _filePrefix;
        private readonly string _fileExtension;
        private readonly string _filePath;

        private int _fileNumber;
        private int _recordCount;
        private StreamWriter _activeWriter;
        private IRecordValidationSerializer _serializer;

        public MultipleValidationFileLogWriter(IRecordValidationSerializer serializer, int maxRecordsPerFile, string filePath, string filePrefix, string fileExtension)
        {
            _serializer = serializer;
            _maxRecordsPerFile = maxRecordsPerFile;
            _startDate = DateTime.Now;
            _filePrefix = filePrefix;
            _fileExtension = fileExtension;
            _filePath = filePath;
            _fileNumber = 1;

            EnsurePath();
        }

        private void EnsurePath()
        {
            if (!Directory.Exists(_filePath))
            {
                Directory.CreateDirectory(_filePath);
            }
        }

        public void Handle(IRecord record)
        {
            if (_activeWriter == null || _activeWriter.BaseStream == null)
            {
                _activeWriter = GetNewFileWriter();
            }
            _activeWriter.Write(_serializer.Serialize(record));
            if (_recordCount >= _maxRecordsPerFile)
            {
                Close();
                _recordCount = 0;
            }
            else
            {
                _recordCount++;    
            }
        }

        private StreamWriter GetNewFileWriter()
        {
            var fullFilePath = string.Format("{0}\\{1}({2}).{3}", _filePath, _filePrefix, _fileNumber, _fileExtension);
            var stream = new FileStream(fullFilePath, FileMode.OpenOrCreate);
            var writer = new StreamWriter(stream);
            _fileNumber++;
            return writer;
        }


        public void Close()
        {
            if (_activeWriter != null)
            {
                if (_activeWriter.BaseStream == null)
                {
                    _activeWriter.Flush();
                    _activeWriter.Close();
                }
                _activeWriter.Dispose();
            }
        }
    }
}
