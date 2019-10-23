using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDService.Models
{
    public class Write
    {
        public void Writing (string filePath, string fileName, string strLog, bool isAppend = true)
        {
            try
            {
                StreamWriter logWriter = new StreamWriter(filePath + fileName, isAppend);
                logWriter.WriteLine(strLog);
                logWriter.Close();
                logWriter.Dispose();
            }
            catch (Exception)
            {
                
            }
        }
    }
}
