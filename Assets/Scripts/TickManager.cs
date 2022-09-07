using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : SingletonMonoBehaviour<TickManager>
{
    public List<IEnumerator> jobs;

    void Start()
    {
        jobs = new List<IEnumerator>();
        StartCoroutine(tick());
    }

    IEnumerator tick()
    {
        while (true)
        {
            List<IEnumerator> list = new List<IEnumerator>();
            for (int i = 0; i < jobs.Count; i++)
            {
                if (jobs[i].MoveNext())
                {
                    list.Add(jobs[i]);
                }
            }
            jobs.Clear();
            jobs = list;

            yield return new WaitForSeconds(0.05f);
        }
    }

    public static void AddJob(IEnumerator job)
    {
        Instance.jobs.Add(job);
    }
}
