using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetOneHopNode;
using System.Collections;

namespace Brute_force1
{

    class Program
    {
        static void Main(string[] args)
        {
            KeyValuePair<string, UInt64> node1;
            KeyValuePair<string, UInt64> node2;
            //    ///一跳测试
            //    ///id--id
            node1 = new KeyValuePair<string, UInt64>("Id", 2140251882);
            node2 = new KeyValuePair<string, UInt64>("Id", 2143554828);
            Solution.solve(node1, node2);
            ///id--AA.AuId
            node1 = new KeyValuePair<string, UInt64>("Id", 2140251882);
            node2 = new KeyValuePair<string, UInt64>("AA.AuId", 2145115012);
            Solution.solve(node1, node2);
            ///AA.AuId--id
            node1 = new KeyValuePair<string, UInt64>("AA.AuId", 2145115012);
            node2 = new KeyValuePair<string, UInt64>("Id", 2140251882);
            Solution.solve(node1, node2);
            ///AA.AuId--AA.AuId
            node1 = new KeyValuePair<string, UInt64>("AA.AuId", 2145115012);
            node2 = new KeyValuePair<string, UInt64>("AA.AuId", 2145115015);
            Solution.solve(node1, node2);
            ///id to id  1970381522  to 2162351023  大家算出来多少？ 36
            ///[id, AA.AuId]=[2273736245,2094437628]有多少对啊？ 41  19
            ///大家看看这组数据有多少边，我这里出来2584条，感觉有点虚：2126125555，2153635508
            ///两跳测试
            ///id--id
            //node1 = new KeyValuePair<string, UInt64>("Id", 2126125555);
            //node2 = new KeyValuePair<string, UInt64>("Id", 2153635508);
            //Solution.solve(node1, node2);

            Console.ReadLine();
        }
    }
    class Solution
    {
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

     
        public static SortedSet<KeyValuePair<string, UInt64>> GetOneHopNode(KeyValuePair<string, UInt64> nodeid)
        {
            //通过id获取one-hop节点集合
            SortedSet<KeyValuePair<string, UInt64>> retval;
            /*
             * 这里是获取id 的过程，基本上需要两步，第一步通过id获取该id的属性，
             * 第二步通过属性获取有此属性的一跳节点id集合
             */
            /*
            *  获取1-Hop的node list
            *  by Esdreal
            */
            GetOneHopNodeClass getOneHop = new GetOneHopNodeClass();
            retval = getOneHop.getNode(nodeid);
            return retval;
        }
        public static Dictionary<KeyValuePair<string, UInt64>, SortedSet<KeyValuePair<string, UInt64>>> GetTwoHopNode(SortedSet<KeyValuePair<string, UInt64>> hop1set)
        {
            Dictionary<KeyValuePair<string, UInt64>, SortedSet<KeyValuePair<string, UInt64>>> dic = new Dictionary<KeyValuePair<string, ulong>, SortedSet<KeyValuePair<string, ulong>>>();
            //通过id获取one-hop节点集合
            long start, end;
            foreach (KeyValuePair<string, UInt64> nodeid in hop1set)
            {
                ///Console.WriteLine("find:{0}:{1}", nodeid.Key, nodeid);
                start = DateTime.Now.Ticks;
                GetOneHopNodeClass getOneHop = new GetOneHopNodeClass();
                SortedSet<KeyValuePair<string, UInt64>> tmp = getOneHop.getNode(nodeid);
                end = DateTime.Now.Ticks;
                /// Console.WriteLine("{0}:获取hop1set花费时间", (end - start) / 1000);
                dic.Add(nodeid, tmp);
            }
            return dic;
        }
        //public static SortedSet<KeyValuePair<string, UInt64>> GetTwoHopNode2(SortedSet<KeyValuePair<string, UInt64>> hop1set, KeyValuePair<string, UInt64>dst)
        //{
        //    SortedSet<KeyValuePair<string, UInt64>> res = new SortedSet<KeyValuePair<string, ulong>>(new SortedSetComparer());
        //    GetOneHopNodeClass getOneHop = new GetOneHopNodeClass();
        //    foreach (KeyValuePair<string, UInt64> node in hop1set)
        //    {
        //        SortedSet<KeyValuePair<string, UInt64>> tmp = getOneHop.getNode(node,dst);
        //        res.UnionWith(tmp);
        //    }
        //    return res;
        //}
        /// <summary>
        /// </summary>
        /// <param name="node1">节点1</param>
        /// <param name="node2">节点2</param>
        public static void solve(KeyValuePair<string, UInt64> node1, KeyValuePair<string, UInt64> node2)
        {
            long count = 0;
            long start, end;
            ///step1:获取node1和node2是否存在一跳关系
            start = DateTime.Now.Ticks;
            SortedSet<KeyValuePair<string, UInt64>> retval;
            GetOneHopNodeClass getOneHop = new GetOneHopNodeClass();
            bool oneHop = getOneHop.checkOneHop(node1,node2);
            end = DateTime.Now.Ticks;
            Console.WriteLine("时间{0}", (end - start) / 1000000);
            if (oneHop)
            {
                Console.WriteLine("{0}:存在one-hop", count++);
                Console.WriteLine("[{0},{1}]", node1, node2);
            }
            ///step2:获取两跳关系






            //start = DateTime.Now.Ticks;
            //Dictionary<KeyValuePair<string, UInt64>, SortedSet<KeyValuePair<string, UInt64>>> hop2dic = GetTwoHopNode(hop1set);
            //SortedSet<KeyValuePair<string, UInt64>> hop2set = new SortedSet<KeyValuePair<string, ulong>>(new SortedSetComparer());
            //foreach (KeyValuePair<KeyValuePair<string, UInt64>, SortedSet<KeyValuePair<string, UInt64>>> kv in hop2dic)
            //{
            //    foreach (KeyValuePair<string, UInt64> tmppair in kv.Value)
            //        hop2set.Add(tmppair);
            //    if (kv.Value.Contains(node2))
            //    {
            //        Console.WriteLine("{0}:存在two-hop", count++);
            //        Console.WriteLine("[{0},{1},{2}]", node1, kv.Key, node2);
            //    }
            //}

            //end = DateTime.Now.Ticks;
            //Console.WriteLine("时间{0}:set大小：{1}", (end - start) / 1000000, hop2set.ToList().Capacity);
            //step3:获取三跳关系
            //start = DateTime.Now.Ticks;
            //Dictionary<KeyValuePair<string, UInt64>, SortedSet<KeyValuePair<string, UInt64>>> hop3dic = GetTwoHopNode(hop2set);
            //SortedSet<KeyValuePair<string, UInt64>> hop3set = new SortedSet<KeyValuePair<string, ulong>>(new SortedSetComparer());
            //foreach (KeyValuePair<KeyValuePair<string, UInt64>, SortedSet<KeyValuePair<string, UInt64>>> kv in hop3dic)
            //{
            //    hop3set.Union(kv.Value);
            //    if (kv.Value.Contains(node2))
            //    {
            //        Console.WriteLine("{0}:存在three-hop", count++);
            //        Console.WriteLine("[{0},{1},{2}]", node1, kv.Key, node2);
            //    }

            //}
            //end = DateTime.Now.Ticks;
            //Console.WriteLine("时间{0}:set大小：{1}", (end - start) / 1000000, hop3set.ToList().Capacity);
        }
    }
}
