using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace NetSteps.Common.Utility
{
    public class TextFile : IEnumerable<string[]>
    {

        private string _fileName = string.Empty;
        private string _delimiter = string.Empty;
        private Stream _stream = null;

        public TextFile(string fileName, string delimiter)
        {
            this._fileName = fileName;
            this._delimiter = delimiter;
        }

        public TextFile(Stream stream, string delimiter)
        {
            this._stream = stream;
            this._delimiter = delimiter;
        }

        #region IEnumerable<string[]> Members

        IEnumerator<string[]> IEnumerable<string[]>.GetEnumerator()
        {
            if (this._stream == null)
            {
                using (StreamReader streamReader = new StreamReader(this._fileName))
                {
                    while (!streamReader.EndOfStream)
                    {
                        yield return streamReader.ReadLine().Split(this._delimiter.ToCharArray());
                    }
                }
            }
            else
            {
                using (StreamReader streamReader = new StreamReader(this._stream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        yield return streamReader.ReadLine().Split(this._delimiter.ToCharArray());
                    }
                }
            }

        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)((IEnumerator)this)).GetEnumerator();
        }

        #endregion
    }

}
