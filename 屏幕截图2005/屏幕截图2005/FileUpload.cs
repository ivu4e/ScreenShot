using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Net;

namespace 屏幕截图2005
{
    /// <summary>
    /// 文件上传类
    /// </summary>
    public class FileUpload
    {
        private string upload_Url = string.Empty;
        private Image upload_Image = null;
        string random = DateTime.Now.Ticks.ToString();
        private Exception last_Exception = null;
        private string status = string.Empty;
        private string remoteUrl = string.Empty;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="uploadUrl"></param>
        /// <param name="uploadImage"></param>
        public FileUpload(string uploadUrl, Image uploadImage)
        {
            this.upload_Url = uploadUrl;
            this.upload_Image = uploadImage;

            if (uploadUrl.IndexOf("?") != -1)
            {
                string lastWord = uploadUrl.Substring(uploadUrl.Length - 1);
                if (lastWord == "&" || lastWord == "?")
                {
                    this.upload_Url += "T=" + random;
                }
                else
                {
                    this.upload_Url += "&T=" + random;
                }
            }
            else
            {
                this.upload_Url += "?T=" + random;
            }
        }

        /// <summary>
        /// 开始上传
        /// </summary>
        public void Start()
        {
            try
            {
                HttpWebRequest HttpWReq = (HttpWebRequest)WebRequest.Create(this.upload_Url);
                HttpWReq.Credentials = CredentialCache.DefaultCredentials;
                HttpWReq.UserAgent = "Dreacom Screen Cutor";
                HttpWReq.Method = "POST";

                string strHTTPBoundary = ("---------------------------" + random);
                string fileName = Guid.NewGuid().ToString("N");
                string strPreFileData = MakePreFileData(strHTTPBoundary, fileName + ".png", fileName);
                string strPostFileData = MakePostFileData(strHTTPBoundary);

                HttpWReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-cn");
                //HttpWReq.Host = HttpWReq.RequestUri.Authority;

                System.IO.MemoryStream mStream = new System.IO.MemoryStream();
                this.upload_Image.Save(mStream, System.Drawing.Imaging.ImageFormat.Png);
                byte[] preBuffer = Encoding.Default.GetBytes(strPreFileData);
                byte[] pstBuffer = Encoding.Default.GetBytes(strPostFileData);
                HttpWReq.ContentType = "multipart/form-data; boundary=" + strHTTPBoundary;
                HttpWReq.ContentLength = preBuffer.Length + pstBuffer.Length + mStream.Length;


                System.IO.Stream reqStream = HttpWReq.GetRequestStream();
                if (reqStream != null)
                {

                    reqStream.Write(preBuffer, 0, preBuffer.Length);
                    reqStream.Write(mStream.GetBuffer(), 0, (int)mStream.Length);

                    reqStream.Write(pstBuffer, 0, pstBuffer.Length);
                    reqStream.Close();
                }
                HttpWebResponse HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();
                if (HttpWResp.StatusCode == HttpStatusCode.OK)
                {
                    this.status = "上传成功!";
                    System.IO.Stream resStream = HttpWResp.GetResponseStream();
                    System.IO.StreamReader sr = new System.IO.StreamReader(resStream, Encoding.Default);
                    this.remoteUrl = sr.ReadToEnd();
                }
                else
                {
                    this.status = HttpWResp.StatusDescription;
                }
                HttpWResp.Close();
            }
            catch (Exception ex)
            {
                this.status = "上传失败!";
                this.last_Exception = ex;
            }
        }

        /// <summary>
        /// 上传产生的异常,如果上传成功则为:NULL
        /// </summary>
        public Exception LastException
        {
            get
            {
                if (this.last_Exception != null)
                {
                    return this.last_Exception;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 上传后的状态
        /// </summary>
        public string Status
        {
            get
            {
                return this.status;
            }
        }

        /// <summary>
        /// 上传成功后的远程URL地址
        /// </summary>
        public string RemoteUrl
        {
            get { return this.remoteUrl; }
        }

        //==============================================
        private string MakeRequestHeaders(string strBoundary, string strServer, int wPort)
        {
            string strFormat = ("Accept: */*\r\nAccept-Language: zh-cn\r\n");
            string strData = ("");
            strFormat += ("Content-Type: multipart/form-data; boundary=%s\r\n");
            strFormat += ("Host: %s:%d\r\n");
            strData = string.Format(strFormat, strBoundary, strServer, wPort);
            return strData;
        }

        private string MakePreFileData(string strBoundary, string strFileName, string strInfo)
        {

            string strFormat = string.Empty;
            string strData = string.Empty;
            strFormat += ("--{0}");
            strFormat += ("\r\n");
            strFormat += ("Content-Disposition: form-data; name=\"guid\"");
            strFormat += ("\r\n\r\n");
            strFormat += ("{1}");
            strFormat += ("\r\n");
            strFormat += ("--{2}");
            strFormat += ("\r\n");
            strFormat += ("Content-Disposition: form-data; name=\"fileData\"; filename=\"{3}\"");
            strFormat += ("\r\n");
            strFormat += ("Content-Type: image/png");
            //strFormat += ("\r\n");
            //strFormat += ("Content-Transfer-Encoding: binary");
            strFormat += ("\r\n\r\n");

            strData = string.Format(strFormat, strBoundary, strInfo, strBoundary, strFileName);

            return strData;
        }

        private string MakePostFileData(string strBoundary)
        {
            string strFormat = string.Empty;
            string strData = string.Empty;

            /*
                strFormat = _T("\r\n");
                strFormat += _T("--%s");
                strFormat += _T("\r\n");
                strFormat += _T("Content-Disposition: form-data; name=\"id\"");
                strFormat += _T("\r\n\r\n");
                strFormat += _T("0061AEFC-F2BC-4FDA-BDCF-836093504B59");*/

            strFormat += ("\r\n");
            strFormat += ("--{0}--");
            strFormat += ("\r\n");

            strData = string.Format(strFormat, strBoundary, strBoundary);

            return strData;
        }
        //==============================================
    }
}
