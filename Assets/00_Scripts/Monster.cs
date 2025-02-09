using System;
using System.Collections;
using _00_Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class Monster : Character
{
    public float speed;
    private bool isSpawn;
    
    public void Init()
    {
        isDead = false;
        attack = 10;
        hp = 5;
        attackRange = 0.5f;
        targetRange = Mathf.Infinity;
        StartCoroutine(SpawnStart());
    }

    private void Update()
    {
        if (!isSpawn)
            return;
        
        if (target == null)
            FindClosetTarget(Spawner.players.ToArray());

        if (target != null)
        {
            float targetDistance = Vector3.Distance(transform.position, target.position);
            if (targetDistance > attackRange && !isAttack)
            {
                AnimatorChange(IsMOVE);
                transform.LookAt(target.position);
                transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime);
            }
            else if (targetDistance <= attackRange && !isAttack)
            {
                isAttack = true;
                AnimatorChange(IsATTACK);
                Invoke(nameof(InitAttack), 1.0f);
            }
        }
    }
    
    private IEnumerator SpawnStart()
    {
        float current = 0.0f;
        float percent = 0.0f;
        float start = 0.0f;
        float end = transform.localScale.x;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.2f;
            // 선형보간 (시작 값, 끝 값, 시간) == 시작에서 끝까지 특정 시간속도로 이동해라.
            float lerpPos = Mathf.Lerp(start, end, percent);
            transform.localScale = new Vector3(lerpPos, lerpPos, lerpPos);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        isSpawn = true;
    }

    protected override void Start()
    {
        base.Start();
        hp = 5;
    }

    public override void GetDamage(double damage)
    {
        if (isDead)
            return;
        
        bool get = Critical(ref damage);

        BaseManager.Pool.PoolingObj("HitText").Get((value) =>
        {
            value.GetComponent<HitText>().Init(transform.position, damage, false, get);
        });
        
        hp -= damage;

        if (hp <= 0)
        {
            isDead = true;
            Spawner.monsters.Remove(this);
            BaseManager.Pool.PoolingObj("Smoke").Get((value) =>
            {
                value.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
                BaseManager.Instance.ReturnToPool(value.GetComponent<ParticleSystem>().main.duration, value, "Smoke");
            });

            BaseManager.Pool.PoolingObj("Coin_Parent").Get((value) =>
            {
                value.GetComponent<Coin_Parent>().Init(transform.position);
            });

            for (int i = 0; i < 3; i++)
            {
                BaseManager.Pool.PoolingObj("ItemObj").Get((value) =>
                {
                    value.GetComponent<ItemObj>().Init(transform.position);
                });
            }
            
            BaseManager.Pool.poolDictionary["Monster"].Return(gameObject);
        }
    }

    // ref, out
    private bool Critical(ref double damage)
    {
        float randomValue = Random.Range(0f, 100.0f);
        if (randomValue <= BaseManager.Player.CriticalPercentage)
        {
            damage *= BaseManager.Player.CriticalDamage / 100;
            return true;
        }

        return false;
    }
}
