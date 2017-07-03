using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NetSteps.Silverlight.Base;

namespace NetSteps.Silverlight
{
    #region Enums

    //public enum Language
    //{
    //    NotSet,
    //    English,
    //    Japanese,
    //    Spanish
    //}

    #endregion

    public class Translation
    {
        #region Members
        protected static object _lock = new object();

        public static bool InsertUnfoundTerms = false;

        private static List<int> _AllSiteLanguages;
        public static List<int> AllSiteLanguages
        {
            get { return _AllSiteLanguages; }
            set { _AllSiteLanguages = value; }
        }

        public static TermDictionary _TermDictionary;
        public static TermDictionary TermDictionary
        {
            get { return _TermDictionary; }
            set
            {
                _TermDictionary = value;
            }
        }
        private static Dictionary<string, string> UnfoundTerms = new Dictionary<string, string>();
        public static Dictionary<string, string> NewTermsToAdd
        {
            get
            {
                //List<string> newTerms = new List<string>();
                //foreach (string item in UnfoundTerms)
                //{
                //    newTerms.Add(item);
                //}
                //return newTerms;
                return UnfoundTerms;
            }
        }

        private const string DefaultLanguageId = "1";

        //// TODO: This value will need to be stored in a "session" type variable not a static one so that each user get the App in their own language - JHE
        private static string _languageId = DefaultLanguageId;
        public static string LanguageId
        {
            get
            {
                return ApplicationContext.LanguageId.ToString();
            }
            set
            {
                lock (_lock)
                {
                    ApplicationContext.LanguageId = Convert.ToInt32(value);
                }
            }
        }

        private static WeakReferenceListBase<UIElement> _applicationRootControls = new WeakReferenceListBase<UIElement>();

        #endregion

        #region Properties
        #endregion

        #region Methods

        public static void ReTranslateAllRootControls()
        {
            _applicationRootControls.CleanOutDeadReferences();
            if (_applicationRootControls.Count > 0)
            {
                AppFactory.Dispatcher.BeginInvoke(() =>
                {
                    foreach (var element in _applicationRootControls)
                        TranslateAllControls(element.RootObject);
                });
            }
        }

        /// <summary>
        ///  This should be called by the root control only to add the control to list of maintained controls
        ///     to be re-translated later if desired. - JHE
        /// </summary>
        /// <param name="element"></param>
        public static void TranslateRootControl(UIElement element)
        {
            if (!_applicationRootControls.Contains(element))
                _applicationRootControls.Add(element);

            if (TermDictionary != null)
            {
                AppFactory.Dispatcher.BeginInvoke(() =>
                {
                    TranslateAllControls(element);
                });
            }
        }

