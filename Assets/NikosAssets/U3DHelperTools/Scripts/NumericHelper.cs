using System;
using NikosAssets.Helpers.Extensions;
using UnityEngine;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// Contains some helpful math operations for <see cref="Vector3"/>s and <see cref="Quaternion"/>s (and other calculations as well)
    /// </summary>
    public static class NumericHelper
    {
        /// <summary>
        /// Helper enum for <see cref="NumericHelper.GetAmountValidation"/>
        /// </summary>
        public enum AmountFilter
        {
            Irrelevant = 0,
            HasNone = 1,
            HasSome = 2,
            //HasNegative = 3
        }
        
        /// <summary>
        /// Useful for slider-like behaviour that snaps a float every <see cref="snapSize"/>
        /// </summary>
        /// <param name="currentValue">
        /// The current (old) value
        /// </param>
        /// <param name="desiredValue">
        /// The desired value we will likely snap
        /// </param>
        /// <param name="snapSize">
        /// Snap the "<paramref name="desiredValue"/>" by for example ".1" or "10" or ".18"
        /// </param>
        /// <param name="snapTolerance">
        /// If the diff between "<paramref name="currentValue"/>" and "<paramref name="desiredValue"/>" is smaller than this,
        /// return the "<paramref name="currentValue"/>"
        /// </param>
        /// <returns>The new snapped "<paramref name="desiredValue"/>" or the old "<paramref name="currentValue"/>"</returns>
        public static float Snap(float currentValue, float desiredValue, float snapSize, float snapTolerance = .5f)
        {
            float valueDiff = desiredValue - currentValue;
            if (Mathf.Abs(valueDiff) < (snapSize * snapTolerance))
                return currentValue;
                    
            return Snap(desiredValue, snapSize);
        }
        
        /// <summary>
        /// Useful for slider-like behaviour that snaps a float every <see cref="snapSize"/>
        /// </summary>
        /// <param name="currentValue">
        /// The current (old) value
        /// </param>
        /// <param name="snapSize">
        /// Snap the "<paramref name="currentValue"/>" by for example ".1" or "10" or ".18"
        /// </param>
        /// <returns>The new snapped "<paramref name="currentValue"/>" that has at first been rounded to an int
        /// and afterwards multiplied by "<paramref name="snapSize"/>"</returns>
        public static float Snap(float currentValue, float snapSize)
        {
            return (snapSize * Mathf.RoundToInt(currentValue / snapSize));
        }

        /// <summary>
        /// A helper method useful to filter listviews for example
        /// </summary>
        /// <param name="amountFilter"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
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
        /// Returns a value somewhere between the x and y value of the <see cref="Vector2"/> (inclusive)
        /// </summary>
        /// <param name="minMax">The boundaries</param>
        /// <returns>A value somewhere between the x and y value (inclusive)</returns>
        public static float GetRandomFloatFromMinMaxVector(Vector2 minMax)
        {
            return UnityEngine.Random.Range(minMax.x, minMax.y);
        }
        
        /// <summary>
        /// Reflect the "<paramref name="source"/>" at the given "<paramref name="normal"/>"
        /// </summary>
        /// <param name="source"><see cref="Quaternion"/></param>
        /// <param name="normal"><see cref="Vector3"/></param>
        /// <returns>The reflected <see cref="Quaternion"/></returns>
        public static Quaternion ReflectRotation(Quaternion source, Vector3 normal)
        {
            return Quaternion.LookRotation(Vector3.Reflect(source * Vector3.forward, normal), Vector3.Reflect(source * Vector3.up, normal));
        }

        /// <summary>
        /// Does the calculations to look at a given target with the "<paramref name="alignWithTargetsUp"/>" option
        /// </summary>
        /// <param name="from">Starting <see cref="Transform"/></param>
        /// <param name="to">Has to look at <see cref="Transform"/></param>
        /// <param name="alignWithTargetsUp">Should the "<paramref name="from"/>" <see cref="Transform"/>
        /// align with the up normal of the "<paramref name="to"/>" <see cref="Transform"/>?</param>
        /// <returns>
        /// The <see cref="Quaternion"/> rotation to look at the "<paramref name="to"/>" <see cref="Transform"/>
        /// </returns>
        public static Quaternion LookAt(Transform from, Transform to, bool alignWithTargetsUp)
        {
            Quaternion rot = LookAt(to.position, from.position, Vector3.zero);
            if (alignWithTargetsUp)
            {
                rot = Quaternion.LookRotation(rot * Vector3.forward, to.up);
            }

            return rot;
        }
        
        /// <summary>
        /// Does the calculations to look at a given target
        /// </summary>
        /// <param name="startingPos"></param>
        /// <param name="targetPos"></param>
        /// <param name="eulerOffset"></param>
        /// <returns>
        /// The <see cref="Quaternion"/> rotation to look at the "<paramref name="targetPos"/>"
        /// </returns>
        public static Quaternion LookAt(Vector3 startingPos, Vector3 targetPos, Vector3 eulerOffset)
        {
            Quaternion thisQuat = Quaternion.LookRotation(targetPos - startingPos);
            thisQuat.eulerAngles -= eulerOffset;

            return thisQuat;
        }
        
        /// <summary>
        /// Clamps the given quaternion rotation by the given Vector3 angles for each axis
        /// </summary>
        /// <param name="q">The rotation to clamp</param>
        /// <param name="bounds">The x,y,z axis to clamp the rotation</param>
        /// <returns>A clamped Quaternion rotation</returns>
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

        /// <summary>
        /// Clamps the given quaternion rotation by the given Vector2 minmax angles for each axis
        /// </summary>
        /// <param name="q">The rotation to clamp</param>
        /// <param name="boundsX"></param>
        /// <param name="boundsY"></param>
        /// <param name="boundsZ"></param>
        /// <returns>A clamped Quaternion rotation</returns>
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

        /// <summary>
        /// Converts the given global <see cref="Quaternion"/> to a local one relative to the given "<paramref name="forTransform"/>"
        /// </summary>
        /// <param name="globalRotationToConvert"></param>
        /// <param name="forTransform"></param>
        /// <returns></returns>
        public static Quaternion ConvertGlobalToLocalRotation(Quaternion globalRotationToConvert, Transform forTransform)
        {
            Transform parent = forTransform.parent;
            
            if (parent == null)
                return globalRotationToConvert;
            
            return Quaternion.Inverse(parent.rotation) * globalRotationToConvert;
        }

        /// <summary>
        /// Roly-poly like behaviour for a rigidbody 
        /// </summary>
        /// <param name="transform">
        /// The transform associated with the "<paramref name="rigidbody"/>"
        /// </param>
        /// <param name="rigidbody">
        /// The <see cref="Rigidbody"/> we want to align
        /// </param>
        /// <param name="direction">
        /// The direction to align to
        /// </param>
        /// <param name="alignmentSpeed">
        /// How fast should the rigidbody jump up
        /// </param>
        /// <param name="alignmentDamping">
        /// The lower this value, the more the rigidbody can overshoot
        /// </param>
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
        /// Is the distance between "<paramref name="a"/>" and "<paramref name="b"/>" (not squared)
        /// in the bounds of "<paramref name="minMaxDistanceInclusive"/>"?
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
        /// Is the "<paramref name="dist"/>" (not squared) in the bounds of "<paramref name="minMaxDistanceInclusive"/>"?
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
        /// Is the distance between "<paramref name="a"/>" and "<paramref name="b"/>" (squared)
        /// in the bounds of "<paramref name="minMaxDistanceNotSquaredInclusive"/>"?
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
        /// Is the "<paramref name="distSquared"/>" (squared) in the bounds of "<paramref name="minMaxDistanceNotSquaredInclusive"/>"?
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
        /// Checks if the ("<paramref name="targetPos"/>" - "<paramref name="originPos"/>") Vector3 direction
        /// is within the given "<paramref name="maxAngle"/>", taking the "<paramref name="originNormal"/>" into account
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
        /// Checks if the "<paramref name="targetPos"/>" is within the horizontal and vertical angle bounds of the origin (caster)
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
        
        /// <summary>
        /// Helper method that indicates if the <see cref="Transform"/> "<paramref name="a"/>" looks at the
        /// <see cref="Vector3"/> position of "<paramref name="bPos"/>" with the given "<paramref name="withinViewAngle"/>" tolerance
        /// </summary>
        /// <param name="a"><see cref="Transform"/></param>
        /// <param name="bPos"><see cref="Vector3"/></param>
        /// <param name="withinViewAngle">The tolerance</param>
        /// <returns>true if "<paramref name="a"/>" is looking at "<paramref name="bPos"/>", otherwise false</returns>
        public static bool IsALookingAtB(Transform a, Vector3 bPos, float withinViewAngle)
        {
            return IsInViewArea3D(a.position, a.forward, bPos, withinViewAngle);
        }
        
        /// <summary>
        /// Helper method that indicates if the <see cref="Transform"/> "<paramref name="a"/>" looks at the
        /// <see cref="Vector3"/> position of "<paramref name="bPos"/>" with the given "<paramref name="withinViewAngle"/>" tolerance
        /// by also taking into account that both are set to be on the same height (simulated in this method)
        /// </summary>
        /// <param name="a"><see cref="Transform"/></param>
        /// <param name="bPos"><see cref="Vector3"/></param>
        /// <param name="withinViewAngle">The tolerance</param>
        /// <returns>true if "<paramref name="a"/>" is looking at "<paramref name="bPos"/>" when they would be at the same height, otherwise false</returns>
        public static bool IsALookingAtBSameHeight(Transform a, Vector3 bPos, float withinViewAngle)
        {
            Vector3 aPos = a.position;
            return IsInViewArea3D(aPos, a.forward, 
                //set position of a to be the same height as b, so that the angle is always valid when looking in the right dir
                bPos.GetWithNewY(aPos.y), withinViewAngle);
        }

        #region Obsolete Methods

        /// <summary>
        /// Returns a <see cref="Vector2"/> division for each value of the given vectors
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns>
        /// The item by item divided <see cref="Vector2"/>
        /// </returns>
        [Obsolete("Method has moved to Extensions.VectorUtils.Divide(...) and will be deleted here in future updates. You can now just use Vector.Divide(otherVector)")]
        public static Vector2 Divide2Vectors(Vector2 dividend, Vector2 divisor)
        {
            return dividend.Divide(divisor);
        }

        /// <summary>
        /// Returns a <see cref="Vector3"/> division for each value of the given vectors
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns>
        /// The item by item divided <see cref="Vector3"/>
        /// </returns>
        [Obsolete("Method has moved to Extensions.VectorUtils.Divide(...) and will be deleted here in future updates. You can now just use Vector.Divide(otherVector)")]
        public static Vector3 Divide2Vectors(Vector3 dividend, Vector3 divisor)
        {
            return dividend.Divide(divisor);
        }
        
        /// <summary>
        /// Returns a <see cref="Vector4"/> division for each value of the given vectors
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns>
        /// The item by item divided <see cref="Vector4"/>
        /// </returns>
        [Obsolete("Method has moved to Extensions.VectorUtils.Divide(...) and will be deleted here in future updates. You can now just use Vector.Divide(otherVector)")]
        public static Vector4 Divide2Vectors(Vector4 dividend, Vector4 divisor)
        {
            return dividend.Divide(divisor);
        }
        
        #endregion
    }
}
