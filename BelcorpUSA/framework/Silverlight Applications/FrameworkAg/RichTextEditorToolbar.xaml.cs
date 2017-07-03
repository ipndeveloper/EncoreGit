using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace FrameworkAg
{
    public partial class RichTextEditorToolbar : UserControl, INotifyPropertyChanged
    {
        #region Members

        public static readonly DependencyProperty RichTextBoxEditorProperty = DependencyProperty.Register("RichTextBoxEditor", typeof(RichTextBox), typeof(RichTextEditorToolbar), null);
        public RichTextBox RichTextBoxEditor
        {
            get
            {
                return (RichTextBox)GetValue(RichTextBoxEditorProperty);
            }
            set
            {
                if (RichTextBoxEditor != value)
                {
                    SetValue(RichTextBoxEditorProperty, value);
                    NotifyPropertyChanged("RichTextBoxEditor");
                }
            }
        }
        #endregion

        #region Properties
        #endregion

        #region Initialize
        public RichTextEditorToolbar()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(UserControl_Loaded);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
        #endregion

        #region Events
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
        #endregion

        #region Methods
        private void ChangeColor(string color)
        {
            if (RichTextBoxEditor != null && RichTextBoxEditor.Selection.Text.Length > 0)
            {
                //color = (cmbFontColors.SelectedItem as ComboBoxItem).Tag.ToString();

                SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(
                    byte.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)));

                RichTextBoxEditor.Selection.ApplyPropertyValue(Run.ForegroundProperty, brush);
            }
        }

        private void ChangeFont(string fontFamily)
        {
            if (RichTextBoxEditor != null && RichTextBoxEditor.Selection.Text.Length > 0)
            {
                //fontFamily = (cmbFonts.SelectedItem as ComboBoxItem).Tag.ToString();
                RichTextBoxEditor.Selection.ApplyPropertyValue(Run.FontFamilyProperty, new FontFamily(fontFamily));
            }
            ReturnFocus();
        }

        private void ChangeFontSize(double fontSize)
        {
            if (RichTextBoxEditor != null && RichTextBoxEditor.Selection.Text.Length > 0)
            {
                //fontSize = double.Parse((cmbFontSizes.SelectedItem as ComboBoxItem).Tag.ToString());
                RichTextBoxEditor.Selection.ApplyPropertyValue(Run.FontSizeProperty, fontSize);
            }
            ReturnFocus();
        }

        //private Image getImage()
        //{
        //    return CreateImageFromUri(new Uri("desert.jpg", UriKind.RelativeOrAbsolute), 200, 150);
        //}

        private void ReturnFocus()
        {
            if (RichTextBoxEditor != null)
                RichTextBoxEditor.Focus();
        }
        #endregion

        #region Event Handlers
        private void makeBold_Click(object sender, RoutedEventArgs e)
        {
            if (RichTextBoxEditor.Selection.GetPropertyValue(Run.FontWeightProperty) is FontWeight && ((FontWeight)RichTextBoxEditor.Selection.GetPropertyValue(Run.FontWeightProperty)) == FontWeights.Normal)
                RichTextBoxEditor.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Bold);
            else
                RichTextBoxEditor.Selection.ApplyPropertyValue(Run.FontWeightProperty, FontWeights.Normal);
        }
        #endregion

        private void makeItalic_Click(object sender, RoutedEventArgs e)
        {
            if (RichTextBoxEditor != null && RichTextBoxEditor.Selection.Text.Length > 0)
            {
                if (RichTextBoxEditor.Selection.GetPropertyValue(Run.FontStyleProperty) is FontStyle &&
                    ((FontStyle)RichTextBoxEditor.Selection.GetPropertyValue(Run.FontStyleProperty)) == FontStyles.Normal)
                    RichTextBoxEditor.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Italic);
                else
                    RichTextBoxEditor.Selection.ApplyPropertyValue(Run.FontStyleProperty, FontStyles.Normal);
            }
            ReturnFocus();
        }

        private void makeUnderline_Click(object sender, RoutedEventArgs e)
        {
            if (RichTextBoxEditor != null && RichTextBoxEditor.Selection.Text.Length > 0)
            {
                if (RichTextBoxEditor.Selection.GetPropertyValue(Run.TextDecorationsProperty) == null)
                    RichTextBoxEditor.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, TextDecorations.Underline);
                else
                    RichTextBoxEditor.Selection.ApplyPropertyValue(Run.TextDecorationsProperty, null);
            }
            ReturnFocus();
        }

        private void applyCut_Click(object sender, RoutedEventArgs e)
        {

        }

        private void applyCopy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void applyPaste_Click(object sender, RoutedEventArgs e)
        {

        }

        private void undo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void redo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void image_Click(object sender, RoutedEventArgs e)
        {
            //InlineUIContainer container = new InlineUIContainer(); container.Child = getImage();
            //RichTextBoxEditor.Selection.Insert(container);
            //ReturnFocus();
        }

        private void hyperlink_Click(object sender, RoutedEventArgs e)
        {
            //Hyperlink hyperlink = new Hyperlink();
            //hyperlink.TargetName = "_blank";
            //hyperlink.NavigateUri = new Uri(cw.txtURL.Text);

            //if (cw.txtURLDesc.Text.Length > 0)
            //    hyperlink.Inlines.Add(cw.txtURLDesc.Text);
            //else
            //    hyperlink.Inlines.Add(cw.txtURL.Text);

            //RichTextBoxEditor.Selection.Insert(hyperlink);
        }
    }
}
