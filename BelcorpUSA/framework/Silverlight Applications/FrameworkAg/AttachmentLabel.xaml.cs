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

namespace FrameworkAg
{
    public partial class AttachmentLabel : UserControl
    {
        #region Members
        #endregion

        #region Properties
        #endregion

        #region Initialize
        public AttachmentLabel()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(UserControl_Loaded);
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
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Panel)
                (this.Parent as Panel).Children.Remove(this);
        }
        #endregion
    }
}
