using System.Windows;

namespace NetSteps.Silverlight
{
    public class PopupManager
    {
        public SetPopupMethod SetInlinePopup;

        public virtual void OnSetInlinePopup(InlinePopupArgs e)
        {
            if (SetInlinePopup != null)
                SetInlinePopup(e);
        }

        public void ShowInlinePopup(string UniqueId, FrameworkElement child)
        {
            OnSetInlinePopup(new InlinePopupArgs(UniqueId, true, child));
        }

        public void HideInlinePopup(string UniqueId, FrameworkElement child)
        {
            OnSetInlinePopup(new InlinePopupArgs(UniqueId, false, child));
        }
    }

    public delegate void SetPopupMethod(InlinePopupArgs args);

    public class InlinePopupArgs
    {
        /// <summary>
        /// UniqueId  for the popup that is unique accross the application. - JHE
        /// </summary>
        public string UniqueId { get; protected set; }
        public bool IsOpen { get; protected set; }
        public FrameworkElement Child { get; protected set; }

        public InlinePopupArgs(string uniqueId, bool isOpen, FrameworkElement child)
        {
            UniqueId = uniqueId;
            IsOpen = isOpen;
            Child = child;
        }
    }
}
