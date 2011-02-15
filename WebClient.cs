using System;
using System.IO;
using System.Net;

namespace H32WP7Helper
{
    /// <summary>
    /// A simple remake of the System.Net.WebClient class, that is easier to use in a more functional programming
    /// style and that DOESN'T USE THE UI THREAD, so it should be more performant.
    /// </summary>
    public class WebClient
    {
        public Uri Uri { get; private set; }

        public bool IsBusy { get; set; }

        public WebClient(string url)
        {
            this.Uri = new Uri(url);
            this.init();
        }

        public WebClient(Uri uri)
        {
            this.Uri = uri;
            this.init();
        }

        public void OpenReadAsync(Action<Stream> callback, Action<Exception> errorCallback)
        {
            this.IsBusy = true;
            var request = createRequest();
            request.BeginGetResponse(
                a =>
                {
                    Stream stream = null;
                    try
                    {
                        stream = request.EndGetResponse(a).GetResponseStream();
                    }
                    catch (Exception ex)
                    {
                        errorCallback(ex);
                    }
                    finally
                    {
                        if (stream != null)
                            callback(stream);
                    }
                }, null);
        }

        public void DownloadStringAsync(Action<string> callback, Action<Exception> errorCallback)
        {
            this.OpenReadAsync(
                stream =>
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string res = null;
                        try
                        {
                            res = reader.ReadToEnd();
                            callback(res);
                        }
                        catch (Exception ex)
                        {
                            errorCallback(ex);
                        }
                        finally
                        {
                            if (res != null)
                                callback(res);
                        }
                    }
                },
                errorCallback);
        }

        private HttpWebRequest createRequest()
        {
            var request = WebRequest.CreateHttp(this.Uri);
            request.UserAgent = "Opera/9.80 (Windows NT 6.1 x64; U; de) Presto/2.7.62 Version/11.00";
            return request;
        }

        private void init()
        {
            this.IsBusy = false;
        }
    }
}
