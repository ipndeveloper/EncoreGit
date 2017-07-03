using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities
{
    public class CeldaPDF
    {
        public string Text { get; set; }
        public string FontName { get; set; }
        public float FontSize { get; set; }
        public bool esBold { get; set; }
        public bool UseDescender { get; set; }
        public int HorizontalAlignment { get; set; }
        public int VerticalAlignment { get; set; }
        public int Colspan { get; set; }
        public float PaddingBottom { get; set; }
        public int BackgroundColor { get; set; }
        public float Width { get; set; }
        public float FixedHeight { get; set; }
        public float PaddingTop { get; set; }
        public float BorderWidthBottom { get; set; }
        public float fixedLeading { get; set; }
        public float multipliedLeading { get; set; }
    }
}
