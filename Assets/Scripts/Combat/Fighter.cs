using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using UnityEngine.AI;
using RPG.Core;
namespace RPG.Combat
{
    public class Fighter : MonoBehaviour,IAction
    {
        //cached fields
        private Health target;
        private Mover mover;
        private NavMeshAgent agent;
        private ActionScheduler scheduler;
        private Animator anim;


        //Stats
        [SerializeField] float timeBetweenAttacks=1f;
        [SerializeField] float weaponRange = 2f;
        private float timeSinceLastAttack=Mathf.Infinity;
       [SerializeField] float damage=5f;

        public bool CanAttack(GameObject target)
        {
            if (target == null)
                return false;
            Health targetToTest = target.GetComponent<Health>();
            return targetToTest != null & !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            target = combatTarget.GetComponent<Health>();
            scheduler.StartAction(this);
            print("BANG BOOM STRAIGHT TO THE MOON"); 
        }
        private void AttackBehaviour()
        {
            //A little bit different from lesson in lesson if dead he puts a return in 
            // the update i prefer to check here so we can still move to the guy
            if (timeSinceLastAttack > timeBetweenAttacks && !target.IsDead())
            {
                //this will trigger the void Hit() event
                gameObject.transform.LookAt(target.transform);
                TriggerAttackAnim();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttackAnim()
        {
            anim.ResetTrigger("StopAttack");
            anim.SetTrigger("Attack");
        }

        //Animation Event
        void Hit()
        {
            if (target != null)
                target.TakeDamage(damage);
        }

        private bool TargetIsInWeaponRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }
        public void Cancel()
        {
            target = null;
            StopAttackAnims();
            GetComponent<Mover>().Cancel();
        }

        private void StopAttackAnims()
        {
            anim.ResetTrigger("Attack");
            anim.SetTrigger("StopAttack");
        }

        void Start()
        {
            scheduler = GetComponent<ActionScheduler>();
            agent = GetComponent<NavMeshAgent>();
            mover = GetComponent<Mover>();
            anim = GetComponent<Animator>();
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target != null)
            {
                mover.MoveTo(target.transform.position, agent,1);
                if (TargetIsInWeaponRange())
                {
                    mover.Cancel();

                    AttackBehaviour();
                }
            }


        }
    }
}