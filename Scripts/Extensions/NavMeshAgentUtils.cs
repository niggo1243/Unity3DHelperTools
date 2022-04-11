using UnityEngine;
using UnityEngine.AI;

namespace NikosAssets.Helpers.Extensions
{
    /// <summary>
    /// A extension helper class for <see cref="NavMeshAgent"/> and desired movement calculations
    /// </summary>
    public static class NavMeshAgentUtils
    {
        public static float GetDesiredMovementSpeed(this NavMeshAgent navMeshAgent)
        {
            Vector3 move = navMeshAgent.desiredVelocity;
            move = navMeshAgent.transform.InverseTransformDirection(move);

            return move.z;
        }
        
        public static float GetDesiredTuringSpeed(this NavMeshAgent navMeshAgent)
        {
            Transform navMeshTransform = navMeshAgent.transform;
            Vector3 move = navMeshAgent.desiredVelocity;

            if (Mathf.Abs(move.sqrMagnitude) > 1)
            {
                move.Normalize();
            }

            move = navMeshTransform.InverseTransformDirection(move);
            move = Vector3.ProjectOnPlane(move, navMeshTransform.up);

            float turnAmount = Mathf.Atan2(move.x, move.z);

            return turnAmount;
        }
    }
}
