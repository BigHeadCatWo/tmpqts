using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using magApiCs;
using System.Collections;

namespace magApiTest
{
    /// <summary>
    /// 测试，实验用样例
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            magApi magTest = new magApi();
            Dictionary<string, object> result = magTest.GetResponse(str: "Id=2171526461");
            ArrayList attr = ((ArrayList)result["histograms"]);
            foreach (Dictionary<string,object> s in attr)
            {
                foreach(Dictionary<string,object> h in (ArrayList)s["histogram"])
                {
                    ulong i;
                    try
                    {
                        i = (ulong)h["value"];
                    }
                    catch
                    {
                        try
                        {
                            i = (ulong)(long)h["value"];
                        }
                        catch
                        {
                            i = (ulong)(int)h["value"];
                        }
                    }
                }
            }
        }
    }
}
