using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using magApiCs;
using System.Collections;
using algorithm2byWang;
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
        ////TODO 响应函数，获得数据后在函数内调用程序计算再返回进行输出
        //public ResponseDataPost postPairAndResponse(RequestDataPost rData)
        //{
        //    var data = rData.pair;
        //    ResponseDataPost pathList =new ResponseDataPost();
        //    //json返回测试
        //    List<int> a = new List<int>(){ 1, 2 };
        //    List<int> b = new List<int>() { 1, 3, 2 };
        //    pathList.pathList = new List<List<int>>() { a, b };
        //    return pathList;
        //    //json返回测试
        //}

        //TODO,响应函数,GET方式，等主程序修好后在这里调用并进行最后的答案处理。
        public List<List<UInt64>> getPairAndResponseIdId(UInt64 id1 = 0, UInt64 id2 = 0, UInt64 auid1 = 0, UInt64 auid2 = 0)
        {
            List<KeyValuePair<string, UInt64>> pair = new List<KeyValuePair<string, ulong>>();
            //判断输入参数是id还是AuId
            if (is_id_auid(id1))
                pair.Add(new KeyValuePair<string, ulong>("AA.AuId", id1));
            else
                pair.Add(new KeyValuePair<string, ulong>("Id", id1));
            if (is_id_auid(id2))
                pair.Add(new KeyValuePair<string, ulong>("AA.AuId", id2));
            else
                pair.Add(new KeyValuePair<string, ulong>("Id", id2));
            //
            //TODO:z在这里调用主程序，返回List<List<UInt64>>
            //
            //json 返回测试
            algorithm2byWang.algorithm2byWang test = new algorithm2byWang.algorithm2byWang();
            List<List<UInt64>> pathList = test.solve(pair[0], pair[1]);
            return pathList;
            //json返回测试
        }
        /// <summary>
        /// 判断输入id是否为AA.AuId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool is_id_auid(UInt64 id)
        {
            magApi mag = new magApi();
            ArrayList attr = new ArrayList();

            StringBuilder str = new StringBuilder("composite(AA.AuId=");
            str.Append(id.ToString());
            str.Append(')');
            Dictionary<string, Object> dataJson = mag.GetResponse(str: str.ToString(), count: 1, attributes: "Id");
            attr = ((ArrayList)dataJson["entities"]);
            SortedSet<KeyValuePair<string, UInt64>> nodeList = convertJsonDatoToList(attr);
            if (nodeList.Count != 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 构造pair的比较函数供set使用
        /// </summary>
        private class SortedSetComparer : IComparer<KeyValuePair<string, UInt64>>
        {
            public int Compare(KeyValuePair<string, UInt64> x, KeyValuePair<string, UInt64> y)
            {
                if (x.Key != y.Key)
                    return x.Key.CompareTo(y.Key);
                else
                    return x.Value.CompareTo(y.Value);
            }
        }
        private SortedSet<KeyValuePair<string, UInt64>> convertJsonDatoToList(ArrayList attr)
        {
            SortedSet<KeyValuePair<string, UInt64>> nodeList = new SortedSet<KeyValuePair<string, UInt64>>(new SortedSetComparer());

            foreach (Dictionary<string, object> s in attr)
            {
                foreach (KeyValuePair<string, object> h in s)
                {
                    if (h.Key == "logprob")
                        continue;
                    if (h.Key == "Id")
                    {
                        nodeList.Add(new KeyValuePair<string, ulong>(h.Key, Convert.ToUInt64(h.Value)));
                        continue;
                    }
                    if (h.Key == "RId" || h.Key == "Id")
                    {
                        foreach (object t in (ArrayList)h.Value)
                        {
                            string tkey = "Id";
                            nodeList.Add(new KeyValuePair<string, ulong>(tkey, Convert.ToUInt64(t)));
                        }
                        continue;
                    }
                    if (h.Key == "J")
                    {
                        foreach (KeyValuePair<string, object> t in (Dictionary<string, object>)h.Value)
                        {
                            nodeList.Add(new KeyValuePair<string, ulong>("J." + t.Key, Convert.ToUInt64(t.Value)));
                        }
                        continue;
                    }
                    foreach (Dictionary<string, object> h1 in (ArrayList)h.Value)
                    {
                        foreach (KeyValuePair<string, object> t in h1)
                        {
                            string tkey = h.Key + '.' + t.Key;
                            nodeList.Add(new KeyValuePair<string, ulong>(tkey, Convert.ToUInt64(t.Value)));
                        }
                    }
                }
            }

            return nodeList;
        }
    }
}
