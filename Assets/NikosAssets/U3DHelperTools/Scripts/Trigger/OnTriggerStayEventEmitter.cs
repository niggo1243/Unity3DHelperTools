using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Trigger
{
    /// <summary>
    /// Helper class that emits C# and <see cref="UnityEngine.Events.UnityEvent"/>
    /// events when colliders with <see cref="acceptedTags"/> stay in a trigger
    /// </summary>
    public class OnTriggerStayEventEmitter : BaseNotesMono
    {
        public event Action<Collider> OnColliderStay;
        
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EVENTS)]
        public TriggerUnityEvent OnColliderStayUnityEvt;

        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tag]
        public List<string> acceptedTags = new List<string>();

        protected virtual void OnTriggerStay(Collider other)
        {
            if (acceptedTags.Contains(other.tag))
            {
                OnColliderStay?.Invoke(other);
                OnColliderStayUnityEvt.Invoke(other);
            }
        }
    }
}
