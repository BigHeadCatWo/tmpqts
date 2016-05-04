using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetOneHopNode;
namespace Brute_force1
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
    class Solution
    {
        /// <summary>
        /// pair比较函数，小于号
        /// </summary>
        /// <param name="o1">操作数1</param>
        /// <param name="o2">操作数2</param>
        /// <returns></returns>
        public static bool pairLess(KeyValuePair<string, UInt64> o1, KeyValuePair<string, UInt64> o2)
        {
            if (o1.Key != o2.Key)
            {
                int i = o1.Key.CompareTo(o2.Key);
                if (i == -1)
                    return true;
                else
                    return false;
            }
            else
                return o1.Value < o2.Value;
        }
        /// <summary>
        /// pair比较函数，大于号
        /// </summary>
        /// <param name="o1">操作数1</param>
        /// <param name="o2">操作数2</param>
        /// <returns></returns>
        public static bool pairLarge(KeyValuePair<string, UInt64> o1, KeyValuePair<string, UInt64> o2)
        {
            if (o1.Key != o2.Key)
            {
                int i = o1.Key.CompareTo(o2.Key);
                if (i == 1)
                    return true;
                else
                    return false;
            }
            else
                return o1.Value > o2.Value;
        }

        /// <summary>
        /// 通过节点id查询节点的一跳节点
        /// </summary>
        /// <param name="nodeid">需要查询的id值</param>
        /// <returns>返回id的一跳节点集合，该集合已经排序</returns>
        public static SortedSet<KeyValuePair<string, UInt64>> GetOneHopNode(KeyValuePair<string, UInt64> nodeid)
        {
            //通过id获取one-hop节点集合
            SortedSet<KeyValuePair<string, UInt64>> retval = new SortedSet<KeyValuePair<string, UInt64>>();
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
        /// <summary>
        /// 计算三跳以内的路径，这里没有设计返回值，因为不知道具体需要如何返回
        /// </summary>
        /// <param name="node1">节点1</param>
        /// <param name="node2">节点2</param>
        public static void solve(KeyValuePair<string, UInt64> node1, KeyValuePair<string, UInt64> node2)
        {
            SortedSet<KeyValuePair<string, UInt64>> node1set = GetOneHopNode(node1);
            SortedSet<KeyValuePair<string, UInt64>> node2set = GetOneHopNode(node2);
            if (node1set.Contains<KeyValuePair<string, UInt64>>(node2) == true)
            {
                //存在one-hop
                Console.WriteLine("[{0},{1}]", node1, node2);
                //移除头尾节点，防止回环
                node1set.Remove(node2);
                node2set.Remove(node1);
            }
            else
            {
                //没有one-hop返回
            }
            int i = 0, j = 0;
            while (i < node1set.Count<KeyValuePair<string, UInt64>>() && j < node2set.Count<KeyValuePair<string, UInt64>>())
            {
                if (pairLess(node1set.ElementAt<KeyValuePair<string, UInt64>>(i), node1set.ElementAt<KeyValuePair<string, UInt64>>(j)))
                {
                    i++;
                }
                else if (pairLarge(node1set.ElementAt<KeyValuePair<string, UInt64>>(i), node1set.ElementAt<KeyValuePair<string, UInt64>>(j)))
                {
                    j++;
                }
                else
                {
                    //有相同
                    //输出two-hop路径，这里可以进行json封装返回
                    Console.WriteLine("[{0},{1},{2}]", node1, node1set.ElementAt<KeyValuePair<string, UInt64>>(i), node2);
                }
            }
            //这里可以区分一下哪一个的数量更小，用来减少查询的次数
            foreach (KeyValuePair<string, UInt64> nodeid in node1set)
            {
                SortedSet<KeyValuePair<string, UInt64>> nodeidset = GetOneHopNode(nodeid);
                i = 0;
                j = 0;
                while (i < nodeidset.Count<KeyValuePair<string, UInt64>>() && j < node2set.Count<KeyValuePair<string, UInt64>>())
                {
                    if (pairLess(nodeidset.ElementAt<KeyValuePair<string, UInt64>>(i), node2set.ElementAt<KeyValuePair<string, UInt64>>(j)))
                    {
                        i++;
                    }
                    else if (pairLarge(nodeidset.ElementAt<KeyValuePair<string, UInt64>>(i), node2set.ElementAt<KeyValuePair<string, UInt64>>(j)))
                    {
                        j++;
                    }
                    else
                    {
                        ///输出三跳的路径
                        Console.WriteLine("[{0},{1},{2},{3}]", node1, nodeid, nodeidset.ElementAt<KeyValuePair<string, UInt64>>(i), node2);
                    }
                }
            }
        }
    }
}
