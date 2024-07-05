using FamilieLaissInterfaces;
using System;

namespace FamilieLaissServices
{
    public class GlobalFunctionsService : IGlobalFunctions
    {
        public string GetFileSizeAsString(decimal fileSize)
        {
            if (fileSize < 1024)
            {
                return fileSize.ToString() + " Bytes";
            }
            else
            {
                if (fileSize < (1024 * 1024))
                {
                    return Math.Round(fileSize / 1024, 2).ToString() + " KB";
                }
                else
                {
                    if (fileSize < (1024 * 1024 * 1024))
                    {
                        return Math.Round(fileSize / 1024 / 1024, 2).ToString() + " MB";
                    }
                    else
                    {
                        return Math.Round(fileSize / 1024 / 1024 / 1024, 2).ToString() + " GB";
                    }
                }
            }
        }
    }
}
