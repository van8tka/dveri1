using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace dveri2.DopMethod
{
    public class DellFilesFromDomain
    {
        //метод удаления файлов из каталога
        public static void DellAllFiles(string domainpath)
        {
            try
            {
                var dir = new DirectoryInfo(domainpath);
                FileInfo[] fileNames = dir.GetFiles("*.*");
                foreach (var item in fileNames)
                {
                    item.Delete();
                }
            }
            catch (Exception er)
            {
                ClassLog.Write(er);
            }
        }
    }
}