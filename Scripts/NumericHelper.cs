using UnityEngine;

namespace NikosAssets.Helpers
{
    public static class NumericHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentInput"></param>
        /// <param name="minInput"></param>
        /// <param name="maxInput"></param>
        /// <param name="minTarget"></param>
        /// <param name="maxTarget"></param>
        /// <returns></returns>
        public static float GetMappedResult(float currentInput, float minInput, float maxInput, float minTarget, float maxTarget)
        {
            float diffInput = maxInput - minInput;
            float shiftInputValue = currentInput - minInput;
            float inputPercentageResult = shiftInputValue / diffInput;

            float diffTargetValue = maxTarget - minTarget;

            return minTarget + diffTargetValue * inputPercentageResult;
        }

        public static bool Approx(float a, float b, float approxBuffer)
        {
            float diff = a - b;

            //Debug.Log(a);
            //Debug.Log(b);
            //Debug.Log(diff);
            //Debug.Log(approxBuffer);

            return Mathf.Abs(diff) <= approxBuffer;
        }

        public static bool RandomChanceSuccess01(float chance01)
        {
            float rand = UnityEngine.Random.Range(0f, 1f);

            /*
             * rand = 0,8; chance = 0,9 -> safe
             * rand = 0,6; chance = 0,5 -> fail
             * rand = 0,2; chance = 0,5 -> safe
             */

            return !(rand > chance01);
        }


        /// <summary>
        /// does the math to look at a given target
        /// </summary>
        /// <param name="posA"></param>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        public static Quaternion LookAt(Vector3 posA, Vector3 targetPos, Vector3 eulerOffset)
        {
            Quaternion thisQuat = Quaternion.LookRotation(posA - targetPos);
            thisQuat.eulerAngles -= eulerOffset;

            return thisQuat;
        }

        public static void CalculateTorqueRotationAlignment(Transform transform, Rigidbody rigidbody, Vector3 direction,
            float alignmentSpeed, float alignmentDamping)
        {
            // Compute target rotation (align rigidybody's up direction to the normal vector)
            Vector3 normal = direction;
            Vector3 proj = Vector3.ProjectOnPlane(transform.forward, normal);
            Quaternion targetRotation = Quaternion.LookRotation(proj, normal); // The target rotation can be replaced with whatever rotation you want to align to

            Quaternion deltaRotation = Quaternion.Inverse(transform.rotation) * targetRotation;
            Vector3 deltaAngles = GetRelativeAngles(deltaRotation.eulerAngles);
            Vector3 worldDeltaAngles = transform.TransformDirection(deltaAngles);

            // alignmentSpeed controls how fast you rotate the body towards the target rotation
            // alignmentDamping prevents overshooting the target rotation
            // Values used: alignmentSpeed = 0.025, alignmentDamping = 0.2
            rigidbody.AddTorque(alignmentSpeed * worldDeltaAngles - alignmentDamping * rigidbody.angularVelocity);

            // Convert angles above 180 degrees into negative/relative angles
            Vector3 GetRelativeAngles(Vector3 angles)
            {
                Vector3 relativeAngles = angles;
                if (relativeAngles.x > 180f)
                    relativeAngles.x -= 360f;
                if (relativeAngles.y > 180f)
                    relativeAngles.y -= 360f;
                if (relativeAngles.z > 180f)
                    relativeAngles.z -= 360f;

                return relativeAngles;
            }
        }

        /// <summary>
        /// more lightweight distance calc then unity internal Vector3.Distance
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float DistanceSquared(Vector3 a, Vector3 b)
        {
            return (b - a).sqrMagnitude;
        }
        
        public static bool IsInAreaDist(float dist, Vector2 minMaxDistance)
        {
            return dist >= minMaxDistance.x && dist <= minMaxDistance.y;
        }

        public static bool IsInAreaDistSquared(float distSquared, Vector2 minMaxDistance)
        {
            return distSquared >= minMaxDistance.x * minMaxDistance.x
                   && distSquared <= minMaxDistance.y * minMaxDistance.y;
        }
        
        /// <summary>
        /// checks if the origin transform is in the given angle view
        /// </summary>
        /// <param name="originTrans"></param>
        /// <param name="targetPos"></param>
        /// <param name="maxAngle"></param>
        /// <returns></returns>
        public static bool IsInViewArea3D(Vector3 originPos, Vector3 originNormal, Vector3 targetPos, float maxAngle)
        {
            Vector3 inverseDirectionToTarget = targetPos - originPos;
            float angle = Vector3.Angle(originNormal, inverseDirectionToTarget);

            return (angle < maxAngle);
        }

        /// <summary>
        /// Checks if the target is within the horizontal and vertical angle bounds of the origin (caster)
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="targetPos"></param>
        /// <param name="verticalMaxAngle"></param>
        /// <param name="horizontalMaxAngle"></param>
        /// <returns></returns>
        public static bool IsInViewHorAndVertBounds(Transform origin, Vector3 targetPos, float verticalMaxAngle, float horizontalMaxAngle)
        {
            Vector3 originPos = origin.position;
            Vector3 dirToTarget = targetPos - originPos;
            
            Quaternion rotCorrection = Quaternion.Inverse(origin.rotation);
            
            Vector3 dirToTargetGlobal = rotCorrection * dirToTarget;

            Vector2 verticalDirProjectionOn2DPlane = new Vector2(dirToTargetGlobal.z, dirToTargetGlobal.y);
            Vector2 horizontalDirProjectionOn2DPlane = new Vector2(dirToTargetGlobal.x, dirToTargetGlobal.z);

            float verticalAngle = Vector2.Angle(verticalDirProjectionOn2DPlane, new Vector2(1, 0));   
            float horizontalAngle = Vector2.Angle(horizontalDirProjectionOn2DPlane, new Vector2(0, 1));
            
            // Debug.Log("vert angle: " + verticalAngle);
            // Debug.Log("hot angle: " + horizontalAngle);
            
            return verticalAngle < verticalMaxAngle && horizontalAngle < horizontalMaxAngle;
        }
        
        public static float VectorFacingDotResult(Vector3 dir, Vector3 directionToCheck)
        {
            dir = dir.normalized;
            return Vector3.Dot(dir, directionToCheck);
        }
        
        /// <summary>
        /// Returns a vector division for each value of the given vectors
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static Vector3 Divide2Vectors(Vector3 dividend, Vector3 divisor)
        {
            return new Vector3(dividend.x / divisor.x, dividend.y / divisor.y, dividend.z / divisor.z);
        }
    }
}
