using NetSteps.Validation.BatchProcess.LogWriters.Common;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.BatchProcess.LogWriters.Implementation
{
    public class SingleValidationFileLogWriter : ISingleValidationFileLogWriter
    {
        private readonly DateTime _startDate;
        private readonly string _filePrefix;
        private readonly string _fileExtension;
        private readonly string _filePath;

        private StreamWriter _activeWriter;
        private IRecordValidationSerializer _serializer;

        public SingleValidationFileLogWriter(IRecordValidationSerializer serializer, string filePath, string filePrefix, string fileExtension)
        {
            _serializer = serializer;
            _startDate = DateTime.Now;
            _filePrefix = filePrefix;
            _fileExtension = fileExtension;
            _filePath = filePath;
            
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
        }

        private StreamWriter GetNewFileWriter()
        {
            var fullFilePath = string.Format("{0}\\{1}.{2}", _filePath, _filePrefix, _fileExtension);
            var stream = new FileStream(fullFilePath, FileMode.OpenOrCreate);
            var writer = new StreamWriter(stream);
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
