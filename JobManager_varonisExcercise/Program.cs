using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobManager_varonisExcercise
{
    internal class Program
    {
        static void Main(string[] args)
        {
            JobManager jobMan = new JobManager();
            jobMan.Init(new List<KeyValuePair<int, int>>() {
                new KeyValuePair<int, int>(2,1),
                new KeyValuePair<int, int>(3,1),
                new KeyValuePair<int, int>(4,2),
                new KeyValuePair<int, int>(4,3),
                new KeyValuePair<int, int>(8,4),
                new KeyValuePair<int, int>(9,8),
                new KeyValuePair<int, int>(10,8),
                new KeyValuePair<int, int>(7,5),
                new KeyValuePair<int, int>(7,6)
            });
            
            foreach(int item in jobMan.GetNextAvailableJobs())
            {
                Console.Write(item);
                Console.Write(" , ");
            }
            Console.WriteLine();

            jobMan.SetJobSuccessful(1);
            //jobMan.SetJobFailed(1);

            foreach (int item in jobMan.GetNextAvailableJobs())
            {
                Console.Write(item);
                Console.Write(" , ");
            }
            Console.WriteLine();
        }
    }
}
