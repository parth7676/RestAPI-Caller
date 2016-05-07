using AvalaraRestAPIHelper;
using AvalaraRestAPIHelper.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPICaller
{
    public class RestAPICaller
    {
        public Response GetRespone(YouRequestObjectType requestobj)
        {
            //Address of the Rest API which we need to call
            var endPoint = @"https://development.avalara.net/1.0/tax/get";

            //Request Method(POST/GET/DELETE/PUT)
            var method = Enums.HttpVerb.POST;

            //Type of content which we want.
            //Rest API accept two content types.
            //1."text/xml"-Working with XML data
            //2."appplication/json"-Working with Json data
            var contentType = "appplication / json";

            //Basic [account number]:[license key] encoded to Base64,
            //as per basic access authentication.
            //For example: Authorization: Basic a2VlcG1vdmluZzpub3RoaW5nMnNlZWhlcmU
            var rawAuthenticationString = requestobj.key + ":" + requestobj.Password;
            byte[] bytes = Encoding.UTF8.GetBytes(rawAuthenticationString);
            var base64 = Convert.ToBase64String(bytes);
            var authenticationHeader = "Basic" + " " + base64;
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
            requestHeaders.Add("Authorization", authenticationHeader);

            //Optional
            //Additional data that you want to pass with your request
            //In case of Json data object is serialized into string using Json.Net 
            var postData = JsonConvert.SerializeObject
                (requestobj, Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            RestAPIHelper callAvalaraRestAPI = new RestAPIHelper();
            RestAPIResponse restAPIResponse = callAvalaraRestAPI.MakeRequest(endPoint, method, contentType, requestHeaders, postData);
            Response response = new Response();
            response.Status = restAPIResponse.Status;
            if (restAPIResponse.Status == Enums.Status.Success)
            {
                //Cast the reponse object in success out put object.
                response.SuccessResponse = JsonConvert.DeserializeObject<ExpectedSuccesOutPutObject>(restAPIResponse.ResponseData);
                response.ErrorResponse = null;
            }
            else
            {
                response.SuccessResponse = null;
                //Cast the reponse object in error out put object.
                response.ErrorResponse = JsonConvert.DeserializeObject<ExpectedErrorOutPutObject>(restAPIResponse.ResponseData);
            }

            return response;
        }
    }
}
