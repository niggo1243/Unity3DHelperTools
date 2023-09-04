using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Id
{
    public abstract class IdWrapperMono : BaseNotesMono
    {
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected bool _isInverted;
        public bool IsInverted => _isInverted;

        public abstract bool ContainsId(int iD);
    }
}
