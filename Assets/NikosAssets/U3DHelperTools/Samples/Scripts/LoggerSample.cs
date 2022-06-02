using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Samples
{
    public class LoggerSample : BaseNotesMono
    {
        [Button("Log \"hello\"")]
        public void Log(string text = "hello")
        {
            Debug.Log(text);
        }
    }
}
