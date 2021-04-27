using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine.AI;
using System;

namespace RPG.Controller
{
    public class AIController : MonoBehaviour
    {
        //Try and reformate this later with a FSM

  


        //Cached Fields
        private GameObject player;
        private Fighter fighter;
        private Health health;
        private Mover mover;
        private NavMeshAgent agent;
        private ActionScheduler scheduler;

        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] float wayPointTolerence = 1f;
        private Vector3 initialPos;
        private int currentWaypointIndex = 0;

        [SerializeField] private float chaseDistance = 5f;
        float timeSinceLastSeenPlayer = Mathf.Infinity;
        float suspicionTimer = 3f;
        private float timeDwelling = 0f;
        [SerializeField] float dwellTimer = 2f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = .2f;

        private void Start()
        {
            scheduler = GetComponent<ActionScheduler>();
            player = GameObject.FindGameObjectWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            agent = GetComponent<NavMeshAgent>();
            initialPos = transform.position;
        }
        private void Update()
        {
            if (health.IsDead()) return;
            if (InAttackRange())
            {
                timeSinceLastSeenPlayer = 0;
                AttackBehaviour();
            }
            else if(timeSinceLastSeenPlayer<suspicionTimer)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            timeSinceLastSeenPlayer += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = initialPos;

            if(patrolPath!=null)
            {
                if(AtWaypoint())
                {
                    if (timeDwelling >= dwellTimer)
                    {
                        CycleWaypoint();
                        timeDwelling = 0;
                    }
                    timeDwelling += Time.deltaTime;
                }
                nextPosition = GetCurrentWaypoint();
            }

            mover.StartMoveAction(nextPosition, agent,patrolSpeedFraction);
        }

        private Vector3 GetCurrentWaypoint()
        {
            if (patrolPath != null)
                return patrolPath.GetWayPoint(currentWaypointIndex);
            else return initialPos;
        }

        private void CycleWaypoint()
        {
            if (patrolPath != null)
                currentWaypointIndex = patrolPath.GetNextWayPointIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < wayPointTolerence;
        }

        private void SuspicionBehaviour()
        {
            scheduler.CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private bool InAttackRange()
        {
            return Vector3.Distance(gameObject.transform.position, player.transform.position)<chaseDistance;
        }

        //called by unity
        private void OnDrawGizmosSelected()
        {
            //Super cool so basically when i select an enemy i can get a visual representation
            //of his chase distance
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);


        }
    }
}