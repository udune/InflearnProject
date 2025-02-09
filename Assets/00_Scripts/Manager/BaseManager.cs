using System;
using System.Collections;
using System.Collections.Generic;
using _00_Scripts.Manager;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    private static BaseManager instance;
    public static BaseManager Instance => instance;

    private static PoolingManager pool = new ();
    private static PlayerManager player = new ();
    public static PoolingManager Pool => pool;
    public static PlayerManager Player => player;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (instance == null)
        {
            instance = this;
            pool.Initialize(transform);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject InstantiatePath(string path)
    {
        return Instantiate(Resources.Load<GameObject>(path));
    }

    public void ReturnToPool(float timer, GameObject obj, string path)
    {
        StartCoroutine(ReturnToPoolCoroutine(timer, obj, path));
    }

    private IEnumerator ReturnToPoolCoroutine(float time, GameObject obj, string path)
    {
        yield return new WaitForSeconds(time);
        pool.poolDictionary[path].Return(obj);
    }
}
