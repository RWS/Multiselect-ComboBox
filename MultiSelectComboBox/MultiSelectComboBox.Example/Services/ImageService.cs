using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace Sdl.MultiSelectComboBox.Example.Services
{
	public class ImageService
	{
		private static readonly string ExecutingAssemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		public static BitmapImage GetImage(string path, string name, Size imageSize)
		{
			try
			{
				var filePath = Path.Combine(ExecutingAssemblyFolder, path, name + ".ico");
				if (!File.Exists(filePath))
				{
					return null;
				}

				Icon icon;
				using (var stream = new FileStream(filePath, FileMode.Open))
				{
					icon = new Icon(stream, imageSize);
					stream.Flush();
					stream.Close();
				}

				var bitmap = icon.ToBitmap();
				bitmap.MakeTransparent();

				return Convert(bitmap);
			}
			catch
			{
				return null;
			}
		}

		private static BitmapImage Convert(object value)
		{
			if (value != null && value is Image image)
			{
				var memoryStream = new MemoryStream();
				var bitmap = new BitmapImage();
				bitmap.BeginInit();
				image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
				memoryStream.Seek(0, SeekOrigin.Begin);
				bitmap.StreamSource = memoryStream;
				bitmap.EndInit();

				bitmap.Freeze();

				return bitmap;
			}

			return null;
		}
	}
}
