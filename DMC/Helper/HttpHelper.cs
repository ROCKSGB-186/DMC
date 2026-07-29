using DMC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DMC.Helper
{
    /// <summary>
    /// Http辅助类
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// 获取提交数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetPostData<T>(T model)
        {
            var jsonStr = string.Empty;
            var props = typeof(T).GetProperties();
            foreach (var item in props)
            {
                jsonStr += $"{item.Name}={item.GetValue(model, null)}&";
            }

            return jsonStr.TrimEnd('&');
        }


        //show

        /// <summary>
        /// Get获取数据
        /// </summary>
        /// <param name="getUrl"></param>
        /// <param name="resultData"></param>
        /// <returns></returns>
        public static bool GetData(string getUrl, ref string resultData)
        {
            try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getUrl);
                request.Timeout = int.MaxValue;
                request.Method = "GET";
                using (HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(myResponse.GetResponseStream(), encoding))
                    {
                        resultData = sr.ReadToEnd();
                    }
                }
                return true;
            }
            catch (Exception exp)
            {
                resultData = exp.Message;

                return false;
            }
        }

        /// <summary>
        /// Get获取数据
        /// </summary>
        /// <param name="getUrl"></param>
        /// <param name="resultData"></param>
        /// <returns></returns>
        public static bool GetData(string getUrl, Dictionary<string, string> headers, ref string resultData)
        {
            try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getUrl);
                request.Timeout = int.MaxValue;
                request.Method = "GET";
                if (headers != null)
                {
                    foreach (var obj in headers)
                    {
                        request.Headers.Add(obj.Key, obj.Value);
                    }
                }
                using (HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(myResponse.GetResponseStream(), encoding))
                    {
                        resultData = sr.ReadToEnd();
                    }
                }

                if (AppGlobalModel.Logging)
                {
                    LogHelper.WriteLocalLog(new object(), $"路径：{getUrl} \r\nHeaders：{JsonConvert.SerializeObject(headers)} \r\n返回值：{resultData}", "HttpGet");
                }

                return true;
            }
            catch (Exception exp)
            {
                LogHelper.WriteLocalErrorLog(new object(), $"路径：{getUrl} \r\nHeaders：{JsonConvert.SerializeObject(headers)} \r\n错误信息：{exp.ToString()}", "HttpGet");

                resultData = exp.Message;

                return false;
            }
        }

        /// <summary>
        /// Post提交数据
        /// </summary>
        /// <param name="postUrl">Url</param>
        /// <param name="postData">提交数据</param>
        /// <param name="resultData">结果数据</param>
        /// <returns></returns>
        public static bool PostData(string postUrl, string postData, ref string resultData)
        {
            return PostData(postUrl, postData, "application/x-www-form-urlencoded;charset=utf-8", null, ref resultData);
        }
        /// <summary>
        /// Post提交数据
        /// </summary>
        /// <param name="postUrl">Url</param>
        /// <param name="postData">提交数据</param>
        /// <param name="headers">HTTP标头键值对</param>
        /// <param name="resultData">结果数据</param>
        /// <returns></returns>
        public static bool PostData(string postUrl, string postData, Dictionary<string, string> headers, ref string resultData)
        {
            return PostData(postUrl, postData, "application/x-www-form-urlencoded;charset=utf-8", headers, ref resultData);
        }
        /// <summary>
        /// Post提交数据
        /// </summary>
        /// <param name="postUrl">Url</param>
        /// <param name="postData">提交数据</param>
        /// <param name="contentType">Content-type HTTP 标头的值</param>
        /// <param name="resultData">结果数据</param>
        /// <returns></returns>
        public static bool PostData(string postUrl, string postData, string contentType, ref string resultData)
        {
            return PostData(postUrl, postData, contentType, null, ref resultData);
        }
        /// <summary>
        /// Post提交数据
        /// </summary>
        /// <param name="postUrl">Url</param>
        /// <param name="postData">提交数据</param>
        /// <param name="contentType">Content-type HTTP 标头的值</param>
        /// <param name="headers">HTTP标头键值对</param>
        /// <param name="resultData">结果数据</param>
        /// <returns></returns>
        public static bool PostData(string postUrl, string postData, string contentType, Dictionary<string, string> headers, ref string resultData)
        {
            try
            {

                Encoding encoding = Encoding.UTF8;
                byte[] dataByte = encoding.GetBytes(postData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
                //请求版本信息超时默认值
                if (!postUrl.Contains("/app/getVersion"))
                {
                    request.Timeout = int.MaxValue;
                }
                request.Method = "POST";
                request.ContentType = contentType;
                //request.ContentType = "application/json;charset=utf-8";
                //request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentType = "multipart/form-data";
                request.ContentLength = dataByte.Length;
                if (headers != null)
                {
                    foreach (var obj in headers)
                    {
                        request.Headers.Add(obj.Key, obj.Value);
                    }
                }
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(dataByte, 0, dataByte.Length);
                }
                using (HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(myResponse.GetResponseStream(), encoding))
                    {
                        resultData = sr.ReadToEnd();
                    }
                }

                if (AppGlobalModel.Logging)
                {
                    LogHelper.WriteLocalLog(new object(), $"路径：{postUrl} \r\nHeaders：{JsonConvert.SerializeObject(headers)} \r\n参数：{postData}\r\n返回值：{resultData}", "HttpPost");
                }

                return true;
            }
            catch (Exception exp)
            {
                LogHelper.WriteLocalErrorLog(new object(), $"路径：{postUrl} \r\nHeaders：{JsonConvert.SerializeObject(headers)} \r\n参数：{postData}\r\n错误信息：{exp.ToString()}", "HttpPost");

                resultData = exp.Message;

                return false;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <param name="path"></param>
        /// <param name="resultData"></param>
        /// <param name="paras"></param>
        /// <param name="paraName"></param>
        /// <returns></returns>
        public static bool HttpUploadFile(string url, string token, string path, ref string resultData, Dictionary<string, string> paras = null, string paraName = "file")
        {
            try
            {
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Timeout = int.MaxValue;
                request.Method = "POST";
                request.Headers.Add("token", token);

                string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
                request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
                byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
                byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                int pos = path.LastIndexOf("\\");
                string fileName = path.Substring(pos + 1);

                //请求头部信息
                StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"" + paraName + "\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                byte[] bArr = new byte[fs.Length];
                fs.Read(bArr, 0, bArr.Length);
                fs.Close();

                Stream postStream = request.GetRequestStream();

                //1.1 key/value
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

                if (paras != null)
                {
                    foreach (var obj in paras)
                    {
                        postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                        string formitem = string.Format(formdataTemplate, obj.Key, obj.Value);
                        byte[] formitembytes = Encoding.GetEncoding("UTF-8").GetBytes(formitem);
                        postStream.Write(formitembytes, 0, formitembytes.Length);
                    }
                }

                postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                postStream.Write(bArr, 0, bArr.Length);
                postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                postStream.Close();

                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream, Encoding.UTF8);
                //返回结果网页（html）代码
                resultData = sr.ReadToEnd();

                if (AppGlobalModel.Logging)
                {
                    LogHelper.WriteLocalLog(new object(), $"路径：{url} \r\nToken：{token} \r\n参数：文件路径：{path}\r\n文件名：{paraName}\r\n{(paras == null ? "" : JsonConvert.SerializeObject(paras))}\r\n返回值：{resultData}", "HttpUpload");
                }

                return true;
            }
            catch (Exception exp)
            {
                LogHelper.WriteLocalErrorLog(new object(), $"路径：{url} \r\nToken：{token} \r\n参数：文件路径：{path}\r\n文件名：{paraName}\r\n{(paras == null ? "" : JsonConvert.SerializeObject(paras))}\r\n错误信息：{exp.ToString()}", "HttpUpload");

                resultData = exp.Message;

                return false;
            }
        }
    }
}
