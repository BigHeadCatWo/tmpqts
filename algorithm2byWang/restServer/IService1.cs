using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
    
namespace restServer
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        //// TODO: 需要根据比赛将URL进行修改
        //// 使用POST方法获得输入的json，调用函数计算并发送json
        //[OperationContract]
        //[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, RequestFormat =WebMessageFormat.Json, UriTemplate = "restTest/post")]
        //ResponseDataPost postPairAndResponse(RequestDataPost rData);
        /// <summary>
        /// 使用get方法进行xiang'y
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <param name="auid1"></param>
        /// <param name="auid2"></param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "rest/get?id1={id1}&id2={id2}&auid1={auid1}&auid2={auid2}")]
        List<List<UInt64>> getPairAndResponseIdId(UInt64 id1=0, UInt64 id2=0,UInt64 auid1=0,UInt64 auid2=0);
    }


    // 使用下面示例中说明的数据约定将复合类型添加到服务操作。
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
    //TODO 输入数据的数据结构，需要根据比赛进行修改，尤其是key
    public class RequestDataPost
    {
        [DataMember]
        public List<int> pair { get; set; }

    }
    //TODO 输出数据的数据结构，需要根据比赛进行修改，尤其是key
    public class ResponseDataPost
    {
        [DataMember]
        public List<List<int>> pathList { get; set; }
    }

    public class RequestDataGet
    {
       public List<KeyValuePair<string, UInt64>> inputPair { get; set; }
    }
    public class ResponseDataGet
    {
       public List<UInt64> pathList { get; set; }
    }
}
