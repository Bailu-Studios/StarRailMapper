using System.Net;
using System.Text;

namespace StarRailMapper.Core.Helpers;
public static class Http {
    /// <summary>
    /// GET请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="result">返回的请求体</param>
    /// <returns>请求状态 (0正常,-1异常)</returns>
    public static int HttpGet(string url, out string result) {
        try {
            var http = new HttpClient();
            var response = http.GetByteArrayAsync(url);
            response.Wait();
            result = Encoding.UTF8.GetString(response.Result);
            return 0;
        } catch (Exception e) {
            result = e.Message;
            return -1;
        }
    }

    /// <summary>
    /// Json POST请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="sendData">要发送的数据</param>
    /// <param name="result">返回的请求体</param>
    /// <returns>请求状态 (0正常,-1异常)</returns>
    public static int HttpPostJson(string url, string sendData, out string result) {
        return HttpPost(url, sendData, out result, "application/json");
    }
    
    /// <summary>
    /// POST请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="sendData">要发送的数据</param>
    /// <param name="result">返回的请求体</param>
    /// <param name="contentType">请求类型, 例"application/json"</param>
    /// <returns>请求状态 (0正常,-1异常)</returns>
    public static int HttpPost(string url, string sendData, out string result, string contentType = "") {
        
        try {
            var http = new HttpClient();
            var data = Encoding.UTF8.GetBytes(sendData);
            HttpContent hc = new StreamContent(new MemoryStream(data));
            if (contentType != "") {
                hc.Headers.Add("Content-Type", contentType);
            }
            var response = http.PostAsync(url, hc);
            var body = response.Result.Content.ReadAsByteArrayAsync();
            body.Wait();
            result = Encoding.UTF8.GetString(body.Result);
            return 0;
        } catch (Exception e) {
            result = e.Message;
            return -1;
        }
    }
}