using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetOneHopNode;
using magApiCs;
using System.Collections;
using System.IO;

namespace algorithm2byWang
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamWriter sw = new StreamWriter(@"ConsoleOutput.txt");
            algorithm2byWang test = new algorithm2byWang();
            KeyValuePair<string, UInt64> node1;
            KeyValuePair<string, UInt64> node2;
            node1 = new KeyValuePair<string, UInt64>("Id", 2131653836);
            node2 = new KeyValuePair<string, UInt64>("Id", 2016266703);
            List<List<UInt64>> pathList = test.solve(node1, node2);
            Console.WriteLine("number of path:{0}", pathList.Count);
            Console.SetOut(sw);
            lock (pathList)
            foreach (var i in pathList)
                {
                    Console.WriteLine(string.Join(" - ", i));
                }
            sw.Flush();
            sw.Close();
        }
    }
    public class algorithm2byWang
    {
        public List<List<UInt64>> solve(KeyValuePair<string, UInt64> srcNode, KeyValuePair<string, UInt64> dstNode)
        {
            List<List<UInt64>> pathList = new List<List<ulong>>();
            //
            SortedSet<KeyValuePair<string, UInt64>> NextNodeOfSrc = null;
            ArrayList nextNodeAttrOfSrcAuid = null;
            Task getNextNodeOfSrc = new Task(() =>
            {
                for (int i = 0; i < 10; i++)
                    try
                    {
                        GetOneHopNodeClass getOneHopNode = new GetOneHopNodeClass();
                        NextNodeOfSrc = getOneHopNode.getNextNode(srcNode, ref nextNodeAttrOfSrcAuid);
                        break;
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
            });
            //
            SortedSet<KeyValuePair<string, UInt64>> LastNodeOfdst = null;
            ArrayList LastNodeAttrOfDst = null;
            Task getLastNodeOfdst = new Task(() =>
            {
                for (int i = 0; i < 10; i++)
                    try
                    {
                        GetOneHopNodeClass getOneHopNode = new GetOneHopNodeClass();
                        LastNodeOfdst = getOneHopNode.getLastNode(dstNode, ref LastNodeAttrOfDst);
                        break;
                    }
                    catch
                    {
                        ;
                    }
            });
            //
            getNextNodeOfSrc.Start();
            getLastNodeOfdst.Start();
            //search for 1-hop
            #region search for 1-hop
            getNextNodeOfSrc.Wait();
            if (NextNodeOfSrc.Contains(dstNode))
            {
                //Console.WriteLine("1-hop存在:\n{0}-{1}", srcNode, dstNode);
                lock (pathList)
                pathList.Add(new List<ulong>() { srcNode.Value, dstNode.Value });
            }
            else
            {
                //Console.WriteLine("1-hup不存在");
            }
            #endregion
            #region search for 2-hop
            Task searchOf2hop = new Task(() =>
            {
                getLastNodeOfdst.Wait();
                SortedSet<KeyValuePair<string, UInt64>> instSet = new SortedSet<KeyValuePair<string, ulong>>(new GetOneHopNodeClass.SortedSetComparer());
                foreach (var s in NextNodeOfSrc)
                {
                    instSet.Add(s);
                }
                instSet.IntersectWith(LastNodeOfdst);
                if (instSet.Count != 0)
                {
                    //Console.WriteLine("2-hop存在");
                    foreach (var s in instSet)
                    {
                        //Console.WriteLine("{0}-{1}-{2}", srcNode, s, dstNode);
                        lock (pathList)
                        pathList.Add(new List<ulong>() { srcNode.Value, s.Value, dstNode.Value });
                    }
                }
                else
                {
                    //Console.WriteLine("2-hop不存在");
                }

            });
            #endregion
            searchOf2hop.Start();
            //3-hop
            #region
            List<string> str1L = new List<string>();

            List<string> strLAuId = new List<string>();
            List<string> strLAfId = new List<string>();
            List<string> strRAuId = new List<string>();
            List<string> strRAfId = new List<string>();

            HashSet<UInt64> nextNodeOfSrcHash = new HashSet<ulong>();
            HashSet<UInt64> lastNodeOfDstHash = new HashSet<ulong>();
            #endregion
            #region 遍历nextNodeOfSrc,构造hash
            foreach (KeyValuePair<string,UInt64> s in NextNodeOfSrc)
            {
                nextNodeOfSrcHash.Add(s.Value);
                switch (s.Key)
                {
                    case "Id":
                        {
                            str1L.Add("Id=" + s.Value.ToString());
                            break;
                        }
                    case "F.FId":
                        {
                            break;
                        }
                    case "C.CId":
                        {
                            break;
                        }
                    case "J.JId":
                        {
                            break;
                        }
                    case "AA.AuId":
                        {
                            string value = s.Value.ToString();
                            strLAuId.Add("composite(AA.AuId=" + value + ')');
                            break;
                        }
                    case "AA.AfId":
                        {
                            break;
                        }
                }
            }
            #endregion
            #region 遍历LastNodeOfDst 构造hash
            Task hashOfLastNodeOfDst = new Task(() =>
            {
                getLastNodeOfdst.Wait();
                foreach (KeyValuePair<string,UInt64> s in LastNodeOfdst)
                {
                    lastNodeOfDstHash.Add(s.Value);
                    switch (s.Key)
                    {
                        case "Id":
                            {
                                break;
                            }
                        case "F.FId":
                            {

                                break;
                            }
                        case "C.CId":
                            {
                                break;
                            }
                        case "J.JId":
                            {
                                break;
                            }
                        case "AA.AuId":
                            {
                                strRAuId.Add("composite(AA.AuId=" + s.Value.ToString() + ')');
                                break;
                            }
                        case "AA.AfId":
                            {
                                strRAfId.Add("composite(AA.AfId=" + s.Value.ToString() + ')');
                                break;
                            }
                    }
                }
            });
            hashOfLastNodeOfDst.Start();
            #endregion
            #region threeHop_task_1 Id->Id,F,C,J,A (for src=id)
            Task threeHop_str1 = new Task(() =>
            {
                if (str1L.Count == 0)
                    return;
                int maxExprNum = 60;
                List<Task> taskList = new List<Task>();
                List<List<string>> taskStrList = new List<List<string>>();
                //ToDo
                for (int iPart = 0; iPart < str1L.Count; iPart += maxExprNum)
                {
                    List<string> str1Lpart = str1L.GetRange(iPart, Math.Min(maxExprNum, str1L.Count - iPart));
                    #region new task
                    Task taskPart = new Task(() =>
                    {
                        UInt64 MaxCount = 1000000;
                        string str1LT = string.Join(",or(", str1Lpart.ToArray());
                        StringBuilder str1 = new StringBuilder($"or({str1LT}");
                        str1.Append(',' + str1Lpart.Last().ToString());
                        for (int i = 0; i < str1Lpart.Count(); i++)
                            str1.Append(')');
                        //Console.WriteLine("@@@TestAsync:{0}", str1);
                        magApi mag = new magApi();
                        ArrayList attr = new ArrayList();
                        Dictionary<string, Object> dataJson = null;
                        for (int i = 0; i < 10; i++)
                        {
                            try
                            {
                                dataJson = mag.GetResponse(str: str1.ToString(), count: MaxCount, attributes: "Id,RId,F.FId,C.CId,J.JId,AA.AuId");
                                break;
                            }
                            catch (Exception ex)
                            {
                                ;
                            }
                        }
                        attr = ((ArrayList)dataJson["entities"]);
                        #region 等待hashOfLastNodeOfDst
                        hashOfLastNodeOfDst.Wait();
                        #endregion
                        #region 查找path
                        foreach (Dictionary<string, object> s in attr)
                        {
                            UInt64 selectedId = Convert.ToUInt64((s["Id"]));
                            foreach (KeyValuePair<string, object> h in s)
                            {
                                if (h.Key == "logprob" || h.Key == "Id") continue;
                                if (h.Key == "RId")
                                {
                                    foreach (object t in (ArrayList)h.Value)
                                    {
                                        if (lastNodeOfDstHash.Contains(Convert.ToUInt64(t)))
                                        {
                                            lock (pathList)
                                            pathList.Add(new List<ulong>() { srcNode.Value, selectedId, Convert.ToUInt64(t), dstNode.Value });
                                            //Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode, new KeyValuePair<string, UInt64>("Id", selectedId), new KeyValuePair<string, UInt64>("Id", Convert.ToUInt64(t)), dstNode);
                                        }
                                    }
                                    continue;
                                }
                                if (h.Key == "J" || h.Key == "C")
                                {
                                    foreach (KeyValuePair<string, object> t in (Dictionary<string, object>)h.Value)
                                    {
                                        if (lastNodeOfDstHash.Contains(Convert.ToUInt64(t.Value)))
                                        {
                                            lock (pathList)
                                            pathList.Add(new List<ulong>() { srcNode.Value, selectedId, Convert.ToUInt64(t.Value), dstNode.Value });
                                            //Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode, new KeyValuePair<string, UInt64>("Id", selectedId), t, dstNode);
                                        }
                                    }
                                    continue;
                                }
                                foreach (Dictionary<string, object> h1 in (ArrayList)h.Value)
                                {
                                    foreach (KeyValuePair<string, object> t in h1)
                                    {
                                        if (lastNodeOfDstHash.Contains(Convert.ToUInt64(t.Value)))
                                        {
                                            lock (pathList)
                                            pathList.Add(new List<ulong>() { srcNode.Value, selectedId, Convert.ToUInt64(t.Value), dstNode.Value });
                                            //Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode, new KeyValuePair<string, UInt64>("Id", selectedId), t, dstNode);
                                        }
                                    }
                                }
                            }
                        }
                    }
                        );
                    #endregion
                    taskList.Add(taskPart);
                }
                foreach (Task t in taskList)
                {
                    t.Start();
                }
                Task.WaitAll(taskList.ToArray());
                #endregion
            });
            #endregion
            #region  threeHop_task_2 F,C,J,A->Id
            Task threeHop_str2 = new Task(() =>
            {
                getLastNodeOfdst.Wait();
                foreach (Dictionary<string, object> s in LastNodeAttrOfDst)
                {
                    UInt64 selectedId = Convert.ToUInt64((s["Id"]));
                    foreach (KeyValuePair<string, object> h in s)
                    {
                        if (h.Key == "logprob" || h.Key == "Id") continue;
                        if (h.Key == "J" || h.Key == "C")
                        {
                            foreach (KeyValuePair<string, object> t in (Dictionary<string, object>)h.Value)
                            {
                                if (nextNodeOfSrcHash.Contains(Convert.ToUInt64(t.Value)))
                                {
                                    lock (pathList)
                                    pathList.Add(new List<ulong>() { srcNode.Value, Convert.ToUInt64(t.Value), selectedId, dstNode.Value });
                                    //Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode, t, new KeyValuePair<string, UInt64>("Id", selectedId), dstNode);
                                }
                            }
                            continue;
                        }

                        foreach (Dictionary<string, object> h1 in (ArrayList)h.Value)
                        {
                            foreach (KeyValuePair<string, object> t in h1)
                            {
                                if (t.Key == "AfId")
                                    continue;
                                if (nextNodeOfSrcHash.Contains(Convert.ToUInt64(t.Value)))
                                {
                                    lock (pathList)
                                    pathList.Add(new List<ulong>() { srcNode.Value, Convert.ToUInt64(t.Value), selectedId, dstNode.Value });
                                    //Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode, (t), new KeyValuePair<string, UInt64>("Id", selectedId), dstNode);
                                }
                            }
                        }
                    }
                }
            });
            #endregion
            #region threeHop_task_1 Id->Id,F,C,J,A(for src = auid)
            Task threeHop_str1_scrAuid = new Task(() =>
            {
                hashOfLastNodeOfDst.Wait();
                foreach (Dictionary<string, object> s in nextNodeAttrOfSrcAuid)
                {
                    UInt64 selectedId = Convert.ToUInt64((s["Id"]));
                    foreach (KeyValuePair<string, object> h in s)
                    {
                        if (h.Key == "logprob" || h.Key == "Id") continue;
                        if (h.Key == "RId")
                        {
                            foreach (object t in (ArrayList)h.Value)
                            {
                                if (lastNodeOfDstHash.Contains(Convert.ToUInt64(t)))
                                {
                                    lock (pathList)
                                    pathList.Add(new List<ulong>() { srcNode.Value, selectedId, Convert.ToUInt64(t), dstNode.Value });
                                    //Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode, new KeyValuePair<string, UInt64>("Id", selectedId), new KeyValuePair<string, UInt64>("Id", Convert.ToUInt64(t)), dstNode);
                                }
                            }
                            continue;
                        }
                        if (h.Key == "J" || h.Key == "C")
                        {
                            foreach (KeyValuePair<string, object> t in (Dictionary<string, object>)h.Value)
                            {
                                if (lastNodeOfDstHash.Contains(Convert.ToUInt64(t.Value)))
                                {
                                    lock (pathList)
                                    pathList.Add(new List<ulong>() { srcNode.Value, selectedId, Convert.ToUInt64(t.Value), dstNode.Value });
                                    //Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode, new KeyValuePair<string, UInt64>("Id", selectedId), t, dstNode);
                                }
                            }
                            continue;
                        }

                        foreach (Dictionary<string, object> h1 in (ArrayList)h.Value)
                        {
                            foreach (KeyValuePair<string, object> t in h1)
                            {
                                if (t.Key == "AfId")
                                    continue;
                                if (lastNodeOfDstHash.Contains(Convert.ToUInt64(t.Value)))
                                {
                                    lock (pathList)
                                    pathList.Add(new List<ulong>() { srcNode.Value, selectedId, Convert.ToUInt64(t.Value), dstNode.Value });
                                    //Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode, new KeyValuePair<string, UInt64>("Id", selectedId), (t), dstNode);
                                }
                            }
                        }
                    }
                }
            });
            #endregion
            #region id-auid-afid-auid
            Task threeHop_auidAfidForIdSrc = new Task(() =>
            {
                if (strLAuId.Count == 0)
                    return;
                hashOfLastNodeOfDst.Wait();
                UInt64 MaxCount = 1000000;
                StringBuilder strL = new StringBuilder("or(" + string.Join(",or(", strLAuId.ToArray()));
                strL.Append(',' + strLAuId.Last().ToString());
                for (int i = 0; i < strLAuId.Count(); i++)
                    strL.Append(')');
                StringBuilder strR = new StringBuilder("or(" + string.Join(",or(", strRAfId.ToArray()));
                strR.Append(',' + strRAfId.Last().ToString());
                for (int i = 0; i < strRAfId.Count(); i++)
                    strR.Append(')');
                StringBuilder str = new StringBuilder($"and({strL.ToString()},{strR.ToString()})");
                magApi mag = new magApi();
                ArrayList attr = new ArrayList();
                Dictionary<string, Object> dataJson = null;
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        dataJson = mag.GetResponse(str: str.ToString(), count: MaxCount, attributes: "AA.AfId,AA.AuId");
                        break;
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                }
                attr = ((ArrayList)dataJson["entities"]);
                foreach (Dictionary<string, object> s in attr)
                {
                    foreach (KeyValuePair<string, object> h in s)
                    {
                        if (h.Key == "AA")
                            foreach (Dictionary<string, object> h1 in (ArrayList)h.Value)
                            {
                                try
                                {
                                    if (nextNodeOfSrcHash.Contains(Convert.ToUInt64(h1["AuId"])) && lastNodeOfDstHash.Contains(Convert.ToUInt64(h1["AfId"])))
                                    {
                                        lock (pathList)
                                        pathList.Add(new List<ulong>() { srcNode.Value, Convert.ToUInt64(h1["AuId"]), Convert.ToUInt64(h1["AfId"]), dstNode.Value });
                                        ////Console.WriteLine("auidAfId");
                                        //Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode, new KeyValuePair<string, UInt64>("AuId", Convert.ToUInt64(h1["AuId"])), new KeyValuePair<string, UInt64>("AfId", Convert.ToUInt64(h1["AfId"])), dstNode);
                                    }
                                }
                                catch
                                { }
                            }
                    }
                }
            });
            #endregion
            #region auid-afid-auid-id
            Task threeHop_afidAuidForIdAuid = new Task(() =>
            {
                hashOfLastNodeOfDst.Wait();
                if (strLAfId.Count == 0)
                    return;
                UInt64 MaxCount = 1000000;
                StringBuilder strL = new StringBuilder("or(" + string.Join(",or(", strLAfId.ToArray()));
                strL.Append(',' + strLAfId.Last().ToString());
                for (int i = 0; i < strLAfId.Count(); i++)
                    strL.Append(')');
                StringBuilder strR = new StringBuilder("or(" + string.Join(",or(", strRAuId.ToArray()));
                strR.Append(',' + strRAuId.Last().ToString());
                for (int i = 0; i < strRAuId.Count(); i++)
                    strR.Append(')');
                StringBuilder str = new StringBuilder($"and({strL.ToString()},{strR.ToString()})");
                magApi mag = new magApi();
                ArrayList attr = new ArrayList();
                Dictionary<string, Object> dataJson = null;
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        dataJson = mag.GetResponse(str: str.ToString(), count: MaxCount, attributes: "AA.AfId,AA.AuId");
                        break;
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                }
                attr = ((ArrayList)dataJson["entities"]);
                foreach (Dictionary<string, object> s in attr)
                {
                    foreach (KeyValuePair<string, object> h in s)
                    {
                        if (h.Key == "AA")
                            foreach (Dictionary<string, object> h1 in (ArrayList)h.Value)
                            {
                                try
                                {
                                    if (nextNodeOfSrcHash.Contains(Convert.ToUInt64(h1["AfId"])) && lastNodeOfDstHash.Contains(Convert.ToUInt64(h1["AuId"])))
                                    {
                                        lock (pathList)
                                        pathList.Add(new List<ulong>() { srcNode.Value, Convert.ToUInt64(h1["AfId"]), Convert.ToUInt64(h1["AuId"]), dstNode.Value });
                                        ////Console.WriteLine("auidAfId");
                                        //Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode, new KeyValuePair<string, UInt64>("AfId", Convert.ToUInt64(h1["AfId"])), new KeyValuePair<string, UInt64>("AuId", Convert.ToUInt64(h1["AuId"])), dstNode);
                                    }
                                }
                                catch
                                { }
                            }
                    }
                }
            });
            #endregion
            //3-hop
            if (srcNode.Key == "Id")
            {
                threeHop_str1.Start();
            }
            else
            {
                threeHop_str1_scrAuid.Start();
            }
            if (srcNode.Key == "Id" && dstNode.Key == "AA.AuId")
            {
                threeHop_auidAfidForIdSrc.Start();
            }
            if (srcNode.Key == "AA.AuId" && dstNode.Key == "Id")
            {
                threeHop_afidAuidForIdAuid.Start();
            }
            threeHop_str2.Start();
            if (srcNode.Key == "Id")
            {
                threeHop_str1.Wait();
            }
            else
            {
                threeHop_str1_scrAuid.Wait();
            }
            if (srcNode.Key == "Id" && dstNode.Key == "AA.AuId")
            {
                threeHop_auidAfidForIdSrc.Wait();
            }
            if (srcNode.Key == "AA.AuId" && dstNode.Key == "Id")
            {
                threeHop_afidAuidForIdAuid.Wait();
            }
            threeHop_str2.Wait();
            searchOf2hop.Wait();
            Task.WaitAll();
            HashSet<List<UInt64>> hashPathList = new HashSet<List<ulong>>(pathList, new SortedSetComparer());
            return hashPathList.ToList();
        }
        public class SortedSetComparer : IEqualityComparer<List<UInt64>>
        {
            public bool Equals(List<UInt64> x, List<UInt64> y)
            {
                if (x.Count != y.Count)
                    return false;
                for (int i = 0; i < x.Count; i++)
                {
                    if (!x[i].Equals(y[i]))
                        return false;
                }
                return true;
            }
            public int GetHashCode(List<UInt64> x)
            {
                int hash = 19;
                foreach (UInt64 i in x)
                    hash = hash * 31 + i.GetHashCode();
                return hash;
            }
        }
    }
}
