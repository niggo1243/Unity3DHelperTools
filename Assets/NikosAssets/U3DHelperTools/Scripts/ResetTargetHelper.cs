using UnityEngine;

namespace NikosAssets.Helpers
{
    public static class ResetTargetHelper
    {
        public static Vector3 ResetDirectionRandomized(Transform target, Vector2 minMaxAngle)
        {
            Vector3 directionToSpawn = target.forward;
            float angle = Random.Range(minMaxAngle.x, minMaxAngle.y);
            //invert angle 50/50 chance
            if (NumericHelper.RandomChanceSuccess01(.5f)) angle = -angle;
            
            return Quaternion.AngleAxis(angle, Vector3.up) * directionToSpawn;
        }
        
        public static void ResetPosition(Transform target, Transform relativeTo,
            Vector3 direction,
            float resetDist, float maxDistToRetrieveClosestPointOnSurface, float fromUpDist, string[] layerMasks)
        {
            //get the air point slightly behind and above the player
            Vector3 airPoint = relativeTo.position + (direction * resetDist) + (Vector3.up * fromUpDist);
            //get the hit point on the surface (terrain)
            Vector3 hitPoint = RandomPointsHelper.GetClosestPointOnSurface(airPoint, Vector3.down, maxDistToRetrieveClosestPointOnSurface, layerMasks);

            target.position = hitPoint + (Vector3.up * .05f);
        }
        
        public static void ResetPositionWithCollider(
            Collider collider,
            Transform target, Transform relativeTo, 
            Vector3 direction,
            float resetDist, float maxDist, float fromUpDist, string[] layerMasks)
        {
            collider.enabled = false;

            ResetPosition(target, relativeTo, direction, resetDist, maxDist, fromUpDist, layerMasks);
            
            collider.enabled = true;
        }
        
        public static void ResetPositionWithRigidbody(
            Rigidbody rigidbody,
            Transform target, Transform relativeTo, 
            Vector3 direction,
            float resetDist, float maxDist, float fromUpDist, string[] layerMasks)
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            ResetPosition(target, relativeTo, direction, resetDist, maxDist, fromUpDist, layerMasks);

            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }
        
        public static void ResetPositionWithRigidbodyAndCollider(
            Rigidbody rigidbody, Collider collider,
            Transform target, Transform relativeTo, 
            Vector3 direction,
            float resetDist, float maxDist, float fromUpDist, string[] layerMasks)
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            ResetPositionWithCollider(collider, target, relativeTo, direction, resetDist, maxDist, fromUpDist, layerMasks);

            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }
    }
}
