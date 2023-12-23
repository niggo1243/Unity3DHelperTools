using UnityEngine;

namespace NikosAssets.Helpers.ScriptableObjectWrappers
{
    [CreateAssetMenu(
        fileName = nameof(SOWFloat),
        menuName = nameof(NikosAssets) + "/" + nameof(Helpers) + "/" + nameof(ScriptableObjectWrappers) + "/" + nameof(SOWFloat))]
    public class SOWFloat : BaseSOW<float>
    {
    }
}
