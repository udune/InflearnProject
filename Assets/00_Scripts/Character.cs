using UnityEngine;

namespace _00_Scripts
{
    public class Character : MonoBehaviour
    {
        private Animator animator;
        private static readonly int IsIdle = Animator.StringToHash("isIDLE");
        private static readonly int IsMove = Animator.StringToHash("isMOVE");
        private static readonly int IsAttack = Animator.StringToHash("isATTACK");
        protected string IsIDLE = "isIDLE";
        protected string IsMOVE = "isMOVE";
        protected string IsATTACK = "isATTACK";

        // int - 4byte
        // double - 8byte
    
        public double hp;
        public double attack;
        public float attackSpeed;
        public bool isDead = false;
    
        protected float attackRange = 3.0f;
        protected float targetRange = 5.0f;
        protected bool isAttack;
        protected Transform target;

        [SerializeField] Transform bulletTransform;

        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
        }

        protected void InitAttack() => isAttack = false;
    
        protected void AnimatorChange(string temp)
        {
            if (temp == IsATTACK)
            {
                animator.SetTrigger(IsATTACK);
                return;
            }
        
            animator.SetBool(IsIdle, false);
            animator.SetBool(IsMove, false);
        
            animator.SetBool(temp, true);
        }

        protected virtual void Bullet()
        {
            if (target == null)
                return;
        
            BaseManager.Pool.PoolingObj("Attack_Helper").Get((value) =>
            {
                value.transform.position = bulletTransform.position;
                value.GetComponent<Bullet>().Init(target, attack, "CH_01");
            });
        }

        protected virtual void Attack()
        {
            if (target == null)
                return;

            BaseManager.Pool.PoolingObj("Attack_Helper").Get((value) =>
            {
                value.transform.position = target.position;
                value.GetComponent<Bullet>().AttackInit(target, 10);
            });
        }

        public virtual void GetDamage(double damage)
        {
        
        }

        protected void FindClosetTarget<T>(T[] targets) where T : Component
        {
            var monsters = targets;
            Transform closetTarget = null;
            float maxDistance = targetRange;

            foreach (var monster in monsters)
            {
                float targetDistance = Vector3.Distance(transform.position, monster.transform.position);

                if (targetDistance < maxDistance)
                {
                    closetTarget = monster.transform;
                    maxDistance = targetDistance;
                }
            }

            target = closetTarget;
            if (target != null)
                transform.LookAt(target.transform);
        }
    }
}
