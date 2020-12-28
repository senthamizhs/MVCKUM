using System;
using System.Net;

namespace STSTRAVRAYS
{

    public class MyWebClient : WebClient
    {
        public int LintTimeout = 200000;
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Timeout = LintTimeout;
            return request;
        }

    }
}
