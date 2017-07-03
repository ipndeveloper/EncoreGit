using System;
using System.Collections;
using System.Web.UI.WebControls;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities;
using NetSteps.Web.Controls;

namespace NetSteps.Web.Business.Controls
{
    public class ContentControl : WebControl
    {
        #region Properties

        private NetSteps.Common.Constants.ViewingMode _pageMode;
        public NetSteps.Common.Constants.ViewingMode PageMode
        {
            get { return _pageMode; }
            set { _pageMode = value; }
        }

        private HtmlContent _content;
        public HtmlContent Content
        {
            get { return _content; }
            set { _content = value; }
        }

        private int _sectionId;
        public int SectionId
        {
            get { return _sectionId; }
            set { _sectionId = value; }
        }

        private NetSteps.Data.Entities.Constants.HtmlSectionEditType _editType = NetSteps.Data.Entities.Constants.HtmlSectionEditType.CorporateOnly;
        public NetSteps.Data.Entities.Constants.HtmlSectionEditType EditType
        {
            get { return _editType; }
            set { _editType = value; }
        }

        private bool _useNewEditWrapper;
        public bool UseNewEditWrapper
        {
            get { return _useNewEditWrapper; }
            set { _useNewEditWrapper = value; }
        }

        private bool _useSurroundImagesWithLinks;
        public bool UseSurroundImagesWithLinks
        {
            get { return _useSurroundImagesWithLinks; }
            set { _useSurroundImagesWithLinks = value; }
        }

        #endregion

        public override void RenderControl(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.Content == null) return;

            string contentToOutput = ReplaceMarkupTokens();

            writer.Write(contentToOutput);
        }

