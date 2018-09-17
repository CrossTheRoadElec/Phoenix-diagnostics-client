using System;
using System.IO;

namespace CTRE.Phoenix.dotNET
{
    public class File
    {
        public static byte[] Read(string filePath)
        {
            byte[] retval = null;
            try
            {
                retval = System.IO.File.ReadAllBytes(filePath);
            }
            catch (Exception excep)
            {
                Debug.Print("FILE", excep.ToString());

            }
            return retval;
        }

        public enum ParseResult
        {
            Folder,
            File,
            Unknown
        }

        public static ParseResult ParseArbitraryPath(string folderOrFile, out string folderPath, out string fileName)
        {
            if (System.IO.Directory.Exists(folderOrFile))
            {
                folderPath = folderOrFile;
                fileName = string.Empty;
                return ParseResult.Folder;
            }

            if (System.IO.File.Exists(folderOrFile))
            {
                folderPath = Path.GetDirectoryName(folderOrFile);
                fileName = Path.GetFileName(folderOrFile);
                return ParseResult.File;
            }

            folderPath = string.Empty; ;
            fileName = string.Empty;
            return ParseResult.Unknown;
        }
    }
}
