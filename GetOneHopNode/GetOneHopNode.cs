using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using magApiCs;
using System.Collections;
namespace GetOneHopNode
{

    public class GetOneHopNodeClass
    {
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

        /// <summary>
        /// 获取1-hop节点
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <returns></returns>
        public SortedSet<KeyValuePair<string, UInt64>> getNode(KeyValuePair<string, UInt64> sourceNode)
        {
            magApi mag = new magApi();
            ArrayList attr =new ArrayList();
            SortedSet<KeyValuePair<string, UInt64>> nodeList = new SortedSet<KeyValuePair<string, UInt64>>(new SortedSetComparer());
            switch (sourceNode.Key)
            {
                case "Id":
                    {
                        StringBuilder str = new StringBuilder("Id=");
                        str.Append(sourceNode.Value.ToString());
                        Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id,F.FId,AA.AuId,RId,J.JId,C.CId");
                        attr = ((ArrayList)dataJson["histograms"]);
                        break;
                    }
                case "AA.AuId":
                    {
                        StringBuilder str = new StringBuilder("Composite(AA.AuId=");
                        str.Append(sourceNode.Value.ToString());
                        str.Append(')');
                        Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id,AA.AfId");
                        attr = ((ArrayList)dataJson["histograms"]);
                        break;
                    }
                case "AA.AfId":
                    {
                        StringBuilder str = new StringBuilder("Composite(AA.AfId=");
                        str.Append(sourceNode.Value.ToString());
                        str.Append(')');
                        Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "AA.AuId");
                        attr = ((ArrayList)dataJson["histograms"]);
                        break;
                    }
                case "C.CId":
                    {
                        StringBuilder str = new StringBuilder("Composite(C.CId=");
                        str.Append(sourceNode.Value.ToString());
                        str.Append(')');
                        Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id");
                        attr = ((ArrayList)dataJson["histograms"]);
                        break;
                    }
                case "F.FId":
                    {
                        StringBuilder str = new StringBuilder("Composite(F.FId=");
                        str.Append(sourceNode.Value.ToString());
                        str.Append(')');
                        Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id");
                        attr = ((ArrayList)dataJson["histograms"]);
                        break;
                    }
                case "J.JId":
                    {
                        StringBuilder str = new StringBuilder("Composite(J.JId=");
                        str.Append(sourceNode.Value.ToString());
                        str.Append(')');
                        Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id");
                        attr = ((ArrayList)dataJson["histograms"]);
                        break;
                    }
            }
            //构造1-hop node列表
            foreach (Dictionary<string, object> s in attr)
            {
                foreach (Dictionary<string, object> h in (ArrayList)s["histogram"])
                {
                    UInt64 idValue;
                    try
                    {
                        idValue = (UInt64)h["value"];
                    }
                    catch
                    {
                        try
                        {
                            idValue = (UInt64)(long)h["value"];
                        }
                        catch
                        {
                            idValue = (UInt64)(int)h["value"];
                        }
                    }
                    nodeList.Add(new KeyValuePair<string, UInt64>((string)s["attribute"], idValue));
                }
            }
            return nodeList;
        }
    }
}
