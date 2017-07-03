using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Email.MIME
{
    public class CodeManager
    {
        private CodeManager()
        {
            Load();
        }

        private static CodeManager _Instance = null;
        private Dictionary<string, Code> Codes = new Dictionary<string, Code>();

        public static CodeManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new CodeManager();
                }
                return _Instance;
            }
        }

        private void Load()
        {
            Code Field = new CodeTypes.CodeBase();
            this["Subject"]= Field;
            this["Comments"]= Field;
            this["Content-Description"]= Field;

            Field = new CodeTypes.CodeAddress();
            this["From"]= Field;
            this["To"]= Field;
            this["Resent-To"]= Field;
            this["Cc"]= Field;
            this["Resent-Cc"]= Field;
            this["Bcc"]= Field;
            this["Resent-Bcc"]= Field;
            this["Reply-To"]= Field;
            this["Resent-Reply-To"]= Field;

            Field = new CodeTypes.CodeParameter();
            this["Content-Type"]= Field;
            this["Content-Disposition"]= Field;

            Field = new Code();
            this["7bit"]= Field;
            this["8bit"]= Field;

            Field = new CodeTypes.CodeBase64();
            this["base64"]= Field;

            Field = new CodeTypes.CodeQP();
            this["quoted-printable"]= Field;
        }

        public Code this[string Key]
        {
            get
            {
                Key = Key.ToLower();
                if (Codes.ContainsKey(Key))
                {
                    return Codes[Key];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Key = Key.ToLower();
                if (Codes.ContainsKey(Key))
                {
                    Codes[Key] = value;
                }
                else
                {
                    Codes.Add(Key, value);
                }
            }
        }
    }
}