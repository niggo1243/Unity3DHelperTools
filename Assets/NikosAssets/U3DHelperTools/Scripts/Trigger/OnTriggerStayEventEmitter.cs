using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Trigger
{

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
