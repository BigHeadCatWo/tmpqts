using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace restServer
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
        //TODO 响应函数，获得数据后在函数内调用程序计算再返回进行输出
        public responseData getPairAndResponse(RequestData rdata)
        {
            var data = rdata.pair;
            responseData pathList =new responseData();
            //json返回测试
            List<int> a = new List<int>(){ 1, 2 };
            List<int> b = new List<int>() { 1, 3, 2 };
            pathList.pathList = new List<List<int>>() { a, b };
            return pathList;
            //json返回测试
        }
    }
}
