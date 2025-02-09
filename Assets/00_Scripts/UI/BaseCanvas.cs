using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCanvas : MonoBehaviour
{
    public static BaseCanvas Instance;
    public Transform coin;
    [SerializeField] private Transform layer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Transform HolderLayer(int value)
    {
        return layer.GetChild(value);
    }
}
