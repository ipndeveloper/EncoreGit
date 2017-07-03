using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace NetSteps.Web.Imaging
{
    /// <summary>
    /// ADDED BY: Travis Watson - CTMH
    /// CREATED: Feb. 14, 2004
    /// 
    /// NOTE 1:
    /// "System.Drawing" had to be added to the project references.
    /// 
    /// NOTE 2:
    /// This class will create a new .JPG that has been resized and compressed.
    /// 
    /// </summary>
    public class ImageResize
    {
        private string mstrError;
        private string mstrDestinationImageFileName;
        private string mstrDestinationImageFullFileName;
        private System.Drawing.Size mobjDestinationImageSize;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ImageResize()
        {
            Reset();
        }

        /// <summary>
        /// Internal reset before each image resize
        /// </summary>
        private void Reset()
        {
            mstrError = string.Empty;
            mstrDestinationImageFileName = string.Empty;
            mstrDestinationImageFullFileName = string.Empty;
            mobjDestinationImageSize = new System.Drawing.Size(-1, -1);
        }

        /// <summary>
        /// Resize and save image as a JPEG
        /// </summary>
        /// <param name="inOriginalFullFileName">The full path of the original file name</param>
        /// <param name="inDestinationFullFileName">The full path of the destination file name</param>
        /// <param name="inCompressionValue">100=best compression, 1=worst, -1=use default value</param>
        /// <param name="inPercentWidthResize">Resize width this percent OR -1 for AUTO SIZE width based on height</param>
        /// <param name="inPercentHeightResize">Resize height this perecent OR -1 for AUTO SIZE height based on width</param>
        /// <param name="inError">Any errors are set here</param>
        /// <returns>TRUE if successfull</returns>
        public bool ResizeAndSaveImageAsJPEG(string inOriginalFullFileName, string inDestinationFullFileName, int inCompressionValue, float inPercentWidthResize, float inPercentHeightResize, bool inDoNotResizeBigger, ref string inError)
        {
            return (DoResizeAndSaveImageAsJPEG(inOriginalFullFileName, inDestinationFullFileName, inCompressionValue, -999, -999, inPercentWidthResize, inPercentHeightResize, inDoNotResizeBigger, ref inError));
        }

        /// <summary>
        /// Resize and save image as a JPEG
        /// </summary>
        /// <param name="inOriginalFullFileName">The full path of the original file name</param>
        /// <param name="inDestinationFullFileName">The full path of the destination file name</param>
        /// <param name="inCompressionValue">100=best compression, 1=worst, -1=use default value</param>
        /// <param name="inWidth">Forced width on the new image OR -1 for AUTO SIZE based on height</param>
        /// <param name="inHeight">Forced heigth on the new image OR -1 for AUTO SIZE based on width</param>
        /// <param name="inError">Any errors are set here</param>
        /// <returns>TRUE if successfull</returns>
        public bool ResizeAndSaveImageAsJPEG(string inOriginalFullFileName, string inDestinationFullFileName, int inCompressionValue, int inWidth, int inHeight, bool inDoNotResizeBigger, ref string inError)
        {
            return (DoResizeAndSaveImageAsJPEG(inOriginalFullFileName, inDestinationFullFileName, inCompressionValue, inWidth, inHeight, -999, -999, inDoNotResizeBigger, ref inError));
        }

        private bool DoResizeAndSaveImageAsJPEG(string inOriginalFullFileName, string inDestinationFullFileName, int inCompressionValue, int inWidth, int inHeight, float inPercentWidthResize, float inPercentHeightResize, bool inDoNotResizeBigger, ref string inError)
        {
            System.Drawing.Size objDestinationSize; // Width and height are determined by parameters

            System.Drawing.Bitmap objOriginalBitmap = null;
            System.Drawing.Bitmap objDestinationBitmap = null;

            inError = string.Empty;
            Reset();

            string strDestinationFullFileName = ForceFileExtension(inDestinationFullFileName, ".JPG"); // In case they are resizing another image besids a .JPG file
            string strOriginalJustFileName = string.Empty;
            string strDestinationJustFileName = string.Empty;
            long lngCompressionValue = Convert.ToInt64(inCompressionValue); // long = Int64
            if (lngCompressionValue == -1) // IF they pass in a compression value of -1, use a default value
            {
                lngCompressionValue = 80; // This default JPEG compression value could be set in the web.config
            }
            if (lngCompressionValue < 1)
            {
                lngCompressionValue = 1;
            }
            else if (lngCompressionValue > 100)
            {
                lngCompressionValue = 100;
            }

            // Get the exact filenames
            strOriginalJustFileName = GetFileNameOnly(inOriginalFullFileName);
            if (strOriginalJustFileName.Length == 0)
            {
                inError = "Invalid original filename passed in for image resize";
                mstrError = inError;
                return (inError.Length == 0);
            }
            strDestinationJustFileName = GetFileNameOnly(strDestinationFullFileName);
            if (strDestinationJustFileName.Length == 0)
            {
                inError = "Invalid destination filename passed in for image resize";
                mstrError = inError;
                return (inError.Length == 0);
            }

            // See if the original image passed in even exists
            if (!System.IO.File.Exists(inOriginalFullFileName))
            {
                inError = "Could not find an image to resize";
                mstrError = inError;
                return (inError.Length == 0);
            }

            // JUST OVERWRITE IT....
            //// See if the destination file already exists
            ////if (System.IO.File.Exists(strDestinationFullFileName))
            ////{
            ////	inError = "The destination image already exists";
            ////  mstrError = inError;
            ////	return (inError);
            ////}

            // Load the image
            objOriginalBitmap = new Bitmap(inOriginalFullFileName);

            // Setup final destination width and height values depending on parameters (forced width and height OR percentage width and height)
            objDestinationSize = SetupWidthAndHeight(objOriginalBitmap.Size, inWidth, inHeight, inPercentWidthResize, inPercentHeightResize, inDoNotResizeBigger);

            // Ccreate a new resized image in memory
            objDestinationBitmap = ResizeImage(objOriginalBitmap, objDestinationSize.Width, objDestinationSize.Height, ref inError);

            // Close the original image JUST IN CASE the destination image has the same file name as the original
            objOriginalBitmap.Dispose();

            // Save the newly sized image in memory as a JPEG with compression to file
            SaveAsJPEG(objDestinationBitmap, strDestinationFullFileName, lngCompressionValue, ref inError);

            // Remember values
            mstrDestinationImageFileName = strDestinationJustFileName;
            mstrDestinationImageFullFileName = strDestinationFullFileName;
            mobjDestinationImageSize = objDestinationSize;

            // Cleanup
            objDestinationBitmap.Dispose();

            mstrError = inError;
            return (inError.Length == 0);
        }

        /// <summary>
        /// Sets up the destination width and heigth depending on the parameters passed in
        /// </summary>
        private System.Drawing.Size SetupWidthAndHeight(System.Drawing.Size inOriginalSize, int inWidth, int inHeight, float inPercentWidthResize, float inPercentHeightResize, bool inDoNotResizeBigger)
        {
            System.Drawing.Size objFinalReturnSize;
            int intFinalWidth = -1;  // Default to AUTO SIZE
            int intFinalHeight = -1; // Default to AUTO SIZE
            float fltTempResizePercentage;

            // Setup Width
            if (inWidth != -999)
            {
                intFinalWidth = inWidth; // NOTE: It could still be set to -1 (AUTO SIZE)
            } // ELSE use percentage value
            else if (inPercentWidthResize != -999)
            {
                if (inPercentWidthResize != -1)
                {
                    intFinalWidth = PercentResize(inOriginalSize.Width, inPercentWidthResize);
                }
            }

            // Setup Height
            if (inHeight != -999)
            {
                intFinalHeight = inHeight; // NOTE: It could still be set to -1 (AUTO SIZE)
            } // ELSE use percentage value
            else if (inPercentHeightResize != -999)
            {
                if (inPercentHeightResize != -1)
                {
                    intFinalHeight = PercentResize(inOriginalSize.Height, inPercentHeightResize);
                }
            }

            // If both final values are -1, then use the original image size
            // OR see if either is set to AUTO SIZE
            if ((intFinalWidth == -1) && (intFinalHeight == -1))
            {
                intFinalWidth = inOriginalSize.Width;
                intFinalHeight = inOriginalSize.Height;
            }
            else if (intFinalWidth == -1)
            {   // then get the width value from intFinalHeight to determine intFinalWidth
                fltTempResizePercentage = Convert.ToSingle(intFinalHeight) / Convert.ToSingle(inOriginalSize.Height);
                intFinalWidth = PercentResize(inOriginalSize.Width, fltTempResizePercentage);
            }
            else if (intFinalHeight == -1)
            {   // then get the width value from intFinalWidth to determine intFinalHeight
                fltTempResizePercentage = Convert.ToSingle(intFinalWidth) / Convert.ToSingle(inOriginalSize.Width);
                intFinalHeight = PercentResize(inOriginalSize.Height, fltTempResizePercentage);
            }

            // Check if original size is smaller then the new resize IF we do not want to resize bigger
            if (inDoNotResizeBigger)
            {
                if ((intFinalWidth > inOriginalSize.Width) || (intFinalHeight > inOriginalSize.Height))
                {
                    intFinalWidth = inOriginalSize.Width;
                    intFinalHeight = inOriginalSize.Height;
                }
            }

            // Now set our return values
            objFinalReturnSize = new System.Drawing.Size(intFinalWidth, intFinalHeight);
            return (objFinalReturnSize);
        }

        private int PercentResize(int inSize, float inPercentResize)
        {
            if (inPercentResize <= 0)
            {
                return (inSize);
            }
            double dblNewSize = System.Math.Round(Convert.ToDouble(inSize) * inPercentResize);
            return (Convert.ToInt32(dblNewSize));
        }

        /// <summary>
        /// Used in cases where a .GIF file was passed in and a .JPG file is saved
        /// So the JPG file doesn't get saved with a .gif extension
        /// </summary>
        /// <param name="inFileName">a file name, with or without full path</param>
        /// <param name="inForcedExtension">EG: .JPG (must have the period)</param>
        /// <returns> a file with the newly forced extension</returns>
        public string ForceFileExtension(string inFileName, string inForcedExtension)
        {
            string strNewImageName = inFileName.Trim();
            string strCurrentExtension = string.Empty;
            int intOffset = strNewImageName.LastIndexOf('.');
            if (intOffset >= 0)
            {
                strCurrentExtension = strNewImageName.Substring(intOffset);
            }
            if (strCurrentExtension.ToUpper() != inForcedExtension.ToUpper())
            {
                strNewImageName = strNewImageName.Substring(0, intOffset) + inForcedExtension;
            }
            return (strNewImageName);
        }

        /// <summary>
        /// Returns just the name of the file passed in
        /// </summary>
        private string GetFileNameOnly(string inFullImageName)
        {
            string strNewImageName = inFullImageName.Replace('\\', '/');
            int intOffset = strNewImageName.LastIndexOf('/');
            if (intOffset >= 0)
            {
                strNewImageName = strNewImageName.Substring(intOffset + 1);
            }
            return (strNewImageName);
        }

        /// <summary>
        /// Returns a resized image
        /// </summary>
        private System.Drawing.Bitmap ResizeImage(System.Drawing.Bitmap inBitmap, int inWidth, int inHeight, ref string inError)
        {
            System.Drawing.Bitmap objResizedDestinationImage = null;
            try
            {
                objResizedDestinationImage = new System.Drawing.Bitmap(inWidth, inHeight);
                System.Drawing.Graphics objGraphics = Graphics.FromImage((System.Drawing.Image)objResizedDestinationImage);
                objGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; // Bicubic; // HighQualityBicubic;
                objGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                objGraphics.DrawImage(inBitmap, 0, 0, inWidth, inHeight);

                objGraphics.Dispose();
            }
            catch (System.Exception objEx)
            {
                inError = objEx.Message;
            }
            return (objResizedDestinationImage);
        }

        /// <summary>
        /// Save the image as a jpg
        /// </summary>
        /// <param name="inImage">The Image to save</param>
        /// <param name="inSaveAsFileName">The filename to save the image as</param>
        /// <param name="inCompressionValue">Number between 1 and 100 (100 is best quality)</param>
        /// <returns>TRUE on success ELSE FALSE on failure</returns>
        private bool SaveAsJPEG(System.Drawing.Image inImage, string inSaveAsFileName, long inCompressionValue, ref string inError)
        {
            inError = string.Empty;
            try
            {
                System.Drawing.Imaging.EncoderParameters objEncodingParameters = new System.Drawing.Imaging.EncoderParameters(1);
                objEncodingParameters.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, inCompressionValue);
                System.Drawing.Imaging.ImageCodecInfo objImageCodecInfo = GetEncoderInfo("image/jpeg");
                inImage.Save(inSaveAsFileName, objImageCodecInfo, objEncodingParameters);
            }
            catch (System.Exception objEx)
            {
                inError = objEx.Message;
            }
            return (inError.Length == 0);
        }

        private System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(string inDesiredMimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == inDesiredMimeType)  // EG: [0]="image/bmp", [1]="image/jpeg" 
                    return encoders[j];
            }
            return null; // Desired mimetype NOT FOUND!
        }


        public string DestinationImageFileName
        {
            get
            { return (mstrDestinationImageFileName); }
        }

        public string DestinationImageFullFileName
        {
            get
            { return (mstrDestinationImageFullFileName); }
        }

        // NOT NEEDED
        //public System.Drawing.Size DestinationImageSize
        //{
        //	get
        //	{	return (mobjDestinationImageSize);	}
        //}

        public int DestinationImageSizeWidth
        {
            get
            { return (mobjDestinationImageSize.Width); }
        }

        public int DestinationImageSizeHeight
        {
            get
            { return (mobjDestinationImageSize.Height); }
        }

        public string Error
        {
            get
            { return (mstrError); }
        }

    }
}
