namespace NikosAssets.Helpers.ScriptableObjectWrappers
{
    /// <summary>
    /// A base SOW that wraps other SOWs so that it can be assigned in the inspector, since the inspector does not support unset generics in an abstract class header
    /// </summary>
    public abstract class BaseSOWWrapper : BaseNotesSO
    {
    }
}
