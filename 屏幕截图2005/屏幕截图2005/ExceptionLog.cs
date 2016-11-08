using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace 屏幕截图2005
{
    /// <summary>
    /// 异常信息日志处理类
    /// </summary>
    public class ExceptionLog
    {
        private static FileStream fileStream = null;
        private static StreamWriter streamWriter = null;

        static ExceptionLog()
        {
            getInstanse();
        }

        ~ExceptionLog()
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

        public static void Log(String msg)
        {
            String methodName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            getInstanse().WriteLine(methodName + ":" + msg);
            getInstanse().Flush();
        }

        public static void Log(Exception ex)
        {
            String methodName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            Save(ex, methodName);
        }

        public static void Log(Exception ex, String methodName)
        {

        }

        public static StreamWriter getInstanse()
        {
            if (streamWriter == null)
            {
                String fileName = DateTime.Now.ToString("yyyy年MM月dd日") + ".log"; //yyyy_MM_dd__HH_mm_ss_fff
                FileStream fileStream = new FileStream(fileName, FileMode.Append);
                
                streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            }
            return streamWriter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="methodName">发生异常的方法名称</param>
        /// <param name="caseValue">发生异常的文件名称</param>
        public static void Log(Exception ex, String methodName, String fileName)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="methodName">发生异常的方法名称</param>
        /// <param name="fileName">发生异常的文件名称</param>
        /// <param name="relatedCode">关键代码或关键变量的值</param>
        public static void Log(Exception ex, String methodName, String fileName, String relatedCode)
        {
            //string s = typeof(类型名).GetMethod(方法名).GetParameters()[0].Name;
        }

        /// <summary>
        /// 保存异常信息到文件。
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int Save(System.Exception e)
        {
            string fileName = "Error_" + getDataTime() + ".log";
            string filePath = getSavePath();
            filePath += fileName;

            string error = "";
            DateTime now = DateTime.Now;
            string CrLf = Convert.ToString((char)13) + Convert.ToString((char)10);
            string Tab = Convert.ToString((char)9);

            error += getDataTime_CN();

            error += CrLf + CrLf;
            error += "Error:" + e.Message;
            error += CrLf + CrLf;
            error += "Source:" + e.Source;
            error += CrLf + CrLf;
            error += "StackTrace:" + e.StackTrace;
            error += CrLf + CrLf;

            if (e.InnerException != null)
            {
                error += "InnerException";
                error += CrLf;
                error += "______________________________________________________________________";
                error += CrLf + CrLf;
                error += "Error:" + e.InnerException.Message;
                error += CrLf + CrLf;
                error += "Source:" + e.InnerException.Source;
                error += CrLf + CrLf;
                error += "StackTrace:" + e.InnerException.StackTrace;
                error += CrLf + CrLf;
            }
            System.Exception baseExp = e.GetBaseException();
            if (baseExp != null)
            {
                error += "BaseException";
                error += CrLf;
                error += "______________________________________________________________________";
                error += CrLf + CrLf;
                error += "Error:" + baseExp.Message;
                error += CrLf + CrLf;
                error += "Source:" + baseExp.Source;
                error += CrLf + CrLf;
                error += "StackTrace:" + baseExp.StackTrace;
                error += CrLf + CrLf;
            }
            SaveTo(filePath, error);
            return 0;
        }

        /// <summary>
        /// 保存异常信息到文件。
        /// </summary>
        /// <param name="e">一个Exception类型的对象。</param>
        /// <param name="methodName">产生异常的方法名称。</param>
        /// <param name="args">参数列表</param>
        /// <returns></returns>
        public static int Save(System.Exception e, string methodName)
        {
            string error = "";
            string CrLf = Convert.ToString((char)13) + Convert.ToString((char)10);
            string Tab = Convert.ToString((char)9);

            error += getDataTime_CN();

            error += CrLf + CrLf;
            error += Tab + "Error:" + e.Message;
            error += CrLf + CrLf;
            error += Tab + "Source:" + e.Source;
            error += CrLf + CrLf;
            error += Tab + "StackTrace:" + e.StackTrace;
            error += CrLf + CrLf;
            error += Tab + "MethodName:" + methodName;
            error += CrLf + CrLf;
            error += Tab + "ArgumentList{" + CrLf;
            
            error += Tab + "}";
            error += CrLf + CrLf;

            getInstanse().WriteLine(error);
            getInstanse().Flush();
            return 0;
        }

        /// <summary>
        /// 保存信息到文件。
        /// </summary>
        /// <param name="TestMessage"></param>
        /// <returns></returns>
        public static int Save(string TestMessage)
        {
            string fileName = "Error_" + getDataTime() + ".log";
            string filePath = getSavePath();
            filePath += fileName;

            string error = "";
            string CrLf = Convert.ToString((char)13) + Convert.ToString((char)10);
            string Tab = Convert.ToString((char)9);

            error += getDataTime_CN();

            error += CrLf + CrLf;
            error += Tab + "TestMessage:" + TestMessage;

            SaveTo(filePath, error);
            return 0;
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

        /// <summary>
        /// 保存内容到文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileContent"></param>
        private static void SaveTo(string filePath, string fileContent)
        {
            try
            {
                ConfirmSavePath(filePath);
                System.IO.FileStream fs = System.IO.File.Open(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                fs.Position = fs.Length;
                byte[] buff = System.Text.Encoding.GetEncoding(936).GetBytes(fileContent);
                fs.WriteByte(13);
                fs.WriteByte(10);
                fs.Write(buff, 0, buff.Length);
                fs.Close();
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 获取保存路径
        /// </summary>
        /// <returns></returns>
        public static string getSavePath()
        {
            string filePath = buildRootPath("/log/");
            string fpath = "";
            if (System.Web.HttpContext.Current != null)
            {
                fpath = System.Web.HttpContext.Current.Server.MapPath(filePath);
                if (!System.IO.Directory.Exists(fpath))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(fpath);
                    }
                    catch { }
                }
            }
            else
            {
                fpath = System.AppDomain.CurrentDomain.BaseDirectory + filePath.Replace("/", "\\");
                fpath = fpath.Replace("\\\\", "\\");
                if (!System.IO.Directory.Exists(fpath))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(fpath);
                    }
                    catch { }
                }
            }
            return fpath;
        }

        /// <summary>
        /// 获取根目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string buildRootPath(string path)
        {
            //string root, vd;
            //root = "";
            //vd = "VirtualDirectory";
            //if (System.Configuration.ConfigurationManager.AppSettings[vd] != null)
            //{
            //    root = System.Configuration.ConfigurationManager.AppSettings[vd].ToString();
            //}
            //root = "/" + root + "/" + path;
            //root = System.Text.RegularExpressions.Regex.Replace(root, "/{2,}", "/", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            string root = string.Empty;
            if (System.Web.HttpContext.Current != null)
            {
                if (System.Web.HttpContext.Current.Request.ApplicationPath.Length != 1)
                {
                    if (path.Substring(0, 1) == "/")
                    {
                        root = System.Web.HttpContext.Current.Request.ApplicationPath + path;
                    }
                    else
                    {
                        root = System.Web.HttpContext.Current.Request.ApplicationPath + "/" + path;
                    }
                }
                else
                {
                    root = path;
                    if (root.Substring(0, 1) != "/") { root = "/" + root; }
                }
            }
            else
            {
                root = path;
                if (root.Substring(0, 1) != "/") { root = "/" + root; }
            }
            return root;
        }

        /// <summary>
        /// 确认存储路径是否存在,不存在则创建
        /// </summary>
        /// <param name="savePath"></param>
        private static void ConfirmSavePath(string savePath)
        {
            try
            {
                string path = System.IO.Path.GetDirectoryName(savePath);
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
            }
            catch (System.Exception e)
            {
                throw new System.Exception("ConfirmSavePath(\"" + savePath + "\")\r\n\r\n" + e.Message + "\r\n\r\n" + e.Source + "\r\n\r\n" + e.StackTrace);
            }
        }
    }

    /// <summary>
    /// 日志类型，日志处理方式
    /// </summary>
    public enum LogType : byte
    {
        /// <summary>
        /// 不做任何处理
        /// </summary>
        None = 0,
        /// <summary>
        /// 调试模式：如果调用的方法产生异常将会抛出异常
        /// </summary>
        DebugMode = 1,
        /// <summary>
        /// 保存错误信息对文件
        /// </summary>
        SaveToFile = 2,
        /// <summary>
        /// 保存错误信息到数据库
        /// </summary>
        SaveToDB = 3
    }
}
