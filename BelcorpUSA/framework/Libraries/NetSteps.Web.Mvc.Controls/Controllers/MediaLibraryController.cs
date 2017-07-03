using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
namespace NetSteps.Web.Mvc.Controls.Controllers
{
    public class MediaLibraryController : Controller
    {
        [OutputCache(Duration = 120, VaryByParam = "path; width; height")]
        public ActionResult ThumbImage(string path, int width, int height)
        {
            if (!System.IO.File.Exists(path))
            {
                throw new System.Web.HttpException(404, "File not found");
            }
            using (var image = Image.FromFile(path))
            {
                //keep aspect ratio
                if (image.Width > image.Height)
                {
                    height = image.Height * width / image.Width;
                }
                else
                {
                    width = image.Width * height / image.Height;
                }
                using (var bmp = new Bitmap(image, width, height))
                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Png);
                    return File(ms.ToArray(), "image/png");
                }
            }
        }
    }
}
