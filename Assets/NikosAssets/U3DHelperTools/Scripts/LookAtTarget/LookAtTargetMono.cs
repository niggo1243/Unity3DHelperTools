namespace NikosAssets.Helpers.LookAtTarget
{
    /// <summary>
    /// Looks at the target via the <see cref="Update"/> method
    /// </summary>
    public class LookAtTargetMono : BaseLookAtTargetMono
    {
        protected virtual void Update()
        {
            Tick();
        }
    }
}
