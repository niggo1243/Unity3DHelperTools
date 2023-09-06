using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Id
{
    [CreateAssetMenu(
        fileName = nameof(IdSO),
        menuName = nameof(NikosAssets) + "/" + nameof(Helpers) + "/" + nameof(Id) + "/" + nameof(IdSO))]
    public class IdSO : BaseIdSO
    {
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected int _iD = 0;
        public int ID => _iD;

        public override bool ContainsId(int iD, bool inverted)
        {
            bool contains = _iD == iD;
            return inverted ? !contains : contains;
        }  
    }
}
