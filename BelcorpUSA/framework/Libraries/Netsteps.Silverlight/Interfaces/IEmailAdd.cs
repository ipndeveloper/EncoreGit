using System.Collections.ObjectModel;
using System.Windows;

namespace NetSteps.Silverlight
{
    public interface IEmailAdd : IPopupChild
    {
        string TitleText { get; set; }

        string ToFieldText { get; set; }
        Visibility ToFieldVisibility { get; set; }
        bool ToFieldEnabled { get; set; }

        string CcFieldText { get; set; }
        Visibility CcFieldVisibility { get; set; }
        bool CcFieldEnabled { get; set; }

        string BccFieldText { get; set; }
        Visibility BccFieldVisibility { get; set; }
        bool BccFieldEnabled { get; set; }

        ObservableCollection<string> FilterResult { get; set; }
    }
}
