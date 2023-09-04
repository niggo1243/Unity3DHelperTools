using UnityEngine;

namespace NikosAssets.Helpers.ScriptableObjectWrappers
{
    [CreateAssetMenu(
        fileName = nameof(SOWBool),
        menuName = nameof(NikosAssets) + "/" + nameof(ScriptableObjectWrappers) + "/" + nameof(SOWBool))]
    public class SOWBool : BaseSOW<bool>
    {
    }
}