        public string GetBackOfficeUrl()
        {
            string backOfficeURL = WebContext.CurrentSite.BackOfficeUrl;
            if (string.IsNullOrEmpty(backOfficeURL))
            {
                backOfficeURL = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.BackOfficeUrl);
            }
            return backOfficeURL;
        }

        public static string SurroundImagesWithLinks(string htmlSource)
        {
            Hashtable hashInsertions = new Hashtable();
            string returnValue = htmlSource;

            string sImgTagStart = "<img";
            string sImgTagClose = "<!--endimage-->";
            string sSrcStart = "src=\"";
            string sSrcEnd = "\"";
            string sSrcValue = string.Empty;

            int iImgTagStart = htmlSource.IndexOf(sImgTagStart);
            int iImgTagEnd = 0;
            int iSrcStart = 0;
            int iSrcEnd = 0;
            while (iImgTagStart > 0)
            {
                sSrcValue = string.Empty;
                iImgTagEnd = htmlSource.IndexOf(sImgTagClose, iImgTagStart) + sImgTagClose.Length;
                if (iImgTagEnd > iImgTagStart)
                {
                    //find src
                    iSrcStart = htmlSource.IndexOf(sSrcStart, iImgTagStart) + sSrcStart.Length;
                    if (iSrcStart > 0)
                    {
                        iSrcEnd = htmlSource.IndexOf(sSrcEnd, iSrcStart);
                        if (iSrcEnd > iSrcStart)
                            sSrcValue = htmlSource.Substring(iSrcStart, (iSrcEnd - iSrcStart)).Replace("<!--imagepath-->", ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.ImagesWebPath));
                    }

                    if (!string.IsNullOrEmpty(sSrcValue))
                    {
                        hashInsertions.Add(iImgTagStart, ("<a href=\"" + sSrcValue + "\">"));

                        if (!hashInsertions.ContainsKey(iImgTagEnd))
                        {
                            hashInsertions.Add(iImgTagEnd, ("</a>"));
                        }
                        else
                        {
                            //TODO: Fix for Duplicate Cody Help
                            //hashInsertions.Add(iImgTagEnd, ("</a>"));
                        }
                    }
                }

                //find next img tag start
                iImgTagStart = htmlSource.IndexOf(sImgTagStart, (iImgTagStart + 1));
            }

            //Array indexArray = new ;
            int[] indexArray = new int[hashInsertions.Count];
            hashInsertions.Keys.CopyTo(indexArray, 0);
            Array.Sort(indexArray);
            Array.Reverse(indexArray); // get descending list so we start insertions at the end so it doesn't mess up future insertion indexes
            foreach (int index in indexArray)
            {
                returnValue = returnValue.Insert(index, hashInsertions[index].ToString());
            }
            return returnValue;

            #region example
            //string sMenuItemSelectedValue = "<li class=\"selected\">";
            //int iIdxSelectedLiStart = htmlSource.IndexOf(sMenuItemSelectedValue);
            //int iIdxSelectedLiEnd = 0;
            //if (iIdxSelectedLiStart > 0)
            //{
            //    string sLITagClose = "</li>";
            //    iIdxSelectedLiEnd = htmlSource.IndexOf(sLITagClose, iIdxSelectedLiStart) + sLITagClose.Length;
            //    if (iIdxSelectedLiEnd > iIdxSelectedLiStart)
            //    {
            //        string sMainMenuCssClass = "primary";
            //        string sMainMenuDivTagOpen = "<div class=\"" + sMainMenuCssClass + "\">";
            //        string sDivTagClose = "</div>";
            //        string sULTagOpen = "<ul>";
            //        string sULTagClose = "</ul>";

            //        string sbMainMenu_Start = "", sbMainMenu_End = "";
            //        sbMainMenu_Start = htmlSource.Substring(0, iIdxSelectedLiEnd);
            //        sbMainMenu_End = htmlSource.Substring(iIdxSelectedLiEnd, htmlSource.Length - (iIdxSelectedLiEnd));

            //        sbMainMenu_Start += sULTagClose;
            //        sbMainMenu_Start += sDivTagClose;
            //        sbMainMenu_End = sULTagOpen + sbMainMenu_End;
            //        sbMainMenu_End = sMainMenuDivTagOpen + sbMainMenu_End;

            //        PlaceHoldMainNav.Controls.Add(new LiteralControl(sbMainMenu_Start));
            //        string url = "";
            //    }
            //} 
            #endregion
        }

        public string ReplaceMarkupTokens()
        {
            // If a Flash file was uploaded as an image, then we need to hijack the HTML of the Content
            // to embed the flash movie
            int imageIndex;
            string imageTag = this.Content.ParseElement("img", "image", out imageIndex);
            if (imageTag.ToLower().Contains(".swf"))
            {
                string moviePath = this.Content.ParseAttribute(imageTag, "src");
                string movieHeight = this.Content.ParseAttribute(imageTag, "height");
                string movieWidth = this.Content.ParseAttribute(imageTag, "width");

                ObjectContainer flashContainer = new ObjectContainer
                {
                    ObjectUrl = moviePath,
                    FlashExportMode = ObjectContainer.FlashExportModeEnum.FlashSwfObject,
                    Height = Unit.Parse(movieHeight),
                    Width = Unit.Parse(movieWidth)
                };

                this.Content.Html = this.Content.Html.Replace(imageTag, flashContainer.RenderHtml());
            }

            string editLink = String.Empty;
            string contentToOutput = Content.Html.Replace("<!--imagepath-->", ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.ImagesWebPath));
            // It seems there are links that are getting // on them... the following line is to prevent it from causing a link to bomb...
            // I already changed the BaseNavigationPath in BaseMasterPage to prevent it from including a trailing slash
            contentToOutput = contentToOutput.Replace("<!--basepath-->//", "<!--basepath-->/");
            contentToOutput = contentToOutput.Replace("<!--basepath-->", Content.BasePath);
            if (UseSurroundImagesWithLinks)
                contentToOutput = SurroundImagesWithLinks(contentToOutput);

            if (contentToOutput.Contains("<!--subdomain-->"))
            {
                // strip the subdomain from the Content.BasePath
                int begin = Content.BasePath.IndexOf(".");
                if (begin != -1)
                {
                    string subdomain = Content.BasePath.Substring(7, begin - 7);
                    contentToOutput = contentToOutput.Replace("<!--subdomain-->", subdomain);
                }
            }

            if (!String.IsNullOrEmpty(ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.UploadsWebPath))) // && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["UploadsPrefix"])
            {
                string uploads = "/uploads/";//String.Format("{0}/uploads/", ConfigurationManager.AppSettings["UploadsPrefix"]);
                contentToOutput = contentToOutput.Replace(uploads, ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.UploadsWebPath));
            }

            string archiveLink = String.Empty, draftLink = String.Empty;
            if (PageMode == NetSteps.Common.Constants.ViewingMode.Edit)
            {
                editLink = String.Format("<a class='change' href='{0}'>" + (UseNewEditWrapper ? "Edit" : "") + "</a>",
                   String.Format("{0}/ContentAdmin/EditContent.aspx?SectionId={1}&fs=1", ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.AdminUrl), SectionId));

                archiveLink = String.Format("<a class='archives' href='{0}'>Archives</a>",
                   String.Format("{0}/ContentAdmin/Versions.aspx?SectionId={1}&type=archive", ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.AdminUrl), SectionId));

                draftLink = String.Format("<a class='drafts' href='{0}'>Drafts</a>",
                   String.Format("{0}/ContentAdmin/Versions.aspx?SectionId={1}&type=draft", ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.AdminUrl), SectionId));
            }
            else if (PageMode == NetSteps.Common.Constants.ViewingMode.ConsultantEdit && EditType != Constants.HtmlSectionEditType.CorporateOnly)
            {
                string lnkText = (UseNewEditWrapper ? "Edit" : "");
                editLink = String.Format("<a class='change' href='{0}'>" + lnkText + "</a>",
                    String.Format("{0}/ContentAdmin/EditContent.aspx?SectionId={1}", GetBackOfficeUrl(), SectionId));
            }

            int beginIndex = contentToOutput.LastIndexOf("</div>");
            if (beginIndex != -1)
            {
                contentToOutput = contentToOutput.Insert(beginIndex, "<span class=\"ListContentClear\"></span>");
            }

            if (editLink != String.Empty)
            {
                if (UseNewEditWrapper)
                {
                    if (contentToOutput.IndexOf("<div class=\"") == 0)
                    {
                        int endIndex = this.Content.Html.IndexOf("\"", 12);
                        contentToOutput = contentToOutput.Insert(endIndex, " EditableContent");
                    }
                    else
                    {
                        contentToOutput = contentToOutput.Insert(4, " class=\"EditableContent\"");
                    }

                    int insertIndex = contentToOutput.IndexOf(">") + 1;
                    if (insertIndex != 0)
                    {
                        string menu =
                            "<div class=\"EditMenu\">" +
                                "<ul>" +
                                    "<li><a href=\"#\" onclick=\"return false;\">Manage</a>" +
                                        "<ul>" +
                                            "<li>" + editLink + "</li>" +
                                            (!string.IsNullOrEmpty(archiveLink) ? "<li>" + archiveLink + "</li>" : "") +
                                            (!string.IsNullOrEmpty(draftLink) ? "<li>" + draftLink + "</li>" : "") +
                                        "</ul>" +
                                    "</li>" +
                                "</ul>" +
                            "</div>";
                        contentToOutput = contentToOutput.Insert(insertIndex, menu);
                    }
                }
                else
                {
                    int insertIndex = contentToOutput.IndexOf(">") + 1;
                    if (insertIndex != 0)
                    {
                        contentToOutput = contentToOutput.Insert(insertIndex, editLink);
                    }
                }
            }

            return contentToOutput;
        }
    }
}
