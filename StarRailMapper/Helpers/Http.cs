using System.Net;
using System.Text;

namespace StarRailMapper.Helpers;
public static class Http
{
    public static int HttpGet(string url, out string reslut)
    {
        reslut = "";
        try
        {
            HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
            wbRequest.Proxy = null;
            wbRequest.Method = "GET";
            HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
            using (Stream responseStream = wbResponse.GetResponseStream())
            {
                using (StreamReader sReader = new StreamReader(responseStream))
                {
                    reslut = sReader.ReadToEnd();
                }
            }
        }
        catch (Exception e)
        {
            reslut = e.Message;     //输出捕获到的异常，用OUT关键字输出
            return -1;              //出现异常，函数的返回值为-1
        }
        return 0;
    }

    public static int HttpPost(string url, string sendData, out string reslut)
    {
        reslut = "";
        try
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(sendData);
            HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);  // 制备web请求
            wbRequest.Proxy = null;     //现场测试注释掉也可以上传
            wbRequest.Method = "POST";
            wbRequest.ContentType = "application/json";
            wbRequest.ContentLength = data.Length;

            //#region //【1】获得请求流，OK
            //Stream newStream = wbRequest.GetRequestStream();
            //newStream.Write(data, 0, data.Length);
            //newStream.Close();//关闭流
            //newStream.Dispose();//释放流所占用的资源
            //#endregion

            #region //【2】将创建Stream流对象的过程写在using当中，会自动的帮助我们释放流所占用的资源。OK
            using (Stream wStream = wbRequest.GetRequestStream())         //using(){}作为语句，用于定义一个范围，在此范围的末尾将释放对象。
            {
                wStream.Write(data, 0, data.Length);
            }
            #endregion

            //获取响应
            HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
            using (Stream responseStream = wbResponse.GetResponseStream())
            {
                using (StreamReader sReader = new StreamReader(responseStream, Encoding.UTF8))      //using(){}作为语句，用于定义一个范围，在此范围的末尾将释放对象。
                {
                    reslut = sReader.ReadToEnd();
                }
            }
        }
        catch (Exception e)
        {
            reslut = e.Message;     //输出捕获到的异常，用OUT关键字输出
            return -1;              //出现异常，函数的返回值为-1
        }
        return 0;
    }
}