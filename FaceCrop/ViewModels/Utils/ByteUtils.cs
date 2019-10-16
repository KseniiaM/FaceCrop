using System;
using System.IO;
using Plugin.Media.Abstractions;

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
                mediaFile.Dispose();
                result = memoryStream.ToArray();
            }

            return result;
        }
    }
}
