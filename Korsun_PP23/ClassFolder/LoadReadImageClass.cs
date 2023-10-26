using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Korsun_PP23.ClassFolder
{
    class LoadReadImageClass
    {

        public static BitmapImage BytesToImage(byte[] array)
        {
            if (array != null)
            {

                using (MemoryStream ms = new MemoryStream(array, 0, array.Length))
                {

                    var image = new BitmapImage();

                    image.BeginInit();


                    image.CacheOption = BitmapCacheOption.OnLoad;

                    image.StreamSource = ms;

                    image.EndInit();

                    return image;
                }
            }
            return null;
        }


        public static byte[] ImageToByte(string selectedPhotoName)
        {

            Bitmap bitmap = new Bitmap(selectedPhotoName);
            ImageFormat imageFormat = bitmap.RawFormat;

            var imageToConvert = Image.FromFile(selectedPhotoName);

            using (MemoryStream ms = new MemoryStream())
            {
                imageToConvert.Save(ms, imageFormat);


                return ms.ToArray();
            }

        }




    }
}
