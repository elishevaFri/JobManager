using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobManager_varonisExcercise
{
    public class JobManager
    {
        Dictionary<int, HashSet<int>> dependenciesDic = new Dictionary<int, HashSet<int>>();
        Dictionary<int, int> unavailableDic = new Dictionary<int, int>();
        HashSet<int> availableDep = new HashSet<int>();
        HashSet<int> successedJobs = new HashSet<int>();
        HashSet<int> failedJobs = new HashSet<int>();

        public void Init(List<KeyValuePair<int, int>> dependencies) //key = job, value = dependsOn
        {
            foreach (var dep in dependencies)
            {
                if (dep.Key == dep.Value)
                    throw new Exception("job can not depend on itself");

                InitDependenciesDic(dep, dependenciesDic, unavailableDic);
                InitUnavailableDic(dep, dependenciesDic, unavailableDic);
                InitAvailableDep(dep, availableDep, unavailableDic);
            }
        }
        private void InitDependenciesDic(KeyValuePair<int, int> dep, Dictionary<int, HashSet<int>> dependenciesDic, Dictionary<int, int> unavailableDic)
        {
            if (!dependenciesDic.ContainsKey(dep.Value))
            {
                if(dependenciesDic.ContainsKey(dep.Key) && dependenciesDic[dep.Key].Contains(dep.Value)) // Circle!
                    throw new Exception("Circular dependency detected");

                dependenciesDic.Add(dep.Value, new HashSet<int>() { dep.Key });
            }
            else if (!dependenciesDic[dep.Value].Contains(dep.Key))
            {
                dependenciesDic[dep.Value].Add(dep.Key);
            }
        }
        private void InitUnavailableDic(KeyValuePair<int, int> dep, Dictionary<int, HashSet<int>> dependenciesDic, Dictionary<int, int> unavailableDic)
        { 
            if (!unavailableDic.ContainsKey(dep.Key))
            {
                unavailableDic.Add(dep.Key, 1);
            }
            else
                unavailableDic[dep.Key]++;
        }
        private void InitAvailableDep(KeyValuePair<int, int> dep, HashSet<int> availableDep, Dictionary<int, int> unavailableDic)
        {
            if (!unavailableDic.ContainsKey(dep.Value) && !availableDep.Contains(dep.Value))
            {
                availableDep.Add(dep.Value);
            }

            if (!unavailableDic.ContainsKey(dep.Key))
            {
                unavailableDic.Add(dep.Key, 1);
            }
            else if (availableDep.Contains(dep.Key))
            {
                unavailableDic[dep.Key]++;
                availableDep.Remove(dep.Key);
            }
        }

        public HashSet<int> GetNextAvailableJobs()
        {
            return availableDep;
        }
        public void SetJobSuccessful(int job)
        {
            CheckIfExist(job);
            
            availableDep.Remove(job);

            HashSet<int> depsOnJob = dependenciesDic[job];
            foreach (var item in depsOnJob)
            {
                unavailableDic[item]--;
                if (unavailableDic[item] == 0)
                {
                    availableDep.Add(item);
                }
                unavailableDic.Remove(item);
            }
        }

        public void SetJobFailed(int job) 
        {
            CheckIfExist(job);
            availableDep.Remove(job);
            HashSet<int> depsOnJob = dependenciesDic[job];
            foreach(var item in depsOnJob)
            {
                unavailableDic.Remove(item);
            }
        }

        private void CheckIfExist(int job)
        {
            if (!availableDep.Contains(job))
            {
                if (successedJobs.Contains(job))
                    throw new Exception($"job {job} has already successed");
                else if (failedJobs.Contains(job))
                    throw new Exception($"job {job} has already failed");
                else if (unavailableDic.ContainsKey(job))
                    throw new Exception($"job {job} depends on other jobs");
                else
                    throw new Exception($"job {job} is not exist");
            }
        }
    }
}
