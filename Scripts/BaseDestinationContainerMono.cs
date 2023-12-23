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
    /// <typeparam name="ComponentType">
    /// The type stored in the <see cref="Destinations"/> list.
    /// </typeparam>
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
        
        /// <summary>
        /// Adds the given type only if it is not null and either if no duplicate is found or if "<paramref name="addDistinct"/>" is false.
        /// Emits the <see cref="OnDestinationAddedUnityEvent"/> and <see cref="OnDestinationAdded"/> events on success.
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="addDistinct"></param>
        /// <returns>
        /// false if couldn't add, otherwise true
        /// </returns>
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

        /// <summary>
        /// A helper method to add multiple <see cref="ComponentType"/>s to the <see cref="Destinations"/> list,
        /// also emitting the <see cref="OnDestinationAddedUnityEvent"/> and <see cref="OnDestinationAdded"/> events.
        /// </summary>
        /// <param name="componentTypes"></param>
        /// <param name="addDistinct">Are duplicates allowed?</param>
        public virtual void AddMultipleToDestinations(List<ComponentType> componentTypes, bool addDistinct = false)
        {
            componentTypes.ForEach(ct => this.AddToDestinations(ct, addDistinct));
        }

        /// <summary>
        /// Removes all items from the <see cref="Destinations"/> list and emits
        /// the <see cref="OnDestinationRemovedUnityEvent"/> and <see cref="OnDestinationRemoved"/> events.
        /// </summary>
        public virtual void RemoveAllFromDestinations()
        {
            for (int i = 0; i < this.Destinations.Count; i++)
            {
                ComponentType componentType = this.Destinations[i];
                if (this.RemoveFromDestinations(componentType)) --i;
            }
        }
        
        /// <summary>
        /// Removes the given type and
        /// emits the <see cref="OnDestinationRemovedUnityEvent"/> and <see cref="OnDestinationRemoved"/> events on success.
        /// </summary>
        /// <param name="comp"></param>
        /// <returns>
        /// false if couldn't remove, otherwise true
        /// </returns>
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
        
        /// <summary>
        /// Remove and emit events at index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>true on removal success, otherwise false</returns>
        public virtual bool RemoveFromDestinationsAt(int index)
        {
            if (index < 0 || index >= _destinations.Count) return false;

            ComponentType compToRemove = _destinations[index];
            _destinations.RemoveAt(index);
            
            this.OnDestinationRemoved?.Invoke(compToRemove);
            this.OnDestinationRemovedUnityEvent?.Invoke(compToRemove);

            return true;
        }

        #region Obsolete Methods

        [Obsolete("This method will be removed and a similar one might appear in the CollectionsHelper class")]
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
        
        #endregion

    }
}
