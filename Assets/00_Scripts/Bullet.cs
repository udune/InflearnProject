using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _00_Scripts
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed;
    
        private Transform target;
        private Vector3 targetPos;
        private double damage;
        private string characterName;
        private bool getHit;

        private Dictionary<string, GameObject> projectilesDict = new ();
        private Dictionary<string, ParticleSystem> muzzlesDict = new ();

        public ParticleSystem attackParticle;

        private void Awake()
        {
            Transform projectiles = transform.GetChild(0);
            Transform muzzles = transform.GetChild(1);

            for (int i = 0; i < projectiles.childCount; i++)
                projectilesDict.Add(projectiles.GetChild(i).name, projectiles.GetChild(i).gameObject);

            for (int i = 0; i < muzzles.childCount; i++)
                muzzlesDict.Add(muzzles.GetChild(i).name, muzzles.GetChild(i).GetComponent<ParticleSystem>());
        }

        public void AttackInit(Transform target, double damage)
        {
            this.target = target;
            if (this.target != null)
            {
                target.GetComponent<Character>().GetDamage(damage);
            }

            getHit = true;
            attackParticle.Play();
            StartCoroutine(ReturnObject(attackParticle.main.duration));
        }

        public void Init(Transform target, double damage, string characterName)
        {
            getHit = false;
            this.target = target;
            transform.LookAt(this.target);
            targetPos = target.position;
            this.damage = damage;
            this.characterName = characterName;
            projectilesDict[this.characterName].gameObject.SetActive(true);
        }

        private void Update()
        {
            if (getHit) return;

            targetPos.y = 0.5f;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);

            if (Vector3.Distance(transform.position, targetPos) <= 0.1f)
            {
                if (target != null)
                {
                    getHit = true;
                    target.GetComponent<Character>().GetDamage(damage);
                    projectilesDict[characterName].gameObject.SetActive(false);
                    muzzlesDict[characterName].Play();
                    StartCoroutine(ReturnObject(muzzlesDict[characterName].main.duration));
                }
            }
        }

        private IEnumerator ReturnObject(float timer)
        {
            yield return new WaitForSeconds(timer);
            BaseManager.Pool.poolDictionary["Attack_Helper"].Return(gameObject);
        }
    }
}
