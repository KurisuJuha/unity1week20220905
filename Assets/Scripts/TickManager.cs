using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : SingletonMonoBehaviour<TickManager>
{
    public List<IEnumerator> jobs;
    public float interval;
    public int JobCount;

    void Start()
    {
        jobs = new List<IEnumerator>();
        StartCoroutine(tick());
    }

    private void Update()
    {
        JobCount = jobs.Count;
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

            yield return new WaitForSeconds(interval);
        }
    }

    public static void AddJob(IEnumerator job)
    {
        Instance.jobs.Add(job);
    }
}
