using System.Windows.Media;

namespace NetSteps.Silverlight
{
    public interface IPopupChild : ICloseWindow, IStateChanged
    {
        string Text { get; set; }
        ImageSource Icon { get; set; }
        string ImageSourceResourceKey { get; set; }
        bool InstanceIsPopup { get; set; }
    }
}
