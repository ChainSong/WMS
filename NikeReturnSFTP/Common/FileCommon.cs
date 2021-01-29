using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NikeReturnSFTP.Common
{
    public class FileCommon
    {

        /// <summary>
        /// 移动文件，可以
        /// </summary>
        /// <param name="sourceFilepath"></param>
        /// <param name="toFilepath"></param>
        public static void MoveToCover(string sourceFilepath, string toFilepath)
        {
            //判断文件是否存在
            if (File.Exists(toFilepath))
            {
                File.Delete(toFilepath);
            }
            File.Move(sourceFilepath, toFilepath);
        }

    }
}
