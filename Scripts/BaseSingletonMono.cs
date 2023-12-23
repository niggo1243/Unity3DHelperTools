using UnityEngine;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// A singleton wrapper class for <see cref="MonoBehaviour"/>s
    /// </summary>
    /// <typeparam name="T">The custom <see cref="MonoBehaviour"/></typeparam>
    public abstract class BaseSingletonMono<T> : BaseNotesMono
        where T : BaseSingletonMono<T>
    {
        public static T Instance { get; protected set; }
        
        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Instance of " + typeof(T).Name + " already assigned for: " + Instance.name + " !!");
                return;
            }
            
            Instance = (T)this;
        }
    }
}
