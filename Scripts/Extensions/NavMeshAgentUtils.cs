#if !UNITY_2022_2_OR_NEWER

using UnityEngine;
using UnityEngine.AI;

namespace NikosAssets.Helpers.Extensions
{
    /// <summary>
    /// An extension helper class for <see cref="NavMeshAgent"/> and desired movement calculations
    /// </summary>
    public static class NavMeshAgentUtils
    {
        /// <summary>
        /// Gets the desired local z movement speed/ velocity of the <see cref="NavMeshAgent"/>.
        /// Use this for animation movement blending for example.
        /// </summary>
        /// <param name="navMeshAgent"></param>
        /// <returns>The desired local fwd speed</returns>
        public static float GetDesiredMovementSpeed(this NavMeshAgent navMeshAgent)
        {
            Vector3 move = navMeshAgent.desiredVelocity;
            move = navMeshAgent.transform.InverseTransformDirection(move);

            return move.z;
        }
        
        /// <summary>
        /// Get the desired local y turning speed/ velocity of the <see cref="NavMeshAgent"/>.
        /// Use this for animation movement blending for example.
        /// </summary>
        /// <param name="navMeshAgent"></param>
        /// <returns>The desired local y turning speed</returns>
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

#endif
