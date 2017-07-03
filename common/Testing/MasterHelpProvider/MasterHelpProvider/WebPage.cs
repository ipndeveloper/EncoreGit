using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WatiN.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace TestMasterHelpProvider
{
    public class WebPage : Page
    {
        public enum SearchType
        {
            ByName,
            ByID,
            ByText,
            ByClass,
            ByURL,
            ByValue,
            ByAlt
        }

        #region " Control Get Calls "

        public TextField GetTextField(Regex strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByID.Equals(objST))
                {
                    return Document.TextField(Find.ById(strValue));
                }
                else if (SearchType.ByName.Equals(objST))
                {
                    return Document.TextField(Find.ByName(strValue));
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for GetTextField.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }
        public TextField GetTextField(string strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByID.Equals(objST))
                {
                    return Document.TextField(Find.ById(strValue));
                }

                if (SearchType.ByName.Equals(objST))
                {
                    return Document.TextField(Find.ByName(strValue));
                }
                else if(SearchType.ByClass.Equals(objST))
                {
                    return Document.TextField(Find.ByClass(strValue));
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for GetTextField.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }

        public Button GetButton(Regex strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByName.Equals(objST))
                {
                    return Document.Button(Find.ByName(strValue));
                }
                else if (SearchType.ByID.Equals(objST))
                {
                    return Document.Button(Find.ById(strValue));
                }

                else
                {
                    throw new ApplicationException("Invalid SearchType for GetButton.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }
        public Button GetButton(string strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByName.Equals(objST))
                {
                    return Document.Button(Find.ByName(strValue));
                }

                else if (SearchType.ByID.Equals(objST))
                {
                    return Document.Button(Find.ById(strValue));
                }

                else if (SearchType.ByText.Equals(objST))
                {
                    return Document.Button(Find.ByText(strValue));
                }

                else
                {
                    throw new ApplicationException("Invalid SearchType for GetButton.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }

        public CheckBox GetCheckBox(Regex strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByID.Equals(objST))
                {
                    return Document.CheckBox(Find.ById(strValue));
                }

                else if (SearchType.ByName.Equals(objST))
                {
                    return Document.CheckBox(Find.ByName(strValue));
                }
                     
                else
                {
                    throw new ApplicationException("Invalid SearchType for GetCheckBox.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }
        public CheckBox GetCheckBox(string strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByID.Equals(objST))
                {
                    return Document.CheckBox(Find.ById(strValue));
                }

                else if (SearchType.ByName.Equals(objST))
                {
                    return Document.CheckBox(Find.ByName(strValue));
                }

                else
                {
                    throw new ApplicationException("Invalid SearchType for GetCheckBox.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }

        public Link GetLink(Regex strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByText.Equals(objST))
                {
                    return Document.Link(Find.ByText(strValue));
                }
                else if (SearchType.ByID.Equals(objST))
                {
                    return Document.Link(Find.ById(strValue));
                }
                else if (SearchType.ByClass.Equals(objST))
                {
                    return Document.Link(Find.ByClass(strValue));
                }
                else if (SearchType.ByURL.Equals(objST))
                {
                    return Document.Link(Find.ByUrl(strValue));
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for GetLink");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }
        public Link GetLink(string strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByText.Equals(objST))
                {
                    return Document.Link(Find.ByText(strValue));
                }
                else if (SearchType.ByID.Equals(objST))
                {
                    return Document.Link(Find.ById(strValue));
                }
                else if (SearchType.ByClass.Equals(objST))
                {
                    return Document.Link(Find.ByClass(strValue));
                }
                else if (SearchType.ByURL.Equals(objST))
                {
                    return Document.Link(Find.ByUrl(strValue));
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for GetLink");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }

        public Span GetSpan(Regex strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByText.Equals(objST))
                {
                    return Document.Span(Find.ByText(strValue));
                }
                else if (SearchType.ByID.Equals(objST))
                {
                    return Document.Span(Find.ById(strValue));
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for GetSpan.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }
        public Span GetSpan(string strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByText.Equals(objST))
                {
                    return Document.Span(Find.ByText(strValue));
                }
                else if (SearchType.ByID.Equals(objST))
                {
                    return Document.Span(Find.ById(strValue));
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for GetSpan.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }

        public RadioButton GetRadioButton(Regex strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByName.Equals(objST))
                {
                    return Document.RadioButton(Find.ByName(strValue));
                }
                else if (SearchType.ByValue.Equals(objST))
                {
                    return Document.RadioButton(Find.ByValue(strValue));
                }
                else if (SearchType.ByID.Equals(objST))
                {
                    return Document.RadioButton(Find.ById(strValue));
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for GetRadioButton.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }
        public RadioButton GetRadioButton(string strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByName.Equals(objST))
                {
                    return Document.RadioButton(Find.ByName(strValue));
                }
                else if (SearchType.ByValue.Equals(objST))
                {
                    return Document.RadioButton(Find.ByValue(strValue));
                }
                    
                else if (SearchType.ByID.Equals(objST))
                {
                    return Document.RadioButton(Find.ById(strValue));
                }

                else
                {
                    throw new ApplicationException("Invalid SearchType for GetRadioButton.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }

        public SelectList GetSelectList(Regex strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByID.Equals(objST))
                {
                    return Document.SelectList(Find.ById(strValue));
                }

                else if (SearchType.ByName.Equals(objST))
                {
                    return Document.SelectList(Find.ByName(strValue));
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for GetSelectList.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }
        public SelectList GetSelectList(string strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByID.Equals(objST))
                {
                    return Document.SelectList(Find.ById(strValue));
                }

                else if (SearchType.ByName.Equals(objST))
                {
                    return Document.SelectList(Find.ByName(strValue));
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for GetSelectList.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }

        public Div GetDiv(Regex strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByText.Equals(objST))
                {
                    return Document.Div(Find.ByText(strValue));
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for GetDiv.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }
        public Div GetDiv(string strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByText.Equals(objST))
                {
                    return Document.Div(Find.ByText(strValue));
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for GetDiv.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }

        public Image GetImage(Regex strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByAlt.Equals(objST))
                {
                    return Document.Image(Find.ByAlt(strValue));
                }
                else if (SearchType.ByID.Equals(objST))
                {
                    return Document.Image(Find.ById(strValue));
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for GetImage.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }
        public Image GetImage(string strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByAlt.Equals(objST))
                {
                    return Document.Image(Find.ByAlt(strValue));
                }
                else if (SearchType.ByID.Equals(objST))
                {
                    return Document.Image(Find.ById(strValue));
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for GetImage.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }

        public Frame GetFrame(Regex strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByID.Equals(objST))
                {
                    return Document.Frame(Find.ById(strValue));
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for GetFrame.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }
        public Frame GetFrame(string strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByID.Equals(objST))
                {
                    return Document.Frame(Find.ById(strValue));
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for GetFrame.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }

        #endregion

        #region " Control Exists Check "

        public bool CheckBoxExists(Regex strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByID.Equals(objST))
                {
                    return Document.CheckBox(Find.ById(strValue)).Exists;
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for CheckBoxExists.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }
        public bool CheckBoxExists(string strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByID.Equals(objST))
                {
                    return Document.CheckBox(Find.ById(strValue)).Exists;
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for CheckBoxExists.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }

        public bool LinkExists(Regex strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByText.Equals(objST))
                {
                    return Document.Link(Find.ByText(strValue)).Exists;
                }
                else if (SearchType.ByID.Equals(objST))
                {
                    return Document.Link(Find.ById(strValue)).Exists;
                }
                else if (SearchType.ByClass.Equals(objST))
                {
                    return Document.Link(Find.ByClass(strValue)).Exists;
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for LinkExists.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }
        public bool LinkExists(string strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByText.Equals(objST))
                {
                    return Document.Link(Find.ByText(strValue)).Exists;
                }
                else if (SearchType.ByID.Equals(objST))
                {
                    return Document.Link(Find.ById(strValue)).Exists;
                }
                else if (SearchType.ByClass.Equals(objST))
                {
                    return Document.Link(Find.ByClass(strValue)).Exists;
                }
                else if (SearchType.ByURL.Equals(objST))
                {
                    return Document.Link(Find.ByUrl(strValue)).Exists;
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for LinkExists.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }

        public bool SpanExists(Regex strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByText.Equals(objST))
                {
                    return Document.Span(Find.ByText(strValue)).Exists;
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for SpanExists.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }
        public bool SpanExists(string strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByText.Equals(objST))
                {
                    return Document.Span(Find.ByText(strValue)).Exists;
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for SpanExists.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }

        public bool TextFieldExists(Regex strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByID.Equals(objST))
                {
                    return Document.TextField(Find.ById(strValue)).Exists;
                }
                else if (SearchType.ByClass.Equals(objST))
                {
                    return Document.TextField(Find.ByClass(strValue)).Exists;
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for TextFieldExists");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }
        public bool TextFieldExists(string strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByID.Equals(objST))
                {
                    return Document.TextField(Find.ById(strValue)).Exists;
                }
                else if (SearchType.ByClass.Equals(objST))
                {
                    return Document.TextField(Find.ByClass(strValue)).Exists;
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for TextFieldExists");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }

        public bool ImageExists(Regex strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByAlt.Equals(objST))
                {
                    return Document.Image(Find.ByAlt(strValue)).Exists;
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for ImageExists.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }
        public bool ImageExists(string strValue, SearchType objST)
        {
            try
            {
                if (SearchType.ByAlt.Equals(objST))
                {
                    return Document.Image(Find.ByAlt(strValue)).Exists;
                }
                else
                {
                    throw new ApplicationException("Invalid SearchType for ImageExists.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
        }

        #endregion

    }
}