        private static void TranslateAllControls(UIElement element)
        {
            //if (element is UserControl)
            //{
            //    // TODO : Finish this by recursivly traslating all children controls - JHE
            //    foreach (UIElement childElement in (element as UserControl).)
            //    {
            //        if (childElement is UserControl ||
            //            childElement is Grid ||
            //            childElement is Border ||
            //            childElement is StackPanel ||
            //            childElement is Button)
            //        {
            //            TranslateAllControls(childElement);
            //        }
            //    }
            //}
            //else 

            //if (element is Telerik.Windows.Controls.RadGridView)
            //{
            //    object temp = element;
            //    // This only works with non auto generated radgridview columns.
            //    if ((temp as Telerik.Windows.Controls.RadGridView).Columns.Count > 0)
            //    {
            //        foreach (Telerik.Windows.Controls.GridViewColumn childElement in (temp as Telerik.Windows.Controls.RadGridView).Columns)
            //        {
            //            if (!string.IsNullOrEmpty(childElement.UniqueName))
            //            {
            //                if (!string.IsNullOrEmpty(childElement.Header.ToString()))
            //                {
            //                    childElement.Header = FindTerm(childElement.UniqueName, childElement.Header.ToString());
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            if (element is Grid)
            {
                foreach (UIElement childElement in (element as Grid).Children)
                {
                    if (ContainsKnownChildren(childElement))
                        TranslateAllControls(childElement);
                    TranslateControl(childElement);
                }
            }
            else if (element is TabControl)
            {
                foreach (UIElement childElement in (element as TabControl).Items)
                {
                    if (ContainsKnownChildren(childElement))
                        TranslateAllControls(childElement);
                }
            }
            else if (element is TabItem)
            {
                if (((TabItem)element).Content is UIElement && ContainsKnownChildren((UIElement)((TabItem)element).Content))
                    TranslateAllControls((UIElement)((TabItem)element).Content);

                if (((TabItem)element).HeaderTemplate != null)
                {
                    for (int index = 0; index < VisualTreeHelper.GetChildrenCount(element); index++)
                    {
                        DependencyObject obj = VisualTreeHelper.GetChild(element, index);
                        if (obj is UIElement)
                            TranslateAllControls(obj as UIElement);
                    }
                    //TranslateAllControls((UIElement)(((TabItem)element).HeaderTemplate.LoadContent()));
                    TranslateControl(element);
                }

                TranslateControl(element);
            }
            else if (element is Button)
            {
                if (((Button)element).Content is UIElement && ContainsKnownChildren((UIElement)((Button)element).Content))
                    TranslateAllControls((UIElement)((Button)element).Content);
                else if (((Button)element).Content is UIElement)
                    TranslateControl((UIElement)((Button)element).Content);

                TranslateControl(element);
            }
            else if (element is StackPanel)
            {
                foreach (UIElement childElement in (element as StackPanel).Children)
                {
                    if (ContainsKnownChildren(childElement))
                        TranslateAllControls(childElement);

                    TranslateControl(childElement);
                }
            }
            else if (element is Border)
            {
                if (ContainsKnownChildren(((Border)element).Child))
                    TranslateAllControls(((Border)element).Child);

                TranslateControl(((Border)element).Child);
            }
            else if (element is Panel)
            {
                foreach (UIElement childElement in (element as Panel).Children)
                {
                    if (ContainsKnownChildren(childElement))
                        TranslateAllControls(childElement);

                    TranslateControl(childElement);
                }
            }
            else if (element is ScrollViewer)
            {
                if (((ScrollViewer)element).Content is UIElement && ContainsKnownChildren((UIElement)((ScrollViewer)element).Content))
                    TranslateAllControls((UIElement)((ScrollViewer)element).Content);
            }
            //else if (element is AgDataGrid)
            //{
            //    foreach (AgDataGridColumn childElement in (element as AgDataGrid).Columns)
            //    {
            //        if (childElement.HeaderContent != null && !string.IsNullOrEmpty(childElement.FieldName))
            //        {
            //            if (!string.IsNullOrEmpty(childElement.HeaderContent.ToString()))
            //            {
            //                childElement.HeaderContent = FindTerm(childElement.FieldName, childElement.HeaderContent.ToString());
            //            }
            //        }
            //    }
            //}
            else if (element is ItemsControl || element is ItemsPresenter || element is ContentPresenter || element is ScrollContentPresenter || element is ContentControl)
            {
                for (int index = 0; index < VisualTreeHelper.GetChildrenCount(element); index++)
                {
                    DependencyObject obj = VisualTreeHelper.GetChild(element, index);
                    if (obj is UIElement)
                        TranslateAllControls(obj as UIElement);
                }
            }
            else
                TranslateControl(element);
        }

        private static bool ContainsKnownChildren(UIElement element)
        {
            return element is UserControl
                || element is Grid
                || element is Border
                || element is StackPanel
                || element is Button
                || element is ItemsControl
                || element is ItemsPresenter
                || element is ContentPresenter
                || element is ScrollViewer
                || element is ScrollContentPresenter
                || element is Panel
                || element is ContentControl
                //|| element is AgDataGrid
                //|| element is Telerik.Windows.Controls.RadGridView
                ;
        }

        private static void TranslateControl(UIElement element)
        {
            if (element is FrameworkElement)
            {
                string termName = string.Empty;
                if ((element as FrameworkElement).Tag != null)
                    termName = (element as FrameworkElement).Tag.ToString();

                if (!string.IsNullOrEmpty(termName))
                {
                    string translationTerm = string.Empty;
                    if (element is TextBlock)
                    {
                        translationTerm = FindTerm((element as TextBlock).Tag.ToString(), (element as TextBlock).Text);
                        if (!termName.Equals(translationTerm))
                            (element as TextBlock).Text = translationTerm;
                    }
                    else if (element is RadioButton)
                    {
                        translationTerm = FindTerm((element as RadioButton).Tag.ToString(), (element as RadioButton).Content.ToString());
                        if (!termName.Equals(translationTerm))
                            (element as RadioButton).Content = translationTerm;
                    }
                    else if (element is CheckBox)
                    {
                        translationTerm = FindTerm((element as CheckBox).Tag.ToString(), (element as CheckBox).Content.ToString());
                        if (!termName.Equals(translationTerm))
                            (element as CheckBox).Content = translationTerm;
                    }
                    else if (element is Button)
                    {
                        // 1) If Button does not have child control, localize Content
                        // 2) If Button has TooltipService.Tooltip text, localize that with the Tag
                        // 3) If Button has children... just loop through and localize... 
                        if (!(((Button)element).Content is UIElement) && ((Button)element).Content != null)
                        {
                            translationTerm = FindTerm((element as Button).Tag.ToString(), (element as Button).Content.ToString());

                            if (!termName.Equals(translationTerm))
                            {
                                (element as Button).Content = translationTerm;
                                ToolTipService.SetToolTip((element as Button), translationTerm);
                            }
                        }
                        else
                        {
                            if (ToolTipService.GetToolTip(element as Button) != null && !string.IsNullOrEmpty(ToolTipService.GetToolTip((element as Button)).ToString()))
                            {
                                translationTerm = FindTerm((element as Button).Tag.ToString(), ToolTipService.GetToolTip((element as Button)).ToString());

                                if (!termName.Equals(translationTerm))
                                {
                                    ToolTipService.SetToolTip((element as Button), translationTerm);
                                }
                            }
                        }
                    }
                    else if (element is TabItem)
                    {
                        translationTerm = FindTerm((element as TabItem).Tag.ToString(), (element as TabItem).Header.ToString());
                        if (!termName.Equals(translationTerm))
                            (element as TabItem).Header = translationTerm;
                    }
                    else
                    {
                        string s = string.Empty;
                    }
                }
                else
                {
                    // These are elements with no Tags not all controls should be localized...

                    // Add to list of UnfoundTerms - JHE
                    // TODO: Collect these and transmit them to the server for addition/notification of their need to be added - JHE
                    string untranslatedTerm = string.Empty;
                    if (element is TextBlock)
                        untranslatedTerm = (element as TextBlock).Text.Trim();
                    else if (element is RadioButton)
                        untranslatedTerm = (element as RadioButton).GetText();
                    else if (element is CheckBox)
                        untranslatedTerm = (element as CheckBox).GetText();
                    else if (element is Button)
                        untranslatedTerm = (element as Button).GetText();

                    //if (!string.IsNullOrEmpty(untranslatedTerm) && !UnfoundTerms.ContainsIgnoreCase(untranslatedTerm))
                    //    UnfoundTerms.Add(untranslatedTerm);
                }
            }
        }

