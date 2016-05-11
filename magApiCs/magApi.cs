using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
namespace magApiCs
{
    public class magApi
    {
        private HttpClient client = new HttpClient();
        private Task<HttpResponseMessage> response;
        /// <summary>
        /// 获取Request
        /// </summary>
        /// <param name="_str"></param>
        /// <param name="_count"></param>
        /// <param name="_offset"></param>
        /// <param name="_attributes"></param>
        private void MakeRequest(string _str,UInt64 _count,UInt64 _offset,string _attributes)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "f7cc29509a8443c5b3a5e56b0e38b5a6");
            // Request parameters
            queryString["expr"] = _str;
            queryString["model"] = "latest";
            queryString["attributes"] = _attributes;
            queryString["count"] = _count.ToString();
            queryString["offset"] = _offset.ToString();
            var uri = "https://oxfordhk.azure-api.net/academic/v1.0/evaluate?" + queryString;
            
            response = client.GetAsync(uri);
        }
        /// <summary>
        /// 以dictionary类返回JSON的内容
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public Dictionary<string,object> GetResponse(string str, UInt64 count = 100, UInt64 offset = 0, string attributes = "Id,F.FId,AA.AuId,AA.AfId,RId,J.JId,C.CId")
        {
            string jsonStr;
            try
            {
                MakeRequest(str, count, offset, attributes);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            JavaScriptSerializer json = new JavaScriptSerializer();
            json.MaxJsonLength = 209715200;
            try
            {
                jsonStr = response.Result.Content.ReadAsStringAsync().Result;
                return json.Deserialize<Dictionary<string, object>>(jsonStr);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
