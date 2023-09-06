using UnityEngine;

namespace NikosAssets.Helpers.ScriptableObjectWrappers
{
    [CreateAssetMenu(
        fileName = nameof(SOWInt),
        menuName = nameof(NikosAssets) + "/" + nameof(Helpers) + "/" + nameof(ScriptableObjectWrappers) + "/" + nameof(SOWInt))]
    public class SOWInt : BaseSOW<int>
    {
    }
}
