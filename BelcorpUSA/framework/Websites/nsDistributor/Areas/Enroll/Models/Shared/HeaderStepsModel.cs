using System.Collections.Generic;

namespace nsDistributor.Areas.Enroll.Models.Shared
{
    public class HeaderStepsModel
    {
        public IEnumerable<HeaderStepItemModel> HeaderStepItems { get; set; }

        public class HeaderStepItemModel
        {
            public string Text { get; set; }
            public string Url { get; set; }
            public bool IsCurrentStep { get; set; }
            public bool EnableHyperlink { get; set; }
        }
    }
}