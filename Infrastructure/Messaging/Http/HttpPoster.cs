namespace AppliedSystems.Infrastucture.Messaging.Http
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    public class HttpPoster
    {
        public WebRequest WebRequest { get; private set; }

        private string contentType = "text/xml";

        public HttpPoster ConfigureForUrl(Uri url)
        {
            if (WebRequest == null)
            {
                WebRequest = WebRequest.Create(url);
            }
            return this;
        }

        public HttpPoster WithContentType(string type)
        {
            contentType = type;
            return this;
        }

        public HttpPoster WithTimeout(int timeoutInMilliseconds)
        {
            WebRequest.Timeout = timeoutInMilliseconds;
            return this;
        }

        public string Post(string message)
        {
            WebRequest.Method = "POST";
            WebRequest.ContentType = contentType;

            var bytes = Encoding.UTF8.GetBytes(message);

            WebRequest.ContentLength = bytes.Length;

            var stream = WebRequest.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();

            var response = WebRequest.GetResponse();
            var responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
    }
}