using System;
using System.Collections;
using System.Collections.Generic;
using _00_Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public int count;
    public float spawnTime;

    public static List<Monster> monsters = new List<Monster>();
    public static List<Player> players = new List<Player>();

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        int tempCount = count;

        for (int i = 0; i < tempCount; i++)
        {
            Vector3 calcPos = Vector3.zero + Random.insideUnitSphere * 5.0f;
            calcPos.y = 0.0f;

            if (Vector3.Distance(calcPos, Vector3.zero) <= 3.0f)
            {
                tempCount++;
                continue;
            }

            Vector3 pos = calcPos;
            BaseManager.Pool.PoolingObj("Monster").Get((value) =>
            {
                value.GetComponent<Monster>().Init();
                value.transform.position = pos;
                value.transform.LookAt(Vector3.zero);
                
                monsters.Add(value.GetComponent<Monster>());
            });
        }

        yield return new WaitForSeconds(spawnTime);

        StartCoroutine(SpawnCoroutine());
    }
}
