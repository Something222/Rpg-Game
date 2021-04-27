using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;
using System;
using RPG.Core;

namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {
        //Cached Fields
        //Idea make a script called player attributes to cache attributes then give that to 
        //everything and call from that

        private NavMeshAgent agent;
        private Mover mover;
        private Fighter fighter;
        private Health health;

        void Start()
        {
            health = GetComponent<Health>();
            agent = GetComponent<NavMeshAgent>();
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

        // Update is called once per frame
        void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;

           
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach(RaycastHit hit in hits)
            {
               //check if the clickable object has a combat target if not fail
                    CombatTarget target = hit.collider.GetComponent<CombatTarget>();
                if(target==null)continue;

                //Checks if it has a health compnent if not fail
                if (!fighter.CanAttack(target.gameObject))
                    continue;

                if(Input.GetMouseButtonDown(0))
                   fighter.Attack(target.gameObject);
                 return true;
 

            }
            return false;
        }

       

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hashit = Physics.Raycast(GetMouseRay(), out hit);
            if (hashit)
            {
                if (Input.GetMouseButton(0))
                    mover.StartMoveAction(hit.point, agent,1);
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
