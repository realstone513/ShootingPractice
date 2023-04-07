using System.Collections.Generic;
using UnityEngine;

public class CustomObjectPool : Object
{
    public List<GameObject> useQueue;
    public List<GameObject> unuseQueue;
    public GameObject spawnPrefab;
    public Transform spawnTransform;
    private readonly int limit;

    public int Count
    {
        get { return useQueue.Count + unuseQueue.Count; }
    }

    public CustomObjectPool(int poolLimitCount, GameObject spawnPrefab, Transform spawnTransform)
    {
        limit = poolLimitCount;
        useQueue = new (limit);
        unuseQueue = new (limit);
        this.spawnPrefab = spawnPrefab;
        this.spawnTransform = spawnTransform;
    }

    public GameObject GetObject()
    {
        GameObject target;
        if (Count < limit)
        {
            target = Instantiate(spawnPrefab, spawnTransform);
            target.name = spawnPrefab.name;
        }
        else
        {
            if (unuseQueue.Count == 0)
            {
                target = Instantiate(spawnPrefab, spawnTransform);
                target.name = spawnPrefab.name;
            }
            else
            {
                target = unuseQueue[0];
                unuseQueue.Remove(target);
                if (target != null)
                    target.SetActive(true);
            }
        }
        useQueue.Add(target);
        return target;
    }

    public void ReleaseObject(GameObject obj)
    {
        useQueue.Remove(obj);
        if (Count > limit)
            Destroy(obj);
        else
        {
            unuseQueue.Add(obj);
            obj.SetActive(false);
        }
    }
}