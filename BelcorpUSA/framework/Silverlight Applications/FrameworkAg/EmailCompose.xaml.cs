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
using System.IO;
using NetSteps.Silverlight.Extensions;

namespace FrameworkAg
{
    public partial class EmailCompose : UserControl
    {
        #region Members
        #endregion

        #region Properties
        #endregion

        #region Initialize
        public EmailCompose()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(UserControl_Loaded);

            uxEditorToolbar.RichTextBoxEditor = uxBody;
            LayoutRoot.AllowDrop = true;
            LayoutRoot.Drop += new DragEventHandler(LayoutRoot_Drop);
        }

        void LayoutRoot_Drop(object sender, DragEventArgs e)
        {
            // http://www.dotnetfunda.com/articles/article803-silverlight-4-how-to-drag-and-drop-external-files-.aspx
            FileInfo[] droppedFiles = e.Data.GetData(DataFormats.FileDrop) as FileInfo[];

            foreach (var file in droppedFiles)
            {
                AttachmentLabel button = new AttachmentLabel();
                button.DataContext = file;
                uxAttachments.Children.Add(button);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
        #endregion

        #region Events
        #endregion

        #region Methods
        #endregion

        #region Event Handlers
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Parent is ChildWindow)
                (this.Parent as ChildWindow).Close();
        }
        #endregion
    }
}
