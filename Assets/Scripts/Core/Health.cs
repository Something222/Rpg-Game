using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;
        bool isDead = false;
        private ActionScheduler scheduler;
        private void Start()
        {
            scheduler = GetComponent<ActionScheduler>();
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            print(health);
            if (health == 0 && GetComponent<Animator>() != null )
            {
                Die();
            }
        }
        
        private void Die()
        {
            if (isDead) return;
            GetComponent<Animator>().SetTrigger("Die");
            isDead = true;
            scheduler.CancelCurrentAction();
            if(GetComponent<NavMeshAgent>()!=null)
            GetComponent<NavMeshAgent>().enabled = false;
            
        }


    }
}