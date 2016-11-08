using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace 屏幕截图2005
{
    public class WorkOvertime
    {
        //volatile
        private static readonly WorkOvertime _instance = new WorkOvertime();
        private static FileStream fileStream = null;
        private static StreamWriter streamWriter = null;
        
        static WorkOvertime()
        {
            getInstanse();
        }

        public static WorkOvertime Instance {
            get
            {
                return _instance;
            }
	    }

        ~WorkOvertime()
        {
            if (streamWriter != null)
            {
                try
                {
                    streamWriter.Close();
                    streamWriter.Dispose();
                }
                catch (System.Exception ex)
                {
                    System.Console.Out.WriteLine(ex.Message);
                }
                
            }
            if (fileStream != null)
            {
                try
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
                catch (System.Exception ex)
                {
                    System.Console.Out.WriteLine(ex.Message);
                }
               
            }
        }

        /// <summary>
        /// 记录开始加班的时间
        /// </summary>
        public static void OverStart()
        {
            String startTime = "OverStart==" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            getInstanse().WriteLine(startTime);
            getInstanse().Flush();
        }

        /// <summary>
        /// 记录结束加班的时间
        /// </summary>
        public static void OverEnd()
        {
            String startTime = "OverEnd==" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            getInstanse().WriteLine(startTime);
            getInstanse().Flush();
        }

        /// <summary>
        /// 获取StreamWriter实例
        /// </summary>
        /// <returns></returns>
        private static StreamWriter getInstanse()
        {
            //System.AppDomain.CurrentDomain.BaseDirectory
            if (streamWriter == null)
            {
                String fileName = "WorkOvertime_" + DateTime.Now.ToString("yyyy年MM月") + ".log"; //yyyy_MM_dd__HH_mm_ss_fff
                FileStream fileStream = new FileStream(fileName, FileMode.Append);
                
                streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            }
            return streamWriter;
        }

        /// <summary>
        /// 获取日期时间字符串
        /// </summary>
        /// <returns></returns>
        private static string getDataTime()
        {
            string s = "";
            DateTime now = DateTime.Now;
            s = now.Year + "_" + now.Month + "_" + now.Day + "_" + now.Hour + "_" + now.Minute + "_" + now.Second + "_" + now.Millisecond;
            return s;
        }

        /// <summary>
        /// 获取中文格式日期时间
        /// </summary>
        /// <returns></returns>
        private static string getDataTime_CN()
        {
            string s = "";
            DateTime now = DateTime.Now;
            s = now.Year + "年" + now.Month + "月" + now.Day + "日 " + now.Hour + ":" + now.Minute + ":" + now.Second + ":" + now.Millisecond;
            return s;
        }

    }
}
