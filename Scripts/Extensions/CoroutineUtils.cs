using System.Collections;
using UnityEngine;

namespace NikosAssets.Helpers.Extensions
{
    public static class CoroutineUtils
    {

        /// <summary>
        /// 
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
        /// Starts a Coroutine (only one instance)
        /// </summary>
        /// <param name="sender">the original sender</param>
        /// <param name="coroutine">the coroutine to stop</param>
        /// <param name="outCor">the coroutine to set</param>
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