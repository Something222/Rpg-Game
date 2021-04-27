using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;
using RPG.Core;
namespace RPG.Movement
{
    public class Mover : MonoBehaviour,IAction
    {

        //Cached Components
        private NavMeshAgent agent;
        private Animator anim;
        private Fighter fighter;
        private ActionScheduler scheduler;

        [SerializeField] float maxSpeed = 6;

        public void StartMoveAction(Vector3 destination, NavMeshAgent agent,float speedFraction)
        {
            scheduler.StartAction(this);
            MoveTo(destination, agent,speedFraction);
        }
        public void MoveTo(Vector3 destination, NavMeshAgent agent,float speedFraction)
        {
            
            agent.destination = destination;
            agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            agent.isStopped = false;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            //this turns the agent.velocity into a local velocity so the z value is based on the 
            //players cordinates not the world cordinates
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            anim.SetFloat("forwardSpeed", localVelocity.z);
        }
        public void Cancel()
        {
            agent.isStopped = true;
        }
      
        void Start()
        {
            scheduler = GetComponent<ActionScheduler>();
            anim = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();
            fighter = GetComponent<Fighter>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAnimator();
        }

    }
}
