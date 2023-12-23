using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// A helper class to move the <see cref="targetToMove"/> on an even or uneven surface at the same speed no matter how high or low the slope is
    /// </summary>
    public class MoveTargetOnSurface : BaseNotesMono
    {
        /// <summary>
        /// The target to move
        /// </summary>
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("The target to move")]
        public Transform targetToMove;

        /// <summary>
        /// The speed to move the target
        /// </summary>
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("The speed to move the target")]
        public float multiplier = .1f;

        /// <summary>
        /// Only allow movement on those (collider) layers
        /// </summary>
        [SerializeField]
        [Layer]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("Only allow movement on those (collider) layers")]
        protected string _layer = "Terrain";

        /// <summary>
        /// The layermask of the <see cref="_layer"/> names
        /// </summary>
        protected int _tempLayer;
        /// <summary>
        /// The layermask of the <see cref="_layer"/> names
        /// </summary>
        public int TempLayer
        {
            get => _tempLayer;
            set
            {
                _tempLayer = value;
                ResetTempLayerMask();
            }
        }

        protected virtual void Start()
        {
            ResetTempLayerMask();
        }

        protected virtual void OnValidate()
        {
            ResetTempLayerMask();
        }

        /// <summary>
        /// Convert the <see cref="_layer"/> names into the layermask and update the <see cref="TempLayer"/>
        /// </summary>
        public virtual void ResetTempLayerMask()
        {
            _tempLayer = LayerMask.GetMask(_layer);
        }
        
        /// <summary>
        /// Move the <see cref="targetToMove"/> along the surface with the set masks in <see cref="_layer"/>
        /// </summary>
        /// <param name="localMovement">The local direction relative to <see cref="targetToMove"/> to move along</param>
        /// <param name="deltaTime">Will be multiplied with <see cref="multiplier"/></param>
        /// <returns>The movement translation</returns>
        public virtual Vector3 Move(Vector3 localMovement, float deltaTime)
        {
            return Move(localMovement, deltaTime, _tempLayer);
        }

        /// <summary>
        /// Move the <see cref="targetToMove"/> along the surface
        /// </summary>
        /// <param name="localMovement">The local direction relative to <see cref="targetToMove"/> to move along</param>
        /// <param name="deltaTime">Will be multiplied with <see cref="multiplier"/></param>
        /// <param name="layerMask">What surface is supported?</param>
        /// <returns>The movement translation</returns>
        public virtual Vector3 Move(Vector3 localMovement, float deltaTime, int layerMask)
        {
            return Move(localMovement, multiplier * deltaTime, targetToMove, layerMask);
        }

        /// <summary>
        /// Move the "<paramref name="target"/>" along the surface
        /// </summary>
        /// <param name="localMovement">The local direction relative to "<paramref name="target"/>" to move along</param>
        /// <param name="multiplier">The speed (and deltaTime) to move the "<paramref name="target"/>""</param>
        /// <param name="target">The <see cref="Transform"/> to move along the surface</param>
        /// <param name="layerMask">What surface is supported?</param>
        /// <returns>The movement translation</returns>
        public static Vector3 Move(Vector3 localMovement, float multiplier, 
            Transform target, int layerMask)
        {
            //the movedir/ force should be relative to the current terrain slope
            Quaternion slopeCorrection = Quaternion.identity;
            Vector3 rayShootSrcOffset = (Vector3.up * 5);
            
            if (Physics.Raycast(target.position + rayShootSrcOffset, Vector3.down,
                out RaycastHit terrainHit, Mathf.Infinity, layerMask))
            {
                slopeCorrection = Quaternion.FromToRotation(Vector3.up, terrainHit.normal);
            }

            Vector3 finalDir = slopeCorrection * target.localRotation * localMovement;

            //Debug.DrawRay(targetToMove.position, finalMoveDir * 20, Color.red, Time.fixedDeltaTime);
            target.Translate(finalDir * multiplier, Space.World);

            return finalDir;
        }
    }
}