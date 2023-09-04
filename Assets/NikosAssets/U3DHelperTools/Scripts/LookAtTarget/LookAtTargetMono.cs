namespace NikosAssets.Helpers.LookAtTarget
{
    /// <summary>
    /// Looks at the <see cref="target"/> with clamped euler values (if setup)
    /// </summary>
    public class LookAtTargetMono : BaseLookAtTargetMono
    {
        protected virtual void Update()
        {
            Tick();
        }
    }
}
