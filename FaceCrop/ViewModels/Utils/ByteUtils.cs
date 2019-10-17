using System;
using System.IO;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace ViewModels
{
    public static class ByteUtils
    {
        public static byte[] ConvertMediaFileToByteArray(MediaFile mediaFile)
        {
            byte[] result;

            using (var memoryStream = new MemoryStream())
            {
                mediaFile.GetStream().CopyTo(memoryStream);
                result = memoryStream.ToArray();
            }

            return result;
        }
    }
}
