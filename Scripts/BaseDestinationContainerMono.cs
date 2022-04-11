using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using NaughtyAttributes;

namespace NikosAssets.Helpers
{
    [Serializable]
    public class OnDestinationListChangedUnityEvent : UnityEvent<Component>
    {

    }

    /// <summary>
    /// A helper class that stores and offers methods for its list of generic <see cref="ComponentType"/>s
    /// and emits events if the <see cref="Destinations"/> list was modified using methods of this class
    /// </summary>
    /// <typeparam name="ComponentType"></typeparam>
    public abstract class BaseDestinationContainerMono<ComponentType> : BaseNotesMono
        where ComponentType : Component
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EVENTS)]
        public OnDestinationListChangedUnityEvent OnDestinationAddedUnityEvent, OnDestinationRemovedUnityEvent;
        public event Action<ComponentType> OnDestinationAdded, OnDestinationRemoved;
        
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_DESCRIPTIONS)]
        protected List<ComponentType> _destinations = new List<ComponentType>();

        public List<ComponentType> Destinations => _destinations;
        
        public virtual bool AddToDestinations(ComponentType comp, bool addDistinct = false)
        {
            if (comp != null)
            {
                if (addDistinct && _destinations.FindIndex(compInList => compInList == comp) > -1)
                    return false;

                _destinations.Add(comp);

                this.OnDestinationAdded?.Invoke(comp);
                this.OnDestinationAddedUnityEvent?.Invoke(comp);

                return true;
            }

            return false;
        }

        public virtual void AddMultipleToDestinations(List<ComponentType> componentTypes, bool addDistinct = false)
        {
            componentTypes.ForEach(ct => this.AddToDestinations(ct, addDistinct));
        }

        public virtual void RemoveAllFromDestinations()
        {
            for (int i = 0; i < this.Destinations.Count; i++)
            {
                ComponentType componentType = this.Destinations[i];
                if (this.RemoveFromDestinations(componentType)) --i;
            }
        }
        
        public virtual bool RemoveFromDestinations(ComponentType comp)
        {
            if (_destinations.Remove(comp))
            {
                this.OnDestinationRemoved?.Invoke(comp);
                this.OnDestinationRemovedUnityEvent?.Invoke(comp);

                return true;
            }

            return false;
        }
        
        public virtual bool RemoveFromDestinationsAt(int index)
        {
            if (index < 0 || index >= _destinations.Count) return false;

            ComponentType compToRemove = _destinations[index];
            _destinations.RemoveAt(index);
            
            this.OnDestinationRemoved?.Invoke(compToRemove);
            this.OnDestinationRemovedUnityEvent?.Invoke(compToRemove);

            return true;
        }
        
        public virtual List<ComponentType> GetDestinationsWithExcluded(List<ComponentType> excludeDestinations, bool removeFoundDoubleFromExclusiveList)
        {
            List<ComponentType> filteredList = new List<ComponentType>();
            List<ComponentType> excludeDestCopy = new List<ComponentType>(excludeDestinations);

            foreach (ComponentType comp in _destinations)
            {
                for (int i = 0; i < excludeDestCopy.Count; i++)
                {
                    if (!GameObject.ReferenceEquals(comp.gameObject, excludeDestCopy[i].gameObject))
                    {
                        filteredList.Add(comp);
                        continue;
                    }

                    if (removeFoundDoubleFromExclusiveList)
                        excludeDestCopy.RemoveAt(i--);
                }
            }

            return filteredList;
        }
    }
}
