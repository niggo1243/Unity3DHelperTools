using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.ScriptableObjectWrappers
{
    /// <summary>
    /// Wrapper class to store a serializable value in a <see cref="ScriptableObject"/>
    /// </summary>
    /// <typeparam name="T">A serializable value that has to be defined in the subclasses</typeparam>
    public abstract class BaseSOW<T> : BaseSOWWrapper
    {
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected T _value;

        public virtual T GetValue() => _value;

        public virtual void SetValue(T newVal) => _value = newVal;
    }
}
