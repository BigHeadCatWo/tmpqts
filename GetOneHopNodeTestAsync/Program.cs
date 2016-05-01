using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetOneHopNode;

namespace GetOneHopNodeTestAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            GetOneHopNodeClass nodeSearch = new GetOneHopNodeClass();
            List<Task> taskList = new List<Task>();
            taskList.Add(new Task(() =>
            {
                SortedSet<KeyValuePair<string, UInt64>> nodeList1 = nodeSearch.getNode(new KeyValuePair<string, ulong>("Id", 2274896837));
                Console.WriteLine("nodeList1.end");
            }));
            taskList.Add(new Task(() =>
            {
                SortedSet<KeyValuePair<string, UInt64>> nodeList2 = nodeSearch.getNode(new KeyValuePair<string, ulong>("F.FId", 55004796));
                Console.WriteLine("nodeList2.end");
            }));
            taskList.Add(new Task(() =>
            {
                SortedSet<KeyValuePair<string, UInt64>> nodeList3 = nodeSearch.getNode(new KeyValuePair<string, ulong>("AA.AfId", 204722609));
                Console.WriteLine("nodeList3.end");
            }));
            taskList.Add(new Task(() =>
            {
                SortedSet<KeyValuePair<string, UInt64>> nodeList4 = nodeSearch.getNode(new KeyValuePair<string, ulong>("AA.AuId", 2100880362));
                Console.WriteLine("nodeList4.end");
            }));
            taskList.Add(new Task(() =>
            {
                SortedSet<KeyValuePair<string, UInt64>> nodeList5 = nodeSearch.getNode(new KeyValuePair<string, ulong>("C.CId", 1123349196));
                Console.WriteLine("nodeList5.end");
            }));
            taskList.Add(new Task(() =>
            {
                SortedSet<KeyValuePair<string, UInt64>> nodeList6 = nodeSearch.getNode(new KeyValuePair<string, ulong>("C.CId", 1123349196));
                Console.WriteLine("nodeList6.end");
            }));
            int i = 1;
            foreach(Task t in taskList)
            {
                t.Start();
                Console.WriteLine("nodeList" + i.ToString() + "start");
            }
            Task.WaitAll(taskList.ToArray());
        }
    }
}
