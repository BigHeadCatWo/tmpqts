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
            Console.SetOut(sw);
            algorithm2byWang test = new algorithm2byWang();
            KeyValuePair<string, UInt64> node1;
            KeyValuePair<string, UInt64> node2;
            node1 = new KeyValuePair<string, UInt64>("Id", 2126125555);
            node2 = new KeyValuePair<string, UInt64>("Id", 2060367530);
            test.solve(node1, node2);
            sw.Flush();
            sw.Close();
        }
    }
    public class algorithm2byWang
    {
        public List<List<UInt64>> solve(KeyValuePair<string,UInt64> srcNode,KeyValuePair<string,UInt64> dstNode)
        {
            List<List<UInt64>> pathList = new List<List<ulong>>();
            //
            SortedSet<KeyValuePair<string, UInt64>> NextNodeOfSrc=null;
            ArrayList nextNodeAttrOfSrcAuid = null;
            Task getNextNodeOfSrc = new Task(() => 
            {
                for(int i=0;i<10;i++)
                    try
                    {
                        GetOneHopNodeClass getOneHopNode = new GetOneHopNodeClass();
                        NextNodeOfSrc = getOneHopNode.getNextNode(srcNode,ref nextNodeAttrOfSrcAuid);
                        break;
                    }
                    catch(Exception ex)
                    {
                        ;
                    }
            });
            //
            SortedSet<KeyValuePair<string, UInt64>> LastNodeOfdst=null;
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
                Console.WriteLine("1-hop存在:\n{0}-{1}", srcNode, dstNode);
                pathList.Add(new List<ulong>() { srcNode.Value, dstNode.Value });
            }
            else
                Console.WriteLine("1-hup不存在");
            #endregion
            #region search for 2-hop
            Task searchOf2hop = new Task(()=>
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
                    Console.WriteLine("2-hop存在");
                    foreach (var s in instSet)
                    {
                        Console.WriteLine("{0}-{1}-{2}", srcNode, s, dstNode);
                        pathList.Add(new List<ulong>() { srcNode.Value, s.Value, dstNode.Value });
                    }
                }
                else
                {
                    Console.WriteLine("2-hop不存在");
                }
            });
            #endregion
            searchOf2hop.Start();
            //3-hop
            #region
            List<string> str1L = new List<string>();
            List<string> str1R = new List<string>();
            List<string> str2L = new List<string>();
            List<string> str2R = new List<string>();
            List<string> str3L = new List<string>();
            List<string> str3R = new List<string>();
            List<string> str4L = new List<string>();
            List<string> str4R = new List<string>();
            HashSet<UInt64> nextNodeOfSrcHash = new HashSet<ulong>();
            HashSet<UInt64> lastNodeOfDstHash = new HashSet<ulong>();
            #endregion
            #region 遍历nextNodeOfSrc
            foreach (var s in NextNodeOfSrc)
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
                            str2L.Add("composite(F.FId=" + s.Value.ToString() + ')');
                            break;
                        }
                    case "C.CId":
                        {
                            str2L.Add("composite(C.CId=" + s.Value.ToString() + ')');
                            break;
                        }
                    case "J.JId":
                        {
                            str2L.Add("composite(J.JId=" + s.Value.ToString() + ')');
                            break;
                        }
                    case "AA.AuId":
                        {
                            str2L.Add("composite(AA.AuId=" + s.Value.ToString() + ')');
                            str3L.Add("composite(AA.AuId=" + s.Value.ToString() + ')');
                            break;
                        }
                    case "AA.AfId":
                        {
                            str4L.Add("composite(AA.AfId=" + s.Value.ToString() + ')');
                            break;
                        }
                }
            }
            #endregion
            #region 遍历LastNodeOfDst 构造hash
            Task hashOfLastNodeOfDst = new Task(() => 
            {
            getLastNodeOfdst.Wait();
                foreach (var s in LastNodeOfdst)
                {
                    lastNodeOfDstHash.Add(s.Value);
                    switch (s.Key)
                    {
                        case "Id":
                            {
                                str1R.Add("Id=" + s.Value.ToString());
                                str2R.Add("Id=" + s.Value.ToString());
                                break;
                            }
                        case "F.FId":
                            {
                                str1R.Add("composite(F.FId=" + s.Value.ToString() + ')');

                                break;
                            }
                        case "C.CId":
                            {
                                str1R.Add("composite(C.CId=" + s.Value.ToString() + ')');
                                break;
                            }
                        case "J.JId":
                            {
                                str1R.Add("composite(J.JId=" + s.Value.ToString() + ')');
                                break;
                            }
                        case "AA.AuId":
                            {
                                str1R.Add("composite(AA.AuId=" + s.Value.ToString() + ')');
                                str4R.Add("composite(AA.AuId=" + s.Value.ToString() + ')');
                                break;
                            }
                        case "AA.AfId":
                            {
                                str3R.Add("composite(AA.AfId=" + s.Value.ToString() + ')');
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
                UInt64 MaxCount = 1000000;
                string str1LT = string.Join(",or(", str1L.ToArray());
                StringBuilder str1 = new StringBuilder($"or({str1LT}");
                str1.Append(','+str1L.Last().ToString());
                for (int i = 0; i < str1L.Count(); i++)
                    str1.Append(')');
                magApi mag = new magApi();
                ArrayList attr = new ArrayList();
                Dictionary<string, object> dataJson = null;
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        dataJson = mag.GetResponse(str: str1.ToString(), count: MaxCount, attributes: "Id,RId,F.FId,C.CId,J.JId,AA.AuId");
                        break;
                    }
                    catch(Exception ex)
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
                    foreach (KeyValuePair<string,object> h in s)
                    {
                        if (h.Key == "logprob"||h.Key=="Id") continue;
                        if(h.Key=="RId")
                        {
                            foreach (object t in (ArrayList)h.Value)
                            {
                                if (lastNodeOfDstHash.Contains(Convert.ToUInt64(t)))
                                {
                                    pathList.Add(new List<ulong>() { srcNode.Value, selectedId, Convert.ToUInt64(t), dstNode.Value });
                                    Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode, new KeyValuePair<string, UInt64>("Id",selectedId),new KeyValuePair<string,UInt64>("Id",Convert.ToUInt64(t)), dstNode);
                                }
                            }
                            continue;
                        }
                        if (h.Key == "J"||h.Key=="C")
                        {
                            foreach (KeyValuePair<string, object> t in (Dictionary<string, object>)h.Value)
                            {
                                if (lastNodeOfDstHash.Contains(Convert.ToUInt64(t.Value)))
                                {
                                    pathList.Add(new List<ulong>() { srcNode.Value, selectedId, Convert.ToUInt64(t.Value), dstNode.Value });
                                    Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode,new KeyValuePair<string, UInt64>("Id",selectedId), t, dstNode);
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
                                    pathList.Add(new List<ulong>() { srcNode.Value, selectedId, Convert.ToUInt64(t.Value), dstNode.Value });
                                    Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode,new KeyValuePair<string, UInt64>("Id",selectedId), t, dstNode);
                                }
                            }
                        }
                    }
                }
                #endregion
            });
            #endregion
            #region  threeHop_task_2 F,C,J,A->Id
            Task threeHop_str2 = new Task(() => 
            {
                getLastNodeOfdst.Wait();
                foreach(Dictionary<string,object> s in LastNodeAttrOfDst)
                {
                    UInt64 selectedId = Convert.ToUInt64((s["Id"]));
                    foreach (KeyValuePair<string, object> h in s)
                    {
                        if (h.Key == "logprob" || h.Key == "Id") continue;
                        if (h.Key == "J" || h.Key == "C")
                        {
                            foreach (KeyValuePair<string, object> t in (Dictionary<string, object>)h.Value)
                            {
                                if (lastNodeOfDstHash.Contains(Convert.ToUInt64(t.Value)))
                                {
                                    pathList.Add(new List<ulong>() { srcNode.Value, Convert.ToUInt64(t.Value), selectedId, dstNode.Value });
                                    Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode,new KeyValuePair<string, UInt64>("Id",selectedId), t, dstNode);
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
                                    pathList.Add(new List<ulong>() { srcNode.Value, Convert.ToUInt64(t.Value), selectedId, dstNode.Value });
                                    Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode,new KeyValuePair<string, UInt64>("Id",selectedId), (t), dstNode);
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
                                    pathList.Add(new List<ulong>() { srcNode.Value, selectedId, Convert.ToUInt64(t), dstNode.Value });
                                    Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode,new KeyValuePair<string, UInt64>("Id",selectedId), new KeyValuePair<string, UInt64>("Id", Convert.ToUInt64(t)), dstNode);
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
                                    pathList.Add(new List<ulong>() { srcNode.Value, selectedId, Convert.ToUInt64(t.Value), dstNode.Value });
                                    Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode,new KeyValuePair<string, UInt64>("Id",selectedId), t, dstNode);
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
                                    pathList.Add(new List<ulong>() { srcNode.Value, selectedId, Convert.ToUInt64(t.Value), dstNode.Value });
                                    Console.WriteLine("3-hop:{0}-{1}-{2}-{3}", srcNode,new KeyValuePair<string, UInt64>("Id",selectedId), (t), dstNode);
                                }
                            }
                        }
                    }
                }
            });
            #endregion
            //3-hop
            if (srcNode.Key == "Id")
                threeHop_str1.Start();
            else
                threeHop_str1_scrAuid.Start();
            threeHop_str2.Start();
            if (srcNode.Key == "Id")
                threeHop_str1.Wait();
            else
                threeHop_str1_scrAuid.Wait();
            threeHop_str2.Wait();
            searchOf2hop.Wait();
            return pathList;
        }
    }
}
