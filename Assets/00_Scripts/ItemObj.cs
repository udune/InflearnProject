using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemObj : MonoBehaviour
{
    [SerializeField] Transform itemTextRect;
    [SerializeField] TextMeshProUGUI itemText;
    [SerializeField] GameObject[] raritys;
    [SerializeField] ParticleSystem loot;
    [SerializeField] float fireAngle = 45.0f;
    [SerializeField] float gravity = 9.8f;
    
    Rarity rarity;

    private bool isCheck;

    void RarityCheck()
    {
        isCheck = true;
        transform.rotation = Quaternion.identity;
        raritys[(int)rarity].SetActive(true);
        itemTextRect.gameObject.SetActive(true);
        itemTextRect.SetParent(BaseCanvas.Instance.HolderLayer(2));
        itemText.text = Utils.String_Color_Rarity(rarity) + "TEST ITEM" + "</color>";
        StartCoroutine(LootItem());
    }

    IEnumerator LootItem()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 1.5f));

        for (int i = 0; i < raritys.Length; i++)
        {
            raritys[i].SetActive(false);
        }
        
        itemTextRect.transform.SetParent(transform);
        itemTextRect.gameObject.SetActive(false);
        
        loot.Play();

        yield return new WaitForSeconds(0.5f);
        
        BaseManager.Pool.poolDictionary["ItemObj"].Return(gameObject);
    }

    private void Update()
    {
        if (!isCheck)
            return;
        itemTextRect.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void Init(Vector3 pos)
    {
        rarity = (Rarity) Random.Range(0, 5);
        isCheck = false;
        transform.position = pos;
        Vector3 targetPos = new Vector3(pos.x + (Random.insideUnitSphere.x * 2.0f), 0.5f, pos.z + (Random.insideUnitSphere.y * 2.0f));
        StartCoroutine(SimulateProjectile(targetPos));
    }
    
    private IEnumerator SimulateProjectile(Vector3 pos)
    {
        float targetDistance = Vector3.Distance(transform.position, pos);
        float projectileVelocity = targetDistance / (Mathf.Sin(2 * fireAngle * Mathf.Deg2Rad) / gravity);
        float Vx = Mathf.Sqrt(projectileVelocity) * Mathf.Cos(fireAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectileVelocity) * Mathf.Sin(fireAngle * Mathf.Deg2Rad);
        float flightDuration = targetDistance / Vx;
        
        transform.rotation = Quaternion.LookRotation(pos - transform.position);

        float time = 0.0f;
        while (time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * time)) * Time.deltaTime, Vx * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }

        RarityCheck();
    }
}
