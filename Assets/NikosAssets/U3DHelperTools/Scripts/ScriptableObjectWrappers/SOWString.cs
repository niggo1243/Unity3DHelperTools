using UnityEngine;

namespace NikosAssets.Helpers.ScriptableObjectWrappers
{
    [CreateAssetMenu(
        fileName = nameof(SOWString),
        menuName = nameof(NikosAssets) + "/" + nameof(Helpers) + "/" + nameof(ScriptableObjectWrappers) + "/" + nameof(SOWString))]
    public class SOWString : BaseSOW<string>
    {
    }
}