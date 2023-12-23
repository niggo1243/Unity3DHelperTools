using UnityEngine;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// A helper class that resets/ respawns a <see cref="Transform"/> relative to another <see cref="Transform"/>
    /// </summary>
    public static class ResetTargetHelper
    {
        /// <summary>
        /// Get the direction to respawn relative to the "<paramref name="relativeTo"/>" <see cref="Transform"/> angled (across the global up axis)
        /// </summary>
        /// <param name="relativeTo">Respawn direction relative to</param>
        /// <param name="minMaxAngle">The random y angle tolerance</param>
        /// <returns>A global <see cref="Vector3"/> direction</returns>
        public static Vector3 ResetDirectionRandomized(Transform relativeTo, Vector2 minMaxAngle)
        {
            Vector3 directionToSpawn = relativeTo.forward;
            float angle = NumericHelper.GetRandomFloatFromMinMaxVector(minMaxAngle);
            //invert angle 50/50 chance
            if (NumericHelper.RandomChanceSuccess01(.5f)) angle = -angle;
            
            return Quaternion.AngleAxis(angle, Vector3.up) * directionToSpawn;
        }
        
        /// <summary>
        /// Respawn the <see cref="Transform"/> "<paramref name="target"/>" relative to the <see cref="Transform"/> "<paramref name="relativeTo"/>"
        /// along the given "<paramref name="direction"/>" and "<paramref name="resetDist"/>".
        /// </summary>
        /// <param name="target">The <see cref="Transform"/> to respawn</param>
        /// <param name="relativeTo"><see cref="Transform"/></param>
        /// <param name="direction">The direction from the "<paramref name="relativeTo"/>" <see cref="Transform"/>
        /// to respawn the "<paramref name="target"/>" <see cref="Transform"/></param>
        /// <param name="resetDist">The distance to spawn the target relative to the "<paramref name="relativeTo"/>" <see cref="Transform"/></param>
        /// <param name="maxDistToRetrieveClosestPointOnSurface">
        /// We will shoot a raycast to get a valid surface and this is the max distance of that ray
        /// </param>
        /// <param name="addedHeightForRaycast">
        /// Shoot the ray from the potential spawn point of the "<paramref name="target"/>" from this offset height, downwards
        /// </param>
        /// <param name="layerMasks">
        /// The supported <see cref="Collider"/> layers
        /// </param>
        public static void ResetPosition(Transform target, Transform relativeTo,
            Vector3 direction,
            float resetDist, float maxDistToRetrieveClosestPointOnSurface, float addedHeightForRaycast, string[] layerMasks)
        {
            //get the air point slightly behind and above the player
            Vector3 airPoint = relativeTo.position + (direction * resetDist) + (Vector3.up * addedHeightForRaycast);
            //get the hit point on the surface (terrain)
            Vector3 hitPoint = RandomPointsHelper.GetClosestPointOnSurface(airPoint, Vector3.down, maxDistToRetrieveClosestPointOnSurface, layerMasks);

            target.position = hitPoint + (Vector3.up * .05f);
        }

        /// <summary>
        /// Respawn the <see cref="Transform"/> "<paramref name="target"/>" relative to the <see cref="Transform"/> "<paramref name="relativeTo"/>"
        /// along the given "<paramref name="direction"/>" and "<paramref name="resetDist"/>".
        /// </summary>
        /// <param name="collider">
        /// The <see cref="Collider"/> to toggle before and after the spawn
        /// </param>
        /// <param name="target">The <see cref="Transform"/> to respawn</param>
        /// <param name="relativeTo"><see cref="Transform"/></param>
        /// <param name="direction">The direction from the "<paramref name="relativeTo"/>" <see cref="Transform"/>
        /// to respawn the "<paramref name="target"/>" <see cref="Transform"/></param>
        /// <param name="resetDist">The distance to spawn the target relative to the "<paramref name="relativeTo"/>" <see cref="Transform"/></param>
        /// <param name="maxDistToRetrieveClosestPointOnSurface">
        /// We will shoot a raycast to get a valid surface and this is the max distance of that ray
        /// </param>
        /// <param name="addedHeightForRaycast">
        /// Shoot the ray from the potential spawn point of the "<paramref name="target"/>" from this offset height, downwards
        /// </param>
        /// <param name="layerMasks">
        /// The supported <see cref="Collider"/> layers
        /// </param>
        public static void ResetPositionWithCollider(
            Collider collider,
            Transform target, Transform relativeTo, 
            Vector3 direction,
            float resetDist, float maxDistToRetrieveClosestPointOnSurface, float addedHeightForRaycast, string[] layerMasks)
        {
            collider.enabled = false;

            ResetPosition(target, relativeTo, direction, resetDist, maxDistToRetrieveClosestPointOnSurface, addedHeightForRaycast, layerMasks);
            
            collider.enabled = true;
        }
        
        /// <summary>
        /// Respawn the <see cref="Transform"/> "<paramref name="target"/>" relative to the <see cref="Transform"/> "<paramref name="relativeTo"/>"
        /// along the given "<paramref name="direction"/>" and "<paramref name="resetDist"/>".
        /// </summary>
        /// <param name="rigidbody">
        /// The <see cref="Rigidbody"/> to toggle before and after the spawn
        /// </param>
        /// <param name="target">The <see cref="Transform"/> to respawn</param>
        /// <param name="relativeTo"><see cref="Transform"/></param>
        /// <param name="direction">The direction from the "<paramref name="relativeTo"/>" <see cref="Transform"/>
        /// to respawn the "<paramref name="target"/>" <see cref="Transform"/></param>
        /// <param name="resetDist">The distance to spawn the target relative to the "<paramref name="relativeTo"/>" <see cref="Transform"/></param>
        /// <param name="maxDistToRetrieveClosestPointOnSurface">
        /// We will shoot a raycast to get a valid surface and this is the max distance of that ray
        /// </param>
        /// <param name="addedHeightForRaycast">
        /// Shoot the ray from the potential spawn point of the "<paramref name="target"/>" from this offset height, downwards
        /// </param>
        /// <param name="layerMasks">
        /// The supported <see cref="Collider"/> layers
        /// </param>
        public static void ResetPositionWithRigidbody(
            Rigidbody rigidbody,
            Transform target, Transform relativeTo, 
            Vector3 direction,
            float resetDist, float maxDistToRetrieveClosestPointOnSurface, float addedHeightForRaycast, string[] layerMasks)
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            ResetPosition(target, relativeTo, direction, resetDist, maxDistToRetrieveClosestPointOnSurface, addedHeightForRaycast, layerMasks);

            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }
        
        /// <summary>
        /// Respawn the <see cref="Transform"/> "<paramref name="target"/>" relative to the <see cref="Transform"/> "<paramref name="relativeTo"/>"
        /// along the given "<paramref name="direction"/>" and "<paramref name="resetDist"/>".
        /// </summary>
        /// <param name="rigidbody">
        /// The <see cref="Rigidbody"/> to toggle before and after the spawn
        /// </param>
        /// <param name="collider">
        /// The <see cref="Collider"/> to toggle before and after the spawn
        /// </param>
        /// <param name="target">The <see cref="Transform"/> to respawn</param>
        /// <param name="relativeTo"><see cref="Transform"/></param>
        /// <param name="direction">The direction from the "<paramref name="relativeTo"/>" <see cref="Transform"/>
        /// to respawn the "<paramref name="target"/>" <see cref="Transform"/></param>
        /// <param name="resetDist">The distance to spawn the target relative to the "<paramref name="relativeTo"/>" <see cref="Transform"/></param>
        /// <param name="maxDistToRetrieveClosestPointOnSurface">
        /// We will shoot a raycast to get a valid surface and this is the max distance of that ray
        /// </param>
        /// <param name="addedHeightForRaycast">
        /// Shoot the ray from the potential spawn point of the "<paramref name="target"/>" from this offset height, downwards
        /// </param>
        /// <param name="layerMasks">
        /// The supported <see cref="Collider"/> layers
        /// </param>
        public static void ResetPositionWithRigidbodyAndCollider(
            Rigidbody rigidbody, Collider collider,
            Transform target, Transform relativeTo, 
            Vector3 direction,
            float resetDist, float maxDistToRetrieveClosestPointOnSurface, float addedHeightForRaycast, string[] layerMasks)
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            ResetPositionWithCollider(collider, target, relativeTo, direction, resetDist, maxDistToRetrieveClosestPointOnSurface, addedHeightForRaycast, layerMasks);

            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }
    }
}