        private static string FindTerm(string term, string text)
        {
            if (term != null)
                term = term.Trim();

            if (TermDictionary != null)
            {
                foreach (KeyValuePair<string, Dictionary<string, string>> kvp in TermDictionary)
                {
                    bool containsKey = kvp.Value.ContainsKey(LanguageId);
                    bool notNull = false;
                    if (containsKey && kvp.Value[LanguageId] != null)
                        notNull = true;

                    bool keyEqualsTerm = false;
                    if (kvp.Key.Equals(term))
                        keyEqualsTerm = true;

                    if (containsKey && notNull && keyEqualsTerm)
                        return FindTerm2(LanguageId, kvp.Key);
                }
                if (!UnfoundTerms.ContainsKey(term))
                    UnfoundTerms.Add(term, text);
            }

            return text;
        }

        public static string GetTerm(string termName, string defaultValue)
        {
            if (!string.IsNullOrEmpty(termName))
            {
                string Term = FindTerm(termName, defaultValue);
                if (!string.IsNullOrEmpty(Term) && Term != termName.Trim())
                {
                    Term = Term.Replace("<LineBreak/>", Environment.NewLine);
                    return Term;
                }
            }

            return defaultValue;
        }

        private static string FindTerm2(string language, string termName)
        {
            try
            {
                return TermDictionary[termName].ContainsKey(language) ?
                TermDictionary[termName][language] :
                TermDictionary[termName][DefaultLanguageId];
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion
    }

    #region TermDictionary
    /// <summary>
    /// This is a help class extending CacheListBase<> and adding it's own 
    /// InitializeList() method to load its own cache. - JHE
    /// </summary>
    public class TermDictionary : ThreadSafeDictionary<string, Dictionary<string, string>>
    {
        //protected override void InitializeList()
        //{
        //    List = new Dictionary<int, Dictionary<Language, string>>();
        //    try
        //    {
        //        List = GetTerms();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error loading terms into TermDictionary: " + ex.Message);
        //    }
        //}

        // TODO: update this function to get terms from DB later;
        //  This is just some quick data for testing translation on Silverlight controls - JHE
        //private Dictionary<int, Dictionary<Language, string>> GetTerms()
        //{
        //    Dictionary<int, Dictionary<Language, string>> list = new Dictionary<int, Dictionary<Language, string>>();

        //    List<string> englishTerms = "First Name, Last Name, Language, Country, Address, Company, City, Search".ToStringList();
        //    List<string> japaneseTerms = "名前, 姓, Language, 国, 住所, 会社, 市町村, 検索".ToStringList();
        //    List<string> spanishTerms = "Nombre , Apellido , Language, País , Domicilio , Compañía , Ciudad , Buscar ".ToStringList();

        //    for (int i = 0; i < englishTerms.Count; i++)
        //    {
        //        Dictionary<Language, string> languageTerms = new Dictionary<Language, string>();
        //        languageTerms.Add(Language.English, englishTerms[i].Trim());
        //        languageTerms.Add(Language.Japanese, japaneseTerms[i].Trim());
        //        languageTerms.Add(Language.Spanish, spanishTerms[i].Trim());
        //        list.Add(i + 1, languageTerms);
        //    }

        //    return list;
        //}
    }
    #endregion
}
