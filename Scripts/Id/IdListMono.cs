using System.Collections.Generic;
using NaughtyAttributes;
using NikosAssets.Helpers.Interfaces;
using UnityEngine;

namespace NikosAssets.Helpers.Id
{
    public class IdListMono : BaseIdMono, IIdList
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [SerializeField]
        protected List<int> _iDs = new List<int>();

        public List<int> IDs => new List<int>(_iDs);
        
        public override bool ContainsId(int iD, bool inverted)
        {
            bool contains = _iDs.Contains(iD);
            return inverted ? !contains : contains;
        }
    }
}
