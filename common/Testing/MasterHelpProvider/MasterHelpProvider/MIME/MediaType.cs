using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Email.MIME
{
    public struct MediaType
    {
        public MediaEnum MediaEnum;
        public string SubType;
        public string FileExtension;
        public MediaType(MediaEnum MediaEnum, string SubType, string FileExtension)
        {
            this.MediaEnum = MediaEnum;
            this.SubType = SubType;
            this.FileExtension = FileExtension;
        }
    }
}