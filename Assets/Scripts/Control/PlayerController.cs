using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;
using System;

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


        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

        // Update is called once per frame
        void Update()
        {
            InteractWithMovement();
            InteractWithCombat();
        }

        private void InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach(RaycastHit hit in hits)
            {
                if(hit.collider.GetComponent<CombatTarget>()!=null)
                {
                    if(Input.GetMouseButtonDown(0))
                    fighter.Attack();
                }
            }
        }

        private void InteractWithMovement()
        {
            if (Input.GetMouseButton(0))
                MoveToCursor();
        }

        private void MoveToCursor()
        {
            RaycastHit hit;
            bool hashit = Physics.Raycast(GetMouseRay(), out hit);
            if (hashit)
                mover.MoveTo(hit.point, agent);
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
