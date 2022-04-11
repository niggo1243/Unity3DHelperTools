using UnityEngine;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// Helps with numbers
    /// </summary>
    public static class NumericHelper
    {
        public enum AmountFilter
        {
            Irrelevant = 0,
            HasNone = 1,
            HasSome = 2,
            //HasNegative = 3
        }

        public static bool GetAmountValidation(AmountFilter amountFilter, int amount)
        {
            switch (amountFilter)
            {
                case AmountFilter.Irrelevant:
                    return true;
                case AmountFilter.HasNone:
                    return amount == 0;
                case AmountFilter.HasSome:
                    return amount > 0;
                // case AmountFilter.HasNegative:
                //     return amount < 0;
            }

            return false;
        }
        
        /// <summary>
        /// Map incoming input values to another value (with that you can keep the same ratio for different dimensions)
        /// </summary>
        /// <param name="currentInput"></param>
        /// <param name="minInput"></param>
        /// <param name="maxInput"></param>
        /// <param name="minTarget"></param>
        /// <param name="maxTarget"></param>
        /// <returns>
        /// The mapped result
        /// </returns>
        public static float GetMappedResult(float currentInput, float minInput, float maxInput, float minTarget, float maxTarget)
        {
            float diffInput = maxInput - minInput;
            float shiftInputValue = currentInput - minInput;
            float inputPercentageResult = shiftInputValue / diffInput;

            float diffTargetValue = maxTarget - minTarget;

            return minTarget + diffTargetValue * inputPercentageResult;
        }

        /// <summary>
        /// A custom approx helper respecting a certain tolerance
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="approxBuffer"></param>
        /// <returns></returns>
        public static bool Approx(float a, float b, float approxBuffer)
        {
            float diff = a - b;
            return Mathf.Abs(diff) <= approxBuffer;
        }

        /// <summary>
        /// Returns true or false at a given chance
        /// </summary>
        /// <param name="chance01">
        /// Represents the chance in percentage (0 = 0%, 1 = 100%)
        /// </param>
        /// <returns>
        /// <example>
        /// rand = 0,8; chance = 0,9 -> safe    |
        /// rand = 0,6; chance = 0,5 -> fail    |
        /// rand = 0,2; chance = 0,5 -> safe    |
        /// </example>
        /// </returns>
        public static bool RandomChanceSuccess01(float chance01)
        {
            float rand = UnityEngine.Random.Range(0f, 1f);
            return !(rand > chance01);
        }

        /// <summary>
        /// Does the calculations to look at a given target
        /// </summary>
        /// <param name="startingPos"></param>
        /// <param name="targetPos"></param>
        /// <param name="eulerOffset"></param>
        /// <returns>
        /// The <see cref="Quaternion"/> rotation to look at the <paramref name="targetPos"/>
        /// </returns>
        public static Quaternion LookAt(Vector3 startingPos, Vector3 targetPos, Vector3 eulerOffset)
        {
            Quaternion thisQuat = Quaternion.LookRotation(targetPos - startingPos);
            thisQuat.eulerAngles -= eulerOffset;

            return thisQuat;
        }
        
        public static Quaternion ClampRotation(Quaternion q, Vector3 bounds)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, -bounds.x, bounds.x);
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
            angleY = Mathf.Clamp(angleY, -bounds.y, bounds.y);
            q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

            float angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);
            angleZ = Mathf.Clamp(angleZ, -bounds.z, bounds.z);
            q.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZ);

            return q;
        }

        public static Quaternion ClampRotation(Quaternion q, Vector2 boundsX, Vector2 boundsY, Vector2 boundsZ)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, boundsX.x, boundsX.y);
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
            angleY = Mathf.Clamp(angleY, boundsY.x, boundsY.y);
            q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

            float angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);
            angleZ = Mathf.Clamp(angleZ, boundsZ.x, boundsZ.y);
            q.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZ);

            return q;
        }

        public static Quaternion ConvertGlobalToLocalRotation(Quaternion globalRotationToConvert, Transform forTransform)
        {
            Transform parent = forTransform.parent;
            
            if (parent == null)
                return globalRotationToConvert;
            
            return Quaternion.Inverse(parent.rotation) * globalRotationToConvert;
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
        /// More lightweight distance calculation than Unity's internal <see cref="Vector3"/>.<see cref="Vector3.Distance"/> calculation
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// The squared distance
        /// </returns>
        public static float DistanceSquared(Vector3 a, Vector3 b)
        {
            return (b - a).sqrMagnitude;
        }
        
        /// <summary>
        /// Is the distance between <paramref name="a"/> and <paramref name="b"/> (not squared) in the bounds of <paramref name="minMaxDistanceInclusive"/>?
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="minMaxDistanceInclusive"></param>
        /// <returns>
        /// Inbounds = true, otherwise false
        /// </returns>
        public static bool IsInAreaDist(Vector3 a, Vector3 b, Vector2 minMaxDistanceInclusive)
        {
            return IsInAreaDist((b - a).magnitude, minMaxDistanceInclusive);
        }

        /// <summary>
        /// Is the <paramref name="dist"/> (not squared) in the bounds of <paramref name="minMaxDistanceInclusive"/>?
        /// </summary>
        /// <param name="dist"></param>
        /// <param name="minMaxDistanceInclusive"></param>
        /// <returns>
        /// Inbounds = true, otherwise false
        /// </returns>
        public static bool IsInAreaDist(float dist, Vector2 minMaxDistanceInclusive)
        {
            return dist >= minMaxDistanceInclusive.x && dist <= minMaxDistanceInclusive.y;
        }

        /// <summary>
        /// Is the distance between <paramref name="a"/> and <paramref name="b"/> (squared) in the bounds of <paramref name="minMaxDistanceNotSquaredInclusive"/>?
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="minMaxDistanceNotSquaredInclusive"></param>
        /// <returns>
        /// Inbounds = true, otherwise false
        /// </returns>
        public static bool IsInAreaDistSquared(Vector3 a, Vector3 b, Vector2 minMaxDistanceNotSquaredInclusive)
        {
            return IsInAreaDistSquared(DistanceSquared(a, b), minMaxDistanceNotSquaredInclusive);
        }
        
        /// <summary>
        /// Is the <paramref name="distSquared"/> (squared) in the bounds of <paramref name="minMaxDistanceNotSquaredInclusive"/>?
        /// </summary>
        /// <param name="distSquared"></param>
        /// <param name="minMaxDistanceNotSquaredInclusive"></param>
        /// <returns>
        /// Inbounds = true, otherwise false
        /// </returns>
        public static bool IsInAreaDistSquared(float distSquared, Vector2 minMaxDistanceNotSquaredInclusive)
        {
            return distSquared >= minMaxDistanceNotSquaredInclusive.x * minMaxDistanceNotSquaredInclusive.x
                   && distSquared <= minMaxDistanceNotSquaredInclusive.y * minMaxDistanceNotSquaredInclusive.y;
        }

        /// <summary>
        /// Checks if the (<paramref name="targetPos"/> - <paramref name="originPos"/>) Vector3 direction
        /// is within the given <paramref name="maxAngle"/>, taking the <paramref name="originNormal"/> into account
        /// </summary>
        /// <param name="originPos"></param>
        /// <param name="originNormal"></param>
        /// <param name="targetPos"></param>
        /// <param name="maxAngle"></param>
        /// <returns>
        /// True if within angle, otherwise false
        /// </returns>
        public static bool IsInViewArea3D(Vector3 originPos, Vector3 originNormal, Vector3 targetPos, float maxAngle)
        {
            Vector3 inverseDirectionToTarget = targetPos - originPos;
            float angle = Vector3.Angle(originNormal, inverseDirectionToTarget);

            return (angle < maxAngle);
        }

        /// <summary>
        /// Checks if the <paramref name="targetPos"/> is within the horizontal and vertical angle bounds of the origin (caster)
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="targetPos"></param>
        /// <param name="verticalMaxAngle"></param>
        /// <param name="horizontalMaxAngle"></param>
        /// <returns>
        /// True if within angle, otherwise false
        /// </returns>
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
        /// Returns a <see cref="Vector3"/> division for each value of the given vectors
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns>
        /// The item by item divided <see cref="Vector3"/>
        /// </returns>
        public static Vector3 Divide2Vectors(Vector3 dividend, Vector3 divisor)
        {
            return new Vector3(dividend.x / divisor.x, dividend.y / divisor.y, dividend.z / divisor.z);
        }
    }
}
