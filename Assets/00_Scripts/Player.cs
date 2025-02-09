using UnityEngine;

namespace _00_Scripts
{
    public class Player : Character
    {
        public CharacterScriptable characterData;
        public string chracterName;
        public GameObject trailObject;
        private Vector3 startPos;
        private Quaternion rot;

        protected override void Start()
        {
            base.Start();
        
            DataSet(Resources.Load<CharacterScriptable>($"Scriptable/{chracterName}"));
        
            Spawner.players.Add(this);
        
            startPos = transform.position;
            rot = transform.rotation;
        }

        private void DataSet(CharacterScriptable characterData)
        {
            this.characterData = characterData;
            attackRange = characterData.attackRange;
            SetAttackHp();
        }

        public void SetAttackHp()
        {
            attack = BaseManager.Player.Get_ATK(characterData.rarity);
            hp = BaseManager.Player.Get_HP(characterData.rarity);
        }

        private void Update()
        {
            FindClosetTarget(Spawner.monsters.ToArray());
        
            if (target == null)
            {
                float targetPos = Vector3.Distance(transform.position, startPos);
                if (targetPos > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime);
                    transform.LookAt(startPos);
                    AnimatorChange(IsMOVE);
                }
                else
                {
                    transform.rotation = rot;
                    AnimatorChange(IsIDLE);
                }
                return;
            }

            if (target.GetComponent<Character>().isDead)
                FindClosetTarget(Spawner.monsters.ToArray());
        
            float targetDistance = Vector3.Distance(transform.position, target.position);
            if (targetDistance <= targetRange && targetDistance > attackRange && isAttack == false)
            {
                AnimatorChange(IsMOVE);
                transform.LookAt(target.position);
                transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime);
            }
            else if (targetDistance <= attackRange && isAttack == false)
            {
                isAttack = true;
                AnimatorChange(IsATTACK);
                Invoke(nameof(InitAttack), 1.0f);
            }
        }

        public override void GetDamage(double damage)
        {
            base.GetDamage(damage);

            var goObj = BaseManager.Pool.PoolingObj("HitText").Get((value) =>
            {
                value.GetComponent<HitText>().Init(transform.position, damage, true);
            });
        
            hp -= damage;
        }
    
        protected override void Attack()
        {
            base.Attack();
            trailObject.SetActive(true);
        
            Invoke(nameof(TrailDisable), 1.0f);
        }

        private void TrailDisable() => trailObject.SetActive(false);
    }
}
