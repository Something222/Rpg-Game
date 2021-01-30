using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {

        //Cached Components
        private NavMeshAgent agent;
        private Animator anim;


        public void MoveTo(Vector3 destination, NavMeshAgent agent)
        {
            agent.destination = destination;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            //this turns the agent.velocity into a local velocity so the z value is based on the 
            //players cordinates not the world cordinates
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            anim.SetFloat("forwardSpeed", localVelocity.z);
        }

        void Start()
        {
            anim = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAnimator();
        }

    }
}
