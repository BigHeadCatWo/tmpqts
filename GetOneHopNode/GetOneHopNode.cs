using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using magApiCs;
using System.Collections;
using System.IO;

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
        public SortedSet<KeyValuePair<string, UInt64>> getLastNode(KeyValuePair<string, UInt64> sourceNode)
        {
            ulong MaxCount = 1000;
            ArrayList attr = new ArrayList();
            SortedSet<KeyValuePair<string, UInt64>> nodeList = new SortedSet<KeyValuePair<string, UInt64>>(new SortedSetComparer());
            switch (sourceNode.Key)
            {
                case "Id":
                    {
                        StringBuilder str = new StringBuilder("Id=");
                        str.Append(sourceNode.Value.ToString());
                        Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: MaxCount, attributes: "F.FId,AA.AuId,J.JId,C.CId");
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
                        str = new StringBuilder("RId=");
                        str.Append(sourceNode.Value.ToString());
                        dataJson = mag.GetResponse(str: str.ToString(), count: MaxCount, attributes: "Id");
                        attr = ((ArrayList)dataJson["histograms"]);
                        break;
                    }
                case "AA.AuId":
                    {
                        StringBuilder str = new StringBuilder("Composite(AA.AuId=");
                        str.Append(sourceNode.Value.ToString());
                        str.Append(')');
                        Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: MaxCount, attributes: "Id,AA.AfId");
                        attr = ((ArrayList)dataJson["histograms"]);
                        break;
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
        public SortedSet<KeyValuePair<string, UInt64>> getNode(KeyValuePair<string, UInt64> sourceNode)
        {
            ulong maxCount = 1000;
            ArrayList attr =new ArrayList();
            switch (sourceNode.Key)
            {
                case "Id":
                    {
                        StringBuilder str = new StringBuilder("Id=");
                        str.Append(sourceNode.Value.ToString());

                        Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "F.FId,AA.AuId,RId,J.JId,C.CId");
                        attr = ((ArrayList)dataJson["entities"]);

                        break;
                    }
                case "AA.AuId":
                    {
                        StringBuilder str = new StringBuilder("Composite(AA.AuId=");
                        str.Append(sourceNode.Value.ToString());
                        str.Append(')');

                        Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id,AA.AfId");
                        attr = ((ArrayList)dataJson["entities"]);

                        break;
                    }
                case "AA.AfId":
                    {
                        StringBuilder str = new StringBuilder("Composite(AA.AfId=");
                        str.Append(sourceNode.Value.ToString());
                        str.Append(')');

                        Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "AA.AuId");
                        attr = ((ArrayList)dataJson["entities"]);

                        break;
                    }
                case "C.CId":
                    {
                        StringBuilder str = new StringBuilder("Composite(C.CId=");
                        str.Append(sourceNode.Value.ToString());
                        str.Append(')');

                        Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id");
                        attr = ((ArrayList)dataJson["entities"]);

                        break;
                    }
                case "F.FId":
                    {
                        StringBuilder str = new StringBuilder("Composite(F.FId=");
                        str.Append(sourceNode.Value.ToString());
                        str.Append(')');

                        Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 100000, attributes: "Id");
                        attr = ((ArrayList)dataJson["entities"]);

                        break;
                    }
                case "J.JId":
                    {
                        StringBuilder str = new StringBuilder("Composite(J.JId=");
                        str.Append(sourceNode.Value.ToString());
                        str.Append(')');

                        Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id");
                        attr = ((ArrayList)dataJson["entities"]);

                        break;
                    }
            }
            //构造1-hop node列表

            Console.WriteLine("hehe");
            long start = DateTime.Now.Ticks;
            SortedSet<KeyValuePair<string, UInt64>> nodeList = convertJsonDatoToList(attr);
            long end = DateTime.Now.Ticks;
            Console.WriteLine("cost:{0}", (end - start) / 100000000);
            return nodeList;
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
                    if(h.Key=="Id")
                    {
                        nodeList.Add(new KeyValuePair<string, ulong>(h.Key, Convert.ToUInt64(h.Value)));
                        continue;
                    }
                    if (h.Key == "RId"||h.Key=="Id")
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

        public bool checkNodeWithCondition(KeyValuePair<string, UInt64> sourceNode, KeyValuePair<string, UInt64> dstNode)
        {
            magApi mag = new magApi();
            ArrayList attr = new ArrayList();
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
                            attr = ((ArrayList)dataJson["entities"]);
                        }
                        else if (!dstNode.Key.Equals("AA.AfId"))
                        {
                            str.Append(",Composite(" + dstNode.Key + "=" + dstNode.Value + "))");
                            Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: dstNode.Key);
                            attr = ((ArrayList)dataJson["entities"]);
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
                            attr = ((ArrayList)dataJson["entities"]);
                        }
                        if (dstNode.Key.Equals("AA.AfId"))
                        {
                            str.Append("),Composite(" + dstNode.Key + "=" + dstNode.Value + "))");
                            Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: dstNode.Key);
                            attr = ((ArrayList)dataJson["entities"]);
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
                            Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: dstNode.Key);
                            attr = ((ArrayList)dataJson["entities"]);
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
                            Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: dstNode.Key);
                            attr = ((ArrayList)dataJson["entities"]);
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
                            Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: dstNode.Key);
                            attr = ((ArrayList)dataJson["entities"]);
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
                            Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: dstNode.Key);
                            attr = ((ArrayList)dataJson["entities"]);
                        }
                        break;
                    }
            }
            foreach (Dictionary<string, object> s in attr)
            {
                foreach (Dictionary<string, object> h in (ArrayList)s["evaluate"])
                {
                    return true;
                }
            }
            return false;
        }
        public SortedSet<KeyValuePair<string, UInt64>> getNodeWithConditionOr(SortedSet<KeyValuePair<string, UInt64>> hop1Set, KeyValuePair<string, UInt64> dstNode)
        {
            magApi mag = new magApi();
            ArrayList attr = new ArrayList();
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
            StreamWriter sw = new StreamWriter("I:\\1.txt",true);
            sw.WriteLine(str.ToString());
            sw.Flush();
            sw.Close();
            Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id");
            attr = ((ArrayList)dataJson["entities"]);
            SortedSet<KeyValuePair<string, UInt64>> nodeList = convertJsonDatoToList(attr);
            return nodeList;
        }
        public SortedSet<KeyValuePair<string, UInt64>> getNodeWithOr(SortedSet<KeyValuePair<string, UInt64>> hop1Set)
        {
            magApi mag = new magApi();
            ArrayList attr = new ArrayList();
            StringBuilder str = new StringBuilder("");
            int count = hop1Set.Count;
            KeyValuePair<string, UInt64> sourceNode;
            for (int i = 0; i < count - 1; i++)
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
            sourceNode = hop1Set.ElementAt(count - 1);
            if (sourceNode.Key.Equals("Id"))
            {
                str.Append("Id" + "=" + sourceNode.Value);
            }
            else
            {
                str.Append("Composite(" + sourceNode.Key + "=" + sourceNode.Value + ")");

            }
            for (int i = 0; i < count - 1; i++)
            {
                str.Append(")");
            }
            
           /// Console.WriteLine(str.ToString());
            Dictionary<string, object> dataJson = mag.GetResponse(str: str.ToString(), count: 10000000, attributes: "Id");
            attr = ((ArrayList)dataJson["entities"]);
            SortedSet<KeyValuePair<string, UInt64>> nodeList = convertJsonDatoToList(attr);
            return nodeList;
        }
    }
}
