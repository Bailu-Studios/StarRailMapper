using System.Text;

namespace StarRailMapper.Core.Helpers;

public static class Http
{
    /// <summary>
    /// GET请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="result">返回的请求体</param>
    /// <returns>请求状态 (0正常,-1异常)</returns>
    public static int Get(string url, out string result)
    {
        try
        {
            var http = new HttpClient();
            var response = http.GetByteArrayAsync(url);
            response.Wait();
            result = Encoding.UTF8.GetString(response.Result);
            return 0;
        }
        catch (Exception e)
        {
            result = e.Message;
            return -1;
        }
    }

    public static async Task Download(string url, FileInfo file)
    {
        try
        {
            var http = new HttpClient();
            var response = await http.GetAsync(url);
            // var n = response.Content.Headers.ContentLength; // 总长度
            var stream = await response.Content.ReadAsStreamAsync();
            await using var fileStream = file.Create();
            await using (stream)
            {
                var buffer = new byte[1024];
                // var readLength = 0;
                int length;
                while ((length = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    // readLength += length;
                    // Console.WriteLine("下载进度" + (double)readLength / n * 100); // 进度条
                    // 写入到文件
                    fileStream.Write(buffer, 0, length);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// Json POST请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="sendData">要发送的数据</param>
    /// <param name="result">返回的请求体</param>
    /// <returns>请求状态 (0正常,-1异常)</returns>
    public static int PostJson(string url, string sendData, out string result)
    {
        return Post(url, sendData, out result, "application/json");
    }

    /// <summary>
    /// POST请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="sendData">要发送的数据</param>
    /// <param name="result">返回的请求体</param>
    /// <param name="contentType">请求类型, 例"application/json"</param>
    /// <returns>请求状态 (0正常,-1异常)</returns>
    public static int Post(string url, string sendData, out string result, string contentType = "")
    {
        try
        {
            var http = new HttpClient();
            var data = Encoding.UTF8.GetBytes(sendData);
            HttpContent hc = new StreamContent(new MemoryStream(data));
            if (contentType != "")
            {
                hc.Headers.Add("Content-Type", contentType);
            }

            var response = http.PostAsync(url, hc);
            var body = response.Result.Content.ReadAsByteArrayAsync();
            body.Wait();
            result = Encoding.UTF8.GetString(body.Result);
            return 0;
        }
        catch (Exception e)
        {
            result = e.Message;
            return -1;
        }
    }
}