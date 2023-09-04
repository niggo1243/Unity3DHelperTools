using NaughtyAttributes;
using NikosAssets.Helpers.Interfaces;
using UnityEngine;

namespace NikosAssets.Helpers.Id
{
    public class IdMono : IdWrapperMono, IId
    {
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected int _iD = 0;
        public int ID => _iD;
        
        public override bool ContainsId(int iD)
        {
            bool contains = _iD == iD;
            return IsInverted ? !contains : contains;
        }        
    }
}
