using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin_Parent : MonoBehaviour
{
    private Vector3 target;
    private Camera cam;
    private RectTransform[] childs = new RectTransform[5];
    
    [Range(0.0f, 500.0f)]
    [SerializeField] private float distanceRange, speed;

    private void Awake()
    {
        cam = Camera.main;
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = transform.GetChild(i).GetComponent<RectTransform>();
        }
    }

    public void Init(Vector3 pos)
    {
        target = pos;
        
        transform.position = cam.WorldToScreenPoint(pos);

        for (int i = 0; i < 5; i++)
        {
            childs[i].anchoredPosition = Vector2.zero;
        }

        transform.SetParent(BaseCanvas.Instance.HolderLayer(0));

        StartCoroutine(Coin_Effect());
    }

    IEnumerator Coin_Effect()
    {
        Vector2[] RandomPos = new Vector2[childs.Length];
        for (int i = 0; i < childs.Length; i++)
        {
            RandomPos[i] = new Vector2(target.x, target.y) + Random.insideUnitCircle * Random.Range(-distanceRange, distanceRange);
        }

        while (true)
        {
            for (int i = 0; i < childs.Length; i++)
            {
                RectTransform rect = childs[i];
                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, RandomPos[i], Time.deltaTime * speed);
            }

            if (DistanceBoolean(RandomPos, 0.5f))
                break;

            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        
        while (true)
        {
            for (int i = 0; i < childs.Length; i++)
            {
                RectTransform rect = childs[i];
                rect.position = Vector2.MoveTowards(rect.position, BaseCanvas.Instance.coin.position, Time.deltaTime * speed * 10.0f);
            }
        
            if (DistanceBoolean_World(0.5f))
            {
                BaseManager.Pool.poolDictionary["Coin_Parent"].Return(gameObject);
                break;
            }
            
            yield return null;
        }
    }

    private bool DistanceBoolean(Vector2[] end, float range)
    {
        for (int i = 0; i < childs.Length; i++)
        {
            float distance = Vector2.Distance(childs[i].anchoredPosition, end[i]);
            if (distance > range)
            {
                return false;
            }
        }

        return true;
    }

    private bool DistanceBoolean_World(float range)
    {
        for (int i = 0; i < childs.Length; i++)
        {
            float distance = Vector2.Distance(childs[i].position, BaseCanvas.Instance.coin.position);
            if (distance > range)
            {
                return false;
            }
        }

        return true;
    }
}
