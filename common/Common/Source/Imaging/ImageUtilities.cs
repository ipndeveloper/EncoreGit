using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;

namespace NetSteps.Common.Imaging
{
    public static class ImageUtilities
    {
        public static System.Drawing.Image CropImage(System.Drawing.Image image, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(image);
            try
            {
                Bitmap bmpCrop = bmpImage.Clone(cropArea,
                bmpImage.PixelFormat);
                return (System.Drawing.Image)(bmpCrop);
            }
            finally
            {
                bmpImage.Dispose();
            }
        }

        public static Image ResizeImage(Image image, Size size)
        {
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;

            float percent = 0;
            float percentW = 0;
            float percentH = 0;

            percentW = ((float)size.Width / (float)sourceWidth);
            percentH = ((float)size.Height / (float)sourceHeight);

            if (percentH < percentW)
                percent = percentW;
            else
                percent = percentH;

            //caused size to be too small in some cases
            //int destWidth = (int)(sourceWidth * percent);
            //int destHeight = (int)(sourceHeight * percent);
            //we want to err on being larger than the passed in size - ACLS
            int destWidth = (int)Math.Round(sourceWidth * percent);
            int destHeight = (int)Math.Round(sourceHeight * percent);

            var b = new Bitmap(destWidth, destHeight);
            

            using (var g = Graphics.FromImage(b))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                g.DrawImage(image, 0, 0, destWidth, destHeight);
            }

            return b;
        }

        public static string CropWebUpload(string selectedCoords, int width, int height, string imagePath)
        {
            imagePath = imagePath.WebUploadPathToAbsoluteUploadPath();

            string[] coords = selectedCoords.Split(',');
            decimal x = coords[0].ToDecimal();
            decimal y = coords[1].ToDecimal();
            decimal w = coords[4].ToDecimal();
            decimal h = coords[5].ToDecimal();

            if (w == 0 || h == 0)
                throw new NetSteps.Common.Exceptions.NetStepsApplicationException("Please select an area to crop")
                {
                    PublicMessage = Translation.GetTerm("PleaseSelectAnAreaToCrop", "Please select an area to crop")
                };

            var image = System.Drawing.Image.FromFile(imagePath);

            // Resize
            decimal rx, ry;
            int originalImageWidth, originalImageHeight;
            Image resizedImage;
            try
            {
                rx = width / (w * (width / (decimal)image.Width));
                ry = height / (h * (height / (decimal)image.Height));
                Size newSize = new Size((int)Math.Round((rx * width), 0), (int)Math.Round((ry * height), 0));
                resizedImage = ResizeImage(image, newSize);
                originalImageWidth = image.Width;
                originalImageHeight = image.Height;
            }
            finally
            {
                image.Dispose();
            }

            // Crop
            Image final;
            try
            {
                int xCrop = (int)Math.Round(rx * ((width / (decimal)originalImageWidth) * x), 0);
                int yCrop = (int)Math.Round(ry * ((height / (decimal)originalImageHeight) * y), 0);
                Rectangle cropArea = new Rectangle(xCrop, yCrop, width, height);
                final = ImageUtilities.CropImage(resizedImage, cropArea);
            }
            catch (OutOfMemoryException ex)
            {
                //ResizeImage was resizing the image smaller than what we told
                //	it to, which caused CropImage to throw an OutOfMemoryException - ACLS
                throw new Exception("Out of memory or crop area is outside image size", ex);
            }
            finally
            {
                resizedImage.Dispose();
            }

            string newImagePath;
            try
            {
                newImagePath = Regex.IsMatch(imagePath, @"_\w{32}") ? Regex.Replace(imagePath, @"_\w{32}", "_" + Guid.NewGuid().ToString("N")) : imagePath.Insert(imagePath.LastIndexOf('.'), "_" + Guid.NewGuid().ToString("N"));
                final.Save(newImagePath);
            }
            finally
            {
                final.Dispose();
            }

            try
            {
                // Clean up the file after we've finished with it to save server space
                System.IO.File.Delete(imagePath);
            }
            catch { }

            return newImagePath;
        }

        public static Image SaveWebOptimizedImage(string sourcePath, string destinationPath,
            string thumbnailPath = null, int width = 4000, int thumbnailWidth = 150)
        
        {
            // 2012-03-20, JWL, Need to replace with configuration item later

            var uploadedFile = new FileInfo(sourcePath);
            var thumbnailSize = new Size {Height = thumbnailWidth, Width = thumbnailWidth};

            try
            {
                var jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                var bitmap = new Bitmap(uploadedFile.FullName);

                var compressLevel = GetCompressionLevel(uploadedFile.Length);

                bitmap = bitmap.Width > width || bitmap.Height > width
                             ? (Bitmap)ResizeImage(bitmap, new Size(width, width))
                             : bitmap;
                bitmap.Save(destinationPath, jpgEncoder, GetJpgCompressionEncoder(compressLevel));

                if (thumbnailPath != null)
                {
                    var thumbnailImage = (Bitmap) ResizeImage(bitmap, thumbnailSize);
                    thumbnailImage.Save(thumbnailPath, jpgEncoder, GetJpgCompressionEncoder(50L));
                }

                return bitmap;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while attempting to save a web-optimized version of the image.", e);
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
        
        private static EncoderParameters GetJpgCompressionEncoder(long compressionlevel)
        {
            var encoderQualityParm = Encoder.Quality;
            var jpgEncodeParms = new EncoderParameters(1);
            var jpgCompressParm = new EncoderParameter(encoderQualityParm, compressionlevel);
            jpgEncodeParms.Param[0] = jpgCompressParm;
            return jpgEncodeParms;

        }
        
        private static long GetCompressionLevel(long length)
        {
            const int lowFileLimit = 3145728; // 3 Megs
            const int medFileLimit = 6291456; // 6 Megs
            const int highFileLimit = 10485760; // 10 Megs
            const long lowCompressLevel = 99L;
            const long medCompressLevel = 95L;
            const long highCompressLevel = 90L;

            if (length <= lowFileLimit)
            {
                return 100L; // No Compression
            }

            if (length > lowFileLimit && length <= medFileLimit)
            {
                return lowCompressLevel;
            }

            if (length > medFileLimit && length <= highFileLimit)
            {
                return medCompressLevel;
            }

            return highCompressLevel;
        }
    }

        
    }
