using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers
{
    public class MoveTargetOnSurface : BaseNotesMono
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public Transform targetToMove;

        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public float multiplier = .1f;

        [SerializeField]
        [Layer]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected string _layer = "Terrain";

        protected int _tempLayer;
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

        public virtual void ResetTempLayerMask()
        {
            _tempLayer = LayerMask.GetMask(_layer);
        }
        
        public virtual Vector3 Move(Vector3 localMovement, float deltaTime)
        {
            return Move(localMovement, deltaTime, _tempLayer);
        }
        
        public virtual Vector3 Move(Vector3 localMovement, float deltaTime, int layerMask)
        {
            return Move(localMovement, multiplier * deltaTime, targetToMove, layerMask);
        }

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