using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Model;
using Color = System.Drawing.Color;

namespace raceWPF
{
    class PictureHandler
    {
        private static Dictionary<String,Bitmap> _images;
        private static Dictionary<String, Bitmap> _cars;
        private static Dictionary<String, Bitmap> _carsBroken;

        public const string greenCar = @"fotos/greenCar.png";
        public const string greenCarBroken = @"fotos/greenCarBroken.png";
        public const string straight = @"fotos/straightLeftToRight.png";
        public const string finish = @"fotos/straightLeftToRightFinish.png";
        public const string start = @"fotos/straightLeftToRightStart.png";

        //fills the _images dictionary and return the correct image to the link
        //returns null if there isnt a picture
        public static Bitmap GetImageBitmap(String imageLink)
        {
            if (object.Equals(_images, null))
            {
                _images = new Dictionary<string, Bitmap>();
                _images.Add("EmptyGreen", PictureHandler.BitmapFromWidthHeight(300, 300));
            }

            Bitmap bitmap;

            _images.TryGetValue(imageLink, out bitmap);
            if (!Equals(bitmap, null)) { return bitmap; }
            try
            {
                Uri uri = new Uri(imageLink, UriKind.Relative);
                bitmap = (Bitmap)Bitmap.FromStream(Application.GetContentStream(uri).Stream);
            }
            catch (Exception e)
            {
                bitmap = null;
            }

            _images.Add(imageLink, bitmap);
            return bitmap;
        }
        public static Bitmap GetImageCarsBitmap(TeamColors teamColors)
        {
            if (object.Equals(_cars, null))
            {
                _cars = new Dictionary<string, Bitmap>();
                }

            Bitmap bitmap;

            _cars.TryGetValue(teamColors.ToString(), out bitmap);
            if (!Equals(bitmap, null)) { return bitmap; }
            try
            {
                bitmap = GetCarOfColor(Color.FromArgb(teamColors.GetHashCode()), greenCar);
                
            }
            catch (Exception e)
            {
                bitmap = null;
            }

            _cars.Add(teamColors.ToString(), bitmap);
            return bitmap;
        }

        public static Bitmap GetImageCarsBrokenBitmap(TeamColors teamColors)
        {
            if (object.Equals(_carsBroken, null))
            {
                _carsBroken = new Dictionary<string, Bitmap>();
            }

            Bitmap bitmap;

            _carsBroken.TryGetValue(teamColors.ToString(), out bitmap);
            if (!Equals(bitmap, null)) { return bitmap; }
            try
            {
                bitmap = GetCarOfColor(Color.FromArgb(teamColors.GetHashCode()), greenCarBroken);

            }
            catch (Exception e)
            {
                bitmap = null;
            }

            _carsBroken.Add(teamColors.ToString(), bitmap);
            return bitmap;
        }
        //clear the image cache
        public static void clearImageCache()
        {
            _images.Clear();
        }

        public static Bitmap BitmapFromWidthHeight(int height, int width)
        {

            Bitmap bitmap = new Bitmap(height, width);
            using (Graphics graph = Graphics.FromImage(bitmap))
            {
                Rectangle ImageSize = new Rectangle(0, 0, height, width);
                
                graph.FillRectangle(System.Drawing.Brushes.Green, ImageSize);
            }
            return bitmap;
        }
        

        public static void ResizeEmptyGreen(int height,int width)
        {
            GetImageBitmap("EmptyGreen");
            _images["EmptyGreen"] = BitmapFromWidthHeight(height, width);

        }

        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException("bitmap");
            }

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        public static Bitmap GetCarOfColor(System.Drawing.Color toColor , String locatie)
        {
            toColor = Color.FromArgb(255, toColor);
            Bitmap image = (Bitmap)GetImageBitmap(locatie).Clone();
            System.Drawing.Color fromColor = Color.FromArgb(255,0, 255, 0);
            ImageAttributes attributes = new ImageAttributes();
            
            attributes.SetRemapTable(new ColorMap[]
            {
                new ColorMap()
                {
                    OldColor = fromColor,
                    NewColor = toColor,
                }
            }, ColorAdjustType.Bitmap);

            using (Graphics g = Graphics.FromImage(image))
            {
                g.DrawImage(
                    image,
                    new Rectangle(System.Drawing.Point.Empty, image.Size),
                    0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel,
                    attributes);
            }

            return image;
        }
    }
}
