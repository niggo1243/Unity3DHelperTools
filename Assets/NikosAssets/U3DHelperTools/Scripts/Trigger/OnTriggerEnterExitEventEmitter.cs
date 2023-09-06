using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Trigger
{
    /// <summary>
    /// Helper class that emits C# and <see cref="UnityEngine.Events.UnityEvent"/>
    /// events when a trigger has been entered by colliders with <see cref="acceptedTags"/>
    /// </summary>
    public class OnTriggerEnterExitEventEmitter : BaseNotesMono
    {
        public event Action<Collider> OnColliderEntered, OnColliderExited;
        
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EVENTS)]
        public TriggerUnityEvent OnColliderEnteredUnityEvt, OnColliderExitedUnityEvt;

        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tag]
        public List<string> acceptedTags = new List<string>();

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (acceptedTags.Contains(other.tag))
            {
                OnColliderEntered?.Invoke(other);
                OnColliderEnteredUnityEvt.Invoke(other);
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (acceptedTags.Contains(other.tag))
            {
                OnColliderExited?.Invoke(other);
                OnColliderExitedUnityEvt.Invoke(other);
            }
        }
    }
}
