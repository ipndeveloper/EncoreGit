using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Email.MIME.CodeTypes
{
    public class CodeBase:Code
    {
        public CodeBase()
        {
        }

        public override void Decode(string Input, out string Output)
        {
            Output = String.Empty;
            int Index = 0;
            while(Index<Input.Length)
            {
                int CurrentIndex=Input.IndexOf("=?",Index);
                if(CurrentIndex!=-1)
                {
                    Output+=Input.Substring(Index,CurrentIndex-Index);
                    int CurrentIndex2 = Input.IndexOf("?=", CurrentIndex + 2);
                    if(CurrentIndex2!=-1)
                    {
                        CurrentIndex+=2;
                        int CurrentIndex3 = Input.IndexOf('?', CurrentIndex);
                        if(CurrentIndex3!=-1&&Input[CurrentIndex3+2]=='?')
                        {
                            CharacterSet=Input.Substring(CurrentIndex,CurrentIndex3-CurrentIndex);
                            string DECString=Input.Substring(CurrentIndex3+3,CurrentIndex2-CurrentIndex3-3);
                            if(Input[CurrentIndex3+1]=='Q')
                            {
                                Code TempCode=CodeManager.Instance["quoted-printable"];
                                TempCode.CharacterSet=CharacterSet;
                                string TempString=String.Empty;
                                TempCode.Decode(DECString,out TempString);
                                Output+=TempString;
                            }
                            else if(Input[CurrentIndex3+1]=='B')
                            {
                                Code TempCode=CodeManager.Instance["base64"];
                                TempCode.CharacterSet=CharacterSet;
                                string TempString=String.Empty;
                                TempCode.Decode(DECString,out TempString);
                                Output+=TempString;
                            }
                            else
                            {
                                Output+=DECString;
                            }
                        }
                        else
                        {
                            Output+=Input.Substring(CurrentIndex3,CurrentIndex2-CurrentIndex3);
                        }
                        Index=CurrentIndex2+2;
                    }
                    else
                    {
                        Output+=Input.Substring(CurrentIndex,Input.Length-CurrentIndex);
                        break;
                    }
                }
                else
                {
                    Output+=Input.Substring(Index,Input.Length-Index);
                    break;
                }
            }
        }

        protected virtual string[] FoldCharacters
        {
            get { return null; }
        }

        protected virtual bool IsAutoFold
        {
            get { return false; }
        }

        protected virtual bool DelimeterNeeded
        {
            get { return false; }
        }

        protected virtual char[] DelimeterCharacters
        {
            get { return null; }
        }

        protected string EncodeDelimeter(string Input)
        {
            StringBuilder Builder = new StringBuilder();
            char[] Filter = DelimeterCharacters;
            string[] InputArray = Input.Split(Filter);
            int Index = 0;
            foreach (string TempString in InputArray)
            {
                if (TempString != null)
                {
                    Index += TempString.Length;
                    if (string.IsNullOrEmpty(CharacterSet))
                    {
                        CharacterSet = System.Text.Encoding.Default.BodyName;
                    }
                    string EncodingUsing = SelectEncoding(Input).ToLower();
                    if (EncodingUsing.Equals("non", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Builder.Append(TempString);
                    }
                    else if (EncodingUsing.Equals("base64", StringComparison.InvariantCultureIgnoreCase)
                        || EncodingUsing.Equals("quoted-printable", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Code TempCode = CodeManager.Instance[EncodingUsing];
                        TempCode.CharacterSet = CharacterSet;
                        Builder.AppendFormat("=?{0}?Q?{1}?=", CharacterSet, TempCode.Encode(TempString));
                    }
                    if (Index < Input.Length)
                        Builder.Append(Input.Substring(Index, 1));
                    ++Index;
                }
            }
            return Builder.ToString();
        }

        protected string EncodeNoDelimeter(string Input)
        {
            StringBuilder Builder = new StringBuilder();
            if(string.IsNullOrEmpty(CharacterSet))
                CharacterSet = System.Text.Encoding.Default.BodyName;

            string EncodingUsing = SelectEncoding(Input).ToLower();
            if (EncodingUsing.Equals("non", StringComparison.InvariantCultureIgnoreCase))
            {
                Builder.Append(Input);
            }
            else if (EncodingUsing.Equals("base64", StringComparison.InvariantCultureIgnoreCase)
                        || EncodingUsing.Equals("quoted-printable", StringComparison.InvariantCultureIgnoreCase))
            {
                Code TempCode = CodeManager.Instance[EncodingUsing];
                TempCode.CharacterSet = CharacterSet;
                Builder.AppendFormat("=?{0}?Q?{1}?=", CharacterSet, TempCode.Encode(Input));
            }
            return Builder.ToString();
        }

        public override string Encode(string Input)
        {
            StringBuilder Builder = new StringBuilder();
            if (DelimeterNeeded)
            {
                Builder.Append(EncodeDelimeter(Input));
            }
            else
            {
                Builder.Append(EncodeNoDelimeter(Input));
            }

            if (IsAutoFold)
            {
                string[] FoldCharacters = this.FoldCharacters;
                foreach (string FoldCharacter in FoldCharacters)
                {
                    string NewFoldString = FoldCharacter + "\r\n\t";
                    Builder.Replace(FoldCharacter, NewFoldString);
                }
            }
            return Builder.ToString();
        }

        protected string SelectEncoding(string Input)
        {
            int NumberOfNonASCII = 0;
            for (int x = 0; x < Input.Length; ++x)
            {
                if (IsNonASCIICharacter(Input[x]))
                    ++NumberOfNonASCII;
            }
            if (NumberOfNonASCII == 0)
                return "non";
            int QuotableSize = Input.Length + NumberOfNonASCII * 2;
            int Base64Size = (Input.Length + 2) / 3 * 4;
            return (QuotableSize <= Base64Size || NumberOfNonASCII * 5 <= Input.Length) ? "quoted-printable" : "base64";
        }

        protected bool IsNonASCIICharacter(char Input)
        {
            return (int)Input > 255;
        }
    }
}