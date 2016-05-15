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
        public class SortedSetComparer : IComparer<KeyValuePair<string, UInt64>>
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
        public SortedSet<KeyValuePair<string, UInt64>> getLastNode(KeyValuePair<string, UInt64> sourceNode, ref ArrayList LastNodeAttrOfDst)
        {
            ulong MaxCount = 1000000;
            ArrayList attr;
            switch (sourceNode.Key)
            {
                case "Id":
                    {
                        SortedSet<KeyValuePair<string, UInt64>> nodeList = new SortedSet<KeyValuePair<string, ulong>>(new SortedSetComparer());
                        ArrayList LastNodeAttrOfDstTemp = null;
                        Task getLastNodeForId_1 = new Task(() =>
                        {
                            ArrayList attrT;
                            magApi mag = new magApi();
                            StringBuilder str = new StringBuilder("RId=");
                            str.Append(sourceNode.Value.ToString());
                            Dictionary<string, Object> dataJson = mag.GetResponse(str: str.ToString(), count: MaxCount, attributes: "Id,F.FId,C.CId,J.JId,AA.AuId");
                            attrT = ((ArrayList)dataJson["entities"]);
                            LastNodeAttrOfDstTemp = attrT;
                            foreach (Dictionary<string, object> s in attrT)
                            {
                                lock (nodeList)
                                nodeList.Add(new KeyValuePair<string, UInt64>("Id", Convert.ToUInt64(s["Id"])));
                            }
                        });
                        Task getLastNodeForId_2 = new Task(() =>
                        {
                            magApi mag = new magApi();
                            ArrayList attrT;
                            StringBuilder str = new StringBuilder("Id=" + sourceNode.Value.ToString());
                            Dictionary<string, Object> dataJson = mag.GetResponse(str: str.ToString(), count: MaxCount, attributes: "F.FId,AA.AuId,J.JId,C.CId");
                            attrT = ((ArrayList)dataJson["entities"]);
                            SortedSet<KeyValuePair<string, UInt64>> nodeListTemp = convertJsonDataToList(attrT);
                            foreach (var s in nodeListTemp)
                            {
                                lock (nodeList)
                                nodeList.Add(s);
                            }
                        });
                        getLastNodeForId_1.Start();
                        getLastNodeForId_2.Start();
                        getLastNodeForId_2.Wait();
                        getLastNodeForId_1.Wait();
                        LastNodeAttrOfDst = LastNodeAttrOfDstTemp;
                        return nodeList;
                    }
                case "AA.AuId":
                    {
                        StringBuilder str = new StringBuilder("composite(AA.AuId=");
                        str.Append(sourceNode.Value.ToString());
                        str.Append(')');
                        Dictionary<string, Object> dataJson = mag.GetResponse(str: str.ToString(), count: MaxCount, attributes: "Id,AA.AuId,AA.AfId,C.CId,F.FId,J.JId");
                        attr = ((ArrayList)dataJson["entities"]);
                        LastNodeAttrOfDst = attr;
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
                                if (h.Key == "AA")
                                {
                                    foreach (Dictionary<string, object> h1 in (ArrayList)h.Value)
                                    {
                                        if (Convert.ToUInt64(h1["AuId"]) == sourceNode.Value)
                                        {
                                            try
                                            {
                                                nodeList.Add(new KeyValuePair<string, ulong>("AA.AfId", Convert.ToUInt64(h1["AfId"])));
                                            }
                                            catch
                                            { }

                                        }
                                    }
                                }
                            }
                        }

                        return nodeList;
                    }
            }
            return null;
        }
        public SortedSet<KeyValuePair<string, UInt64>> getNextNode(KeyValuePair<string, UInt64> sourceNode, ref ArrayList nextNodeAttrOfSrcAuid)
        {
            ulong MaxCount = 1000000;
            ArrayList attr = new ArrayList();
            SortedSet<KeyValuePair<string, UInt64>> nodeList = null;
            switch (sourceNode.Key)
            {
                case "Id":
                    {
                        StringBuilder str = new StringBuilder("Id=");
                        str.Append(sourceNode.Value.ToString());

                        Dictionary<string, Object> dataJson = mag.GetResponse(str: str.ToString(), count: MaxCount, attributes: "F.FId,AA.AuId,RId,J.JId,C.CId");
                        attr = ((ArrayList)dataJson["entities"]);
                        break;
                    }
                case "AA.AuId":
                    {
                        StringBuilder str = new StringBuilder("Composite(AA.AuId=");
                        str.Append(sourceNode.Value.ToString());
                        str.Append(')');
                        Dictionary<string, Object> dataJson = mag.GetResponse(str: str.ToString(), count: MaxCount, attributes: "Id,RId,F.FId,C.CId,J.JId,AA.AuId,AA.AfId");
                        attr = ((ArrayList)dataJson["entities"]);
                        nextNodeAttrOfSrcAuid = attr;
                        nodeList = new SortedSet<KeyValuePair<string, ulong>>(new SortedSetComparer());

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
                                if (h.Key == "AA")
                                {
                                    foreach (Dictionary<string, object> h1 in (ArrayList)h.Value)
                                    {
                                        try
                                        {
                                            if (Convert.ToUInt64(h1["AuId"]) == sourceNode.Value)
                                                nodeList.Add(new KeyValuePair<string, ulong>("AA.AfId", Convert.ToUInt64(h1["AfId"])));
                                        }
                                        catch
                                        { }
                                    }
                                    continue;
                                }
                            }
                        }

                        return nodeList;
                    }
            }
            //构造1-hop node列表

            //Console.WriteLine("hehe");
            //long start = DateTime.Now.Ticks;
            nodeList = convertJsonDataToList(attr);
            //long end = DateTime.Now.Ticks;
            //Console.WriteLine("cost:{0}", (end - start) / 100000000);
            return nodeList;
        }
        private SortedSet<KeyValuePair<string, UInt64>> convertJsonDataToList(ArrayList attr)
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
                    if (h.Key == "C")
                    {
                        foreach (KeyValuePair<string, object> t in (Dictionary<string, object>)h.Value)
                        {
                            nodeList.Add(new KeyValuePair<string, ulong>("C." + t.Key, Convert.ToUInt64(t.Value)));
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
