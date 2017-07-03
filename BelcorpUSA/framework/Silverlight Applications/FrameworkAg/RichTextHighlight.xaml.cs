using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FrameworkAg
{
    public partial class RichTextHighlight : UserControl
    {
        #region Members
        #endregion

        #region Properties
        #endregion

        #region Initialize
        public RichTextHighlight()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(UserControl_Loaded);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //uxHighlightText.tex

            //uxText.Blocks..Text = "Aardvarks are strange animals";
            //HighlightPhrase(richTextBox1, "a", Color.Red);
        }
        #endregion

        private void uxHighlightText_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Regex rex = new Regex("This");

            //int selectionstart = uxText.SelectionStart;

            //foreach (Match m in rex.Matches(uxText.Text))
            //{
            //    richTextBox1.Select(m.Index, m.Value.Length);
            //    richTextBox1.SelectionColor = Color.Blue;
            //    richTextBox1.SelectionStart = selectionstart;
            //    richTextBox1.SelectionColor = Color.Black;
            //}

            HighlightText2(uxText, uxHighlightText.Text);
        }

        private void HighlightText2(RichTextBox richTextBox, string text)
        {
            TextPointer s = new TextPointer();
            s.

            foreach (Rect r in m_selectionRect)
            {
                if (r.Contains(e.GetPosition(highlightCanvas)))
                {
                    if (highlightRect == null)
                    {
                        highlightRect = CreateHighlightRectangle(r);
                    }
                    else
                    {
                        highlightRect.Visibility = System.Windows.Visibility.Visible;
                        highlightRect.Width = r.Width;
                        highlightRect.Height = r.Height;
                        Canvas.SetLeft(highlightRect, r.Left);
                        Canvas.SetTop(highlightRect, r.Top);
                    }
                }
            }

        }

        private Rectangle CreateHighlightRectangle(Rect bounds)
        {
            Rectangle r = new Rectangle();
            r.Fill = new SolidColorBrush(Color.FromArgb(75, 0, 0, 200));
            r.StrokeThickness = 0;
            r.Width = bounds.Width;
            r.Height = bounds.Height;
            Canvas.SetLeft(r, bounds.Left);
            Canvas.SetTop(r, bounds.Top);

            highlightCanvas.Children.Add(r);

            return r;
        }

        public Rectangle highlightRect;
        private List<Rect> m_selectionRect = new List<Rect>();
        private void HighlightText(RichTextBox richTextBox, string text)
        {
            TextPointer tp = richTextBox.ContentStart;
            TextPointer nextTp = null;
            Rect nextRect = Rect.Empty;
            Rect tpRect = tp.GetCharacterRect(LogicalDirection.Forward);
            Rect lineRect = Rect.Empty;

            int lineCount = 1;

            while (tp != null)
            {
                nextTp = tp.GetNextInsertionPosition(LogicalDirection.Forward);
                if (nextTp != null && nextTp.IsAtInsertionPosition)
                {
                    nextRect = nextTp.GetCharacterRect(LogicalDirection.Forward);
                    // this occurs for more than one line
                    if (nextRect.Top > tpRect.Top)
                    {
                        if (m_selectionRect.Count < lineCount)
                            m_selectionRect.Add(lineRect);
                        else
                            m_selectionRect[lineCount - 1] = lineRect;

                        lineCount++;

                        if (m_selectionRect.Count < lineCount)
                            m_selectionRect.Add(nextRect);

                        lineRect = nextRect;
                    }
                    else if (nextRect != Rect.Empty)
                    {
                        if (tpRect != Rect.Empty)
                            lineRect.Union(nextRect);
                        else
                            lineRect = nextRect;
                    }
                }
                tp = nextTp;
                tpRect = nextRect;
            }
            if (lineRect != Rect.Empty)
            {
                if (m_selectionRect.Count < lineCount)
                    m_selectionRect.Add(lineRect);
                else
                    m_selectionRect[lineCount - 1] = lineRect;
            }
            while (m_selectionRect.Count > lineCount)
            {
                m_selectionRect.RemoveAt(m_selectionRect.Count - 1);
            }
        }

        #region Events
        #endregion

        #region Methods
        //static void HighlightPhrase(RichTextBox box, string phrase, Color color)
        //{
        //    int pos = box.SelectionStart;
        //    string s = box.Text;
        //    for (int ix = 0; ; )
        //    {
        //        int jx = s.IndexOf(phrase, ix, StringComparison.CurrentCultureIgnoreCase);
        //        if (jx < 0) break;
        //        box.SelectionStart = jx;
        //        box.SelectionLength = phrase.Length;
        //        box.SelectionColor = color;
        //        ix = jx + 1;
        //    }
        //    box.SelectionStart = pos;
        //    box.SelectionLength = 0;
        //}
        #endregion

        #region Event Handlers
        #endregion
    }
}