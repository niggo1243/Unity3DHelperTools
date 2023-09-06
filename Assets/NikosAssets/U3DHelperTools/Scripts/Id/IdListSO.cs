using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Id
{
    [CreateAssetMenu(
        fileName = nameof(IdListSO),
        menuName = nameof(NikosAssets) + "/" + nameof(Helpers) + "/" + nameof(Id) + "/" + nameof(IdListSO))]
    public class IdListSO : BaseIdSO
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
