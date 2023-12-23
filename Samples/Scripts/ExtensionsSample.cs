using System;
using System.Collections;
using NaughtyAttributes;
using NikosAssets.Helpers.Extensions;
using UnityEngine;

#if !UNITY_2022_2_OR_NEWER
using UnityEngine.AI;
#endif

namespace NikosAssets.Helpers.Samples
{
    public class ExtensionsSample : BaseNotesMono
    {
#if !UNITY_2022_2_OR_NEWER
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public NavMeshAgent navMeshAgent = default;
#endif
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public Transform target = default;

        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_DESCRIPTIONS)]
        public string tooLongString = "this string is too long and should be cropped please!!!";
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_DESCRIPTIONS)]
        public string cropSymbols = "...";

        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_DESCRIPTIONS)]
        public int maxStringLength = 25;

        [ReadOnly]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_DESCRIPTIONS)]
        public string cropResult = "";

        [ReadOnly]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_DESCRIPTIONS)]
        public ulong hashOfCropped;
        
        private Coroutine _coroutine;

        private void OnDestroy()
        {
            StopLoggerCoroutine();
        }

        private void Update()
        {
#if !UNITY_2022_2_OR_NEWER
            navMeshAgent.SetDestination(target.position);
#endif
        }

        [Button("Start Logger Coroutine", EButtonEnableMode.Playmode)]
        public void StartLoggerCoroutine()
        {
            this.StartAndReplaceCoroutine(ref _coroutine, Log());
        }

        [Button("Stop Logger Coroutine", EButtonEnableMode.Playmode)]
        public void StopLoggerCoroutine()
        {
            this.StopRunningCoroutine(_coroutine);
        }

        private IEnumerator Log()
        {
            while (this.isActiveAndEnabled)
            {
                Debug.Log("Logger cor running!");
                yield return new WaitForSeconds(.5f);
            }
        }

#if !UNITY_2022_2_OR_NEWER
        [Button("Get NavmeshAgent Desired Mov Speed", EButtonEnableMode.Playmode)]
        public void LogDesiredNavmeshMovSpeed()
        {
            Debug.Log("NavMeshAgent, desired speed: " + navMeshAgent.GetDesiredMovementSpeed());
        }
        
        [Button("Get NavmeshAgent Desired Turn Speed", EButtonEnableMode.Playmode)]
        public void LogDesiredNavmeshTurningSpeed()
        {
            Debug.Log("NavMeshAgent, desired speed: " + navMeshAgent.GetDesiredTuringSpeed());
        }
#endif

        [Button("Crop the too long string")]
        public void CropTooLongString()
        {
            cropResult = tooLongString.CropString(maxStringLength, cropSymbols);
        }

        [Button("Generate a hash of cropped string")]
        public void GenerateHashOfTooLongString()
        {
            hashOfCropped = cropResult.GetUInt64Hash();
        }
    }
}
