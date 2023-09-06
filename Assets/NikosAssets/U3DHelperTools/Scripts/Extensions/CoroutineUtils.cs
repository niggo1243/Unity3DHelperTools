using System.Collections;
using UnityEngine;

namespace NikosAssets.Helpers.Extensions
{
    /// <summary>
    /// A <see cref="Coroutine"/> extension helper class
    /// </summary>
    public static class CoroutineUtils
    {
        /// <summary>
        /// Stop the "<paramref name="cor"/>" running in this sender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cor"></param>
        public static void StopRunningCoroutine(this MonoBehaviour sender, Coroutine cor)
        {
            if (cor != null)
            {
                sender.StopCoroutine(cor);
            }
        }

        /// <summary>
        /// Starts a new <see cref="Coroutine"/> (only one instance) by stopping "<paramref name="coroutine"/>" before starting it up again
        /// </summary>
        /// <param name="sender">the original sender</param>
        /// <param name="coroutine">the coroutine to stop (and restart)</param>
        /// <param name="enumeratorToStart">the method to start</param>
        public static Coroutine StartAndReplaceCoroutine(this MonoBehaviour sender, ref Coroutine coroutine, IEnumerator enumeratorToStart)
        {
            //stop the cor
            StopRunningCoroutine(sender, coroutine);

            if (!sender.gameObject.activeInHierarchy)
                return coroutine = null;
            
            return coroutine = sender.StartCoroutine(enumeratorToStart);
        }
    }

}