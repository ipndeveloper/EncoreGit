using System.Collections.Generic;

namespace NetSteps.Silverlight
{
    public interface IOverviewModule : ITitle, IErrorMessage, IStateChanged
    {
        bool CollapseButton { get; set; }
        bool FullScreenButton { get; set; }
        bool CloseButton { get; set; }
        string OverviewPosition { get; set; }
        bool ShowHeader { get; set; }
        bool ShowSeparator { get; set; }
        string Key { get; set; }
        string FullScreenStateName { get; set; }
        Dictionary<string, string> Parameters { get; }
        string Function { get; set; }
    }
}
