using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Controller
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = .5f;
        private void OnDrawGizmos()
        {
            for(int i=0;i<transform.childCount;i++)
            {
                Vector3 currPos = GetWayPoint(i);
                Gizmos.DrawSphere(currPos, waypointGizmoRadius);
                Vector3 nextSphere = GetNextWayPoint(i);
                Gizmos.DrawLine(currPos, nextSphere);

            }
        }

        private Vector3 GetNextWayPoint(int i)
        {
            return i + 1 < transform.childCount ?
                GetWayPoint(i + 1) : GetWayPoint(0);
        }
        public int GetNextWayPointIndex(int i)
        {
            return i + 1 < transform.childCount ?
                i+1 : 0;
        }
        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}