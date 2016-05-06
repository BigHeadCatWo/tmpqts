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
        private magApi mag = new magApi();
        /// <summary>
        /// 获取1-hop节点
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <returns></returns>
        //public SortedSet<KeyValuePair<string, UInt64>> getNode(SortedSet<KeyValuePair<string, UInt64>> srcNodeSet)
        //{
        //    SortedSet<KeyValuePair<string, UInt64>> nodeList = new SortedSet<KeyValuePair<string, UInt64>>(new SortedSetComparer());
        //    StringBuilder str = new StringBuilder("Composite(AA.AuId=");
        //    str.Append(sourceNode.Value.ToString());
        //    str.Append(')');
        //    Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id,AA.AfId");
        //    attr = ((ArrayList)dataJson["histograms"]);
        //    return nodeList;
        //}
        /// <summary>
        /// 获取1-hop节点
        /// </summary>
        /// <param name="sourceNode"></param>
        /// <returns></returns>
        public SortedSet<KeyValuePair<string, UInt64>> getNode(KeyValuePair<string, UInt64> sourceNode)
        {
            ArrayList attr =new ArrayList();
            SortedSet<KeyValuePair<string, UInt64>> nodeList = new SortedSet<KeyValuePair<string, UInt64>>(new SortedSetComparer());
            switch (sourceNode.Key)
            {
                case "Id":
                    {
                        StringBuilder str = new StringBuilder("Id=");
                        str.Append(sourceNode.Value.ToString());
                        Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "F.FId,AA.AuId,RId,J.JId,C.CId");
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
                    string key = (string)s["attribute"];
                    if (key == "RId")
                        key = "Id";
                    nodeList.Add(new KeyValuePair<string, UInt64>(key, idValue));
                }
            }
            return nodeList;
        }
        public SortedSet<KeyValuePair<string, UInt64>> getNodeWithCondition(SortedSet<KeyValuePair<string, UInt64>> hop1Set,KeyValuePair<string, UInt64> dstNode)
        {
            magApi mag = new magApi();
            ArrayList attr = new ArrayList();
            SortedSet<KeyValuePair<string, UInt64>> nodeList = new SortedSet<KeyValuePair<string, UInt64>>(new SortedSetComparer());
            foreach (KeyValuePair<string, UInt64> sourceNode in hop1Set)
            {
                switch (sourceNode.Key)
                {

                    case "Id":
                        {
                            StringBuilder str = new StringBuilder("And(Id=");
                            str.Append(sourceNode.Value.ToString());
                            if (dstNode.Key.Equals("Id"))
                            {
                                str.Append(",RId" + "=" + dstNode.Value + ")");
                                Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id");
                                attr = ((ArrayList)dataJson["histograms"]);
                            }
                            else if(!dstNode.Key.Equals("AA.AfId"))
                            {
                                str.Append(",Composite(" + dstNode.Key + "=" + dstNode.Value + "))");
                                Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id");
                                attr = ((ArrayList)dataJson["histograms"]);
                            }
                            
                            break;
                        }
                    case "AA.AuId":
                        {
                            StringBuilder str = new StringBuilder("And(Composite(AA.AuId=");
                            str.Append(sourceNode.Value.ToString());
                            if (dstNode.Key.Equals("Id"))
                            {
                                str.Append("),Id" + "=" + dstNode.Value + ")");
                                Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id");
                                attr = ((ArrayList)dataJson["histograms"]);
                            }
                            if (dstNode.Key.Equals("AA.AfId"))
                            {
                                str.Append("),Composite(" +dstNode.Key+"=" + dstNode.Value + "))");
                                Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id");
                                attr = ((ArrayList)dataJson["histograms"]);
                            }
                            break;
                        }
                    case "AA.AfId":
                        {
                            StringBuilder str = new StringBuilder("And(Composite(AA.AfId=");
                            str.Append(sourceNode.Value.ToString());
                            if (dstNode.Key.Equals("AA.AuId"))
                            {
                                str.Append("),Composite(" + dstNode.Key + "=" + dstNode.Value + "))");
                                Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id");
                                attr = ((ArrayList)dataJson["histograms"]);
                            }
                            break;
                        }
                    case "C.CId":
                        {
                            StringBuilder str = new StringBuilder("And(Composite(C.CId=");
                            str.Append(sourceNode.Value.ToString());
                            if (dstNode.Key.Equals("Id"))
                            {
                                str.Append(")," + dstNode.Key + "=" + dstNode.Value + ")");
                                Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id");
                                attr = ((ArrayList)dataJson["histograms"]);
                            }
                            break;
                        }
                    case "F.FId":
                        {
                            StringBuilder str = new StringBuilder("And(Composite(F.FId=");
                            str.Append(sourceNode.Value.ToString());
                            if (dstNode.Key.Equals("Id"))
                            {
                                str.Append(")," + dstNode.Key + "=" + dstNode.Value + ")");
                                Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id");
                                attr = ((ArrayList)dataJson["histograms"]);
                            }
                            break;
                        }
                    case "J.JId":
                        {
                            StringBuilder str = new StringBuilder("Composite(J.JId=");
                            str.Append(sourceNode.Value.ToString());
                            if (dstNode.Key.Equals("Id"))
                            {
                                str.Append(")," + dstNode.Key + "=" + dstNode.Value + ")");
                                Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id");
                                attr = ((ArrayList)dataJson["histograms"]);
                            }
                            break;
                        }
                }
            }
           
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
                    string key = (string)s["attribute"];
                    
                    if (key == "RId")
                        key = "Id";
                    nodeList.Add(new KeyValuePair<string, UInt64>(key, idValue));
                }
            }
            return nodeList;
        }
        public SortedSet<KeyValuePair<string, UInt64>> getNodeWithConditionOr(SortedSet<KeyValuePair<string, UInt64>> hop1Set, KeyValuePair<string, UInt64> dstNode)
        {
            magApi mag = new magApi();
            ArrayList attr = new ArrayList();
            SortedSet<KeyValuePair<string, UInt64>> nodeList = new SortedSet<KeyValuePair<string, UInt64>>(new SortedSetComparer());
            StringBuilder str = new StringBuilder("And(");
            int count = hop1Set.Count;
            KeyValuePair<string, UInt64> sourceNode;
            for (int i = 0; i < count-1; i++)
            {
                sourceNode = hop1Set.ElementAt(i);
                if (sourceNode.Key.Equals("Id"))
                {
                    str.Append("Or(Id" + "=" + sourceNode.Value + ",");
                }
                else
                {
                    str.Append("Or(Composite(" + sourceNode.Key + "=" + sourceNode.Value + "),");

                }
            }
            sourceNode = hop1Set.ElementAt(count-1);
            if (sourceNode.Key.Equals("Id"))
            {
                str.Append("Id" + "=" + sourceNode.Value );
            }
            else
            {
                str.Append("Composite(" + sourceNode.Key + "=" + sourceNode.Value + ")");

            }
            for (int i = 0; i < count - 1; i++)
            {
                str.Append(")");
            }
            if (dstNode.Key.Equals("Id"))
            {
                str.Append(",RId" + "=" + dstNode.Value+")");
            }
            else
            {
                str.Append(",Composite(" + dstNode.Key + "=" + dstNode.Value + ")");

            }
            Console.WriteLine(str.ToString());
            Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id");
            attr = ((ArrayList)dataJson["histograms"]);
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
                    string key = (string)s["attribute"];

                    if (key == "RId")
                        key = "Id";
                    nodeList.Add(new KeyValuePair<string, UInt64>(key, idValue));
                }
            }
            return nodeList;
        }
    }
}
