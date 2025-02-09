using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IPool
{
    Transform parentTransform { get; set; }
    Queue<GameObject> pool { get; set; }
    
    GameObject Get(Action<GameObject> action = null);
    void Return(GameObject obj, Action<GameObject> action = null);
}

public class ObjectPool : IPool
{
    public Queue<GameObject> pool { get; set; } = new Queue<GameObject>();
    public Transform parentTransform { get; set; }

    public GameObject Get(Action<GameObject> action = null)
    {
        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        if (action != null)
            action?.Invoke(obj);
        return obj;
    }

    public void Return(GameObject obj, Action<GameObject> action = null)
    {
        pool.Enqueue(obj);
        obj.transform.SetParent(parentTransform);
        obj.SetActive(false);
        if (action != null)
            action?.Invoke(obj);
    }
}

public class PoolingManager
{
    public Dictionary<string, IPool> poolDictionary = new Dictionary<string, IPool>();
    private Transform baseObj;

    public void Initialize(Transform T)
    {
        baseObj = T;
    }

    public IPool PoolingObj(string path)
    {
        if (!poolDictionary.ContainsKey(path))
            AddPool(path); 
        if (poolDictionary[path].pool.Count <= 0) AddQueue(path);
        return poolDictionary[path];
    }

    private GameObject AddPool(string path)
    {
        GameObject obj = new GameObject(path + "##POOL");
        obj.transform.SetParent(baseObj);
        ObjectPool objectPool = new ObjectPool();
        poolDictionary.Add(path, objectPool);
        objectPool.parentTransform = obj.transform;
        
        return obj;
    }

    private void AddQueue(string path)
    {
        GameObject go = BaseManager.Instance.InstantiatePath(path);
        go.transform.SetParent(poolDictionary[path].parentTransform);
        
        poolDictionary[path].Return(go);
    }
}
