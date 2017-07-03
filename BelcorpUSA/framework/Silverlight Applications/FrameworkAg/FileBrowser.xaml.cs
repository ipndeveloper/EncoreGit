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

namespace FrameworkAg
{
    public partial class FileBrowser : UserControl
    {
        #region Members
        #endregion

        #region Properties
        #endregion

        #region Initialize
        public FileBrowser()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(UserControl_Loaded);

            //LayoutRoot.Drop += new DragEventHandler(LayoutRoot_Drop);
        }

        void LayoutRoot_Drop(object sender, DragEventArgs e)
        {
            // http://www.dotnetfunda.com/articles/article803-silverlight-4-how-to-drag-and-drop-external-files-.aspx
            //FileInfo[] droppedFiles = e.Data.GetData(DataFormats.FileDrop) as FileInfo[];

            //foreach (var file in droppedFiles)
            //{
            //    AttachmentLabel button = new AttachmentLabel();
            //    button.DataContext = file;
            //    uxAttachments.Children.Add(button);
            //}
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current.HasElevatedPermissions)
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
                foreach (string dir in Directory.EnumerateDirectories(path))
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = dir.Substring(dir.LastIndexOf('\\') + 1);
                    uxTree.Items.Add(item);
                    ProcessFolder(dir, item.Items);
                }
            } 

        }
        #endregion

        #region Events
        #endregion

        #region Methods
        // http://www.wintellect.com/CS/blogs/jprosise/archive/2009/12/16/silverlight-4-s-new-local-file-system-support.aspx
        private void ProcessFolder(string path, ItemCollection items)
        {
            foreach (string dir in Directory.EnumerateDirectories(path))
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = dir.Substring(dir.LastIndexOf('\\') + 1);
                items.Add(item);
                ProcessFolder(dir, item.Items);
            }
        } 
        #endregion

        #region Event Handlers
        #endregion
    }
}
