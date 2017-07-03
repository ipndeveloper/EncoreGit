using System.Drawing;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Image Extensions
    /// Created: 11-01-2008
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// http://forums.asp.net/t/1145909.aspx - JHE
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static System.Drawing.Image TrimWhiteSpace(this System.Drawing.Image image)
        {
            return TrimWhiteSpace(image, 0);
        }
        public static System.Drawing.Image TrimWhiteSpace(this System.Drawing.Image image, int padding)
        {
            // Adjust this number to determine how close to white will be counted as white.
            // Use threshold to remove pure white only
            int threshold = 254;   //254

            Offset offset = new Offset();
            Bitmap bitmap = (Bitmap)image.Clone();

            offset = GetOffsets(image, threshold);
            if (offset.AreAll1())
                offset = GetOffsets(image, threshold - 1);
            if (offset.AreAll1())
                offset = GetOffsets(image, threshold - 2);
            if (offset.AreAll1())
                offset = GetOffsets(image, threshold - 3);
            if (offset.AreAll1())
                offset = GetOffsets(image, threshold - 4);

            offset.PadOffsets(padding);

            // Create a new image set to the size of the original minus the white space
            Bitmap newImage = new Bitmap(bitmap.Width - offset.Left - offset.Right, bitmap.Height - offset.Top - offset.Bottom);

            // Get a graphics object for the new bitmap, and draw the original bitmap onto it, offsetting it do remove the whitespace
            Graphics g = Graphics.FromImage(newImage);
            g.DrawImage(bitmap, 0 - offset.Left, 0 - offset.Top);

            return (System.Drawing.Image)newImage.Clone();
        }

        private static Offset GetOffsets(System.Drawing.Image image, int threshhold)
        {
            Offset offset = new Offset();
            Bitmap bitmap = (Bitmap)image.Clone();

            bool foundColor = false;
            // Get left bounds to crop
            for (int x = 0; x < bitmap.Width && foundColor == false; x++)
            {
                for (int y = 0; y < bitmap.Height && foundColor == false; y++)
                {
                    Color color = bitmap.GetPixel(x, y);
                    if (((color.R < threshhold || color.G < threshhold || color.B < threshhold) && !color.IsEmpty()) || (!color.IsEmpty()))
                        foundColor = true;
                }
                offset.Left += 1;
            }

            foundColor = false;
            // Get top bounds to crop
            for (int y = 0; y < bitmap.Height && foundColor == false; y++)
            {
                for (int x = 0; x < bitmap.Width && foundColor == false; x++)
                {
                    Color color = bitmap.GetPixel(x, y);
                    if (((color.R < threshhold || color.G < threshhold || color.B < threshhold) && !color.IsEmpty()) || (!color.IsEmpty()))
                        foundColor = true;
                }
                offset.Top += 1;
            }

            foundColor = false;
            // Get right bounds to crop
            for (int x = bitmap.Width - 1; x >= 0 && foundColor == false; x--)
            {
                for (int y = 0; y < bitmap.Height && foundColor == false; y++)
                {
                    Color color = bitmap.GetPixel(x, y);
                    if (((color.R < threshhold || color.G < threshhold || color.B < threshhold) && !color.IsEmpty()) || (!color.IsEmpty()))
                        foundColor = true;
                }
                offset.Right += 1;
            }

            foundColor = false;
            // Get bottom bounds to crop
            for (int y = bitmap.Height - 1; y >= 0 && foundColor == false; y--)
            {
                for (int x = bitmap.Width - 1; x >= 0 && foundColor == false; x--)
                {
                    Color color = bitmap.GetPixel(x, y);
                    if (((color.R < threshhold || color.G < threshhold || color.B < threshhold) && !color.IsEmpty()) || (!color.IsEmpty()))
                        foundColor = true;
                }
                offset.Bottom += 1;
            }

            return offset;
        }

        public class Offset
        {
            public int Top = 0;
            public int Bottom = 0;
            public int Left = 0;
            public int Right = 0;

            public bool AreAll1()
            {
                return (Top == 1 && Bottom == 1 && Right == 1 && Left == 1);
            }

            public void PadOffsets(int padding)
            {
                int tempTop = Top;
                int tempBottom = Bottom;
                int tempLeft = Left;
                int tempRight = Right;
                try
                {
                    if ((Top - padding) >= 0)
                        Top = Top - padding;
                    if ((Bottom - padding) >= 0)
                        Bottom = Bottom - padding;
                    if ((Left - padding) >= 0)
                        Left = Left - padding;
                    if ((Right - padding) >= 0)
                        Right = Right - padding;
                }
                catch
                {
                    Top = tempTop;
                    Bottom = tempBottom;
                    Bottom = tempLeft;
                    Right = tempRight;
                }
            }
        }

        public static System.Drawing.Image RoundCorners(this System.Drawing.Image image, int roundedDiameter)
        {
            if (roundedDiameter > 0)
            {
                Bitmap bitmap = new Bitmap(image.Width, image.Height);
                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(Color.Transparent);
                g.SmoothingMode = (System.Drawing.Drawing2D.SmoothingMode.AntiAlias);
                Brush brush = new System.Drawing.TextureBrush(image);
                FillRoundedRectangle(g, new Rectangle(0, 0, image.Width, image.Height), roundedDiameter, brush);// done with drawing dispose graphics object.
                g.Dispose();

                return (System.Drawing.Image)bitmap;
            }
            else
                return image;
        }
        private static void FillRoundedRectangle(Graphics g, Rectangle r, int d, Brush b)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.X + r.Width - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Y + r.Height - d, d, d, 90, 90);
            g.FillPath(b, gp);
        }
    }
}
