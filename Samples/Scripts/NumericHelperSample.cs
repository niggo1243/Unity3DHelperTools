using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Samples
{
    public class NumericHelperSample : BaseNotesMono
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public Transform transA = default, transB = default;
        
        [BoxGroup("Mapping")]
        public float rawInput = .5f;
        [BoxGroup("Mapping")]
        public Vector2 rawInputBounds = new Vector2(0, 1);
        [BoxGroup("Mapping")]
        public Vector2 mapRawInputTo = new Vector2(10, 100);
        
        [ReadOnly]
        [BoxGroup("Mapping")]
        public float mappedOutputResult;

        [BoxGroup("Random Chance")]
        [Range(0, 1)]
        public float randomChance = .5f;

        [BoxGroup("RolyPoly")]
        public Rigidbody rolyPoly;
        [BoxGroup("RolyPoly")]
        public float alignmentSpeed = 3, damping = 3;
        
        [BoxGroup("Clamp Rotation")]
        public Vector3 clampAngleBounds = Vector3.zero;

        [BoxGroup("In View Area")]
        public float angle = 30;

        [BoxGroup("Divide Vectors")]
        public Vector3 vecA, vecB;

        [Button("Map the raw input to the output values")]
        public void MapRawInput()
        {
            mappedOutputResult = NumericHelper.GetMappedResult(rawInput, rawInputBounds.x, 
                rawInputBounds.y, mapRawInputTo.x, mapRawInputTo.y);
        }

        [Button("Log the success of the random chance")]
        public void RandomChance()
        {
            Debug.Log("random success with chance: " + NumericHelper.RandomChanceSuccess01(randomChance));
        }

        [Button("Clamp the rotation of transA")]
        public void CheckIfVectorIsFacing()
        {
            transA.rotation = NumericHelper.ClampRotation(transA.rotation, clampAngleBounds);
        }

        [Button("Log Is TransA in view area of TransB")]
        public void IsInViewArea()
        {
            Debug.Log("Is In View Area: " + NumericHelper.IsInViewArea3D(transB.position, transB.forward,
                transA.position, angle));
        }

        [Button("Divide the values of 2 vectors")]
        public void DivideVectorValues()
        {
            Debug.Log("Division result: " + NumericHelper.Divide2Vectors(vecA, vecB));
        }

        private void Update()
        {
            NumericHelper.CalculateTorqueRotationAlignment(rolyPoly.transform, rolyPoly, Vector3.up, alignmentSpeed, damping);
        }
    }
}
