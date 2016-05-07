using AvalaraRestAPIHelper.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace AvalaraRestAPIHelper
{
    public class RestAPIHelper
    {
        public RestAPIResponse MakeRequest(string endpoint, Enums.HttpVerb method, string contentType, Dictionary<string, string> requestHeaders, string postData = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = method.ToString();
            request.ContentType = contentType;
            request.ContentLength = 0;

            //Itrating through requestHeaders to set header 
            foreach (KeyValuePair<string, string> header in requestHeaders)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            if (!string.IsNullOrEmpty(postData) && method == Enums.HttpVerb.POST)
            {
                var encoding = new UTF8Encoding();
                var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(postData);
                request.ContentLength = bytes.Length;

                using (var writeStream = request.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                }
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                RestAPIResponse restAPIResponse = new RestAPIResponse();
                var responseValue = string.Empty;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    restAPIResponse.Status = Enums.Status.Failure;
                }
                else
                {
                    restAPIResponse.Status = Enums.Status.Success;
                }

                // grab the response
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                            restAPIResponse.ResponseData = responseValue;
                        }
                }
               
                return restAPIResponse;
            }
         }
    }
}
