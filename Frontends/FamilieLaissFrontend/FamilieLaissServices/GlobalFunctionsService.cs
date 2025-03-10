using FamilieLaissInterfaces;
using System;
using System.Globalization;

namespace FamilieLaissServices
{
    public class GlobalFunctionsService : IGlobalFunctions
    {
        public string GetFileSizeAsString(decimal fileSize)
        {
            if (fileSize < 1024)
            {
                return fileSize.ToString(CultureInfo.InvariantCulture) + " Bytes";
            }
            else
            {
                if (fileSize < (1024 * 1024))
                {
                    return Math.Round(fileSize / 1024, 2).ToString(CultureInfo.InvariantCulture) + " KB";
                }
                else
                {
                    if (fileSize < (1024 * 1024 * 1024))
                    {
                        return Math.Round(fileSize / 1024 / 1024, 2).ToString(CultureInfo.InvariantCulture) + " MB";
                    }
                    else
                    {
                        return Math.Round(fileSize / 1024 / 1024 / 1024, 2).ToString(CultureInfo.InvariantCulture) + " GB";
                    }
                }
            }
        }
    }
}
