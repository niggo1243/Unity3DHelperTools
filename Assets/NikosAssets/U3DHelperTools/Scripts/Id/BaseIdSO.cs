using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Id
{
    /// <summary>
    /// A wrapper class for <see cref="ScriptableObject"/> <see cref="Interfaces.IId"/> or <see cref="Interfaces.IIdList"/> implementations
    /// providing iD match checks
    /// </summary>
    public abstract class BaseIdSO : BaseNotesSO
    {
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected bool _isInverted;
        public bool IsInverted => _isInverted;

        public virtual bool ContainsId(int iD)
        {
            return ContainsId(iD, IsInverted);
        }   
        
        public abstract bool ContainsId(int iD, bool inverted);
    }
}
