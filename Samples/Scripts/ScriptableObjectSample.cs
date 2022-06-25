using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikosAssets.Helpers.Samples
{
    [CreateAssetMenu(menuName = "Tests/" + nameof(ScriptableObjectSample), fileName = nameof(ScriptableObjectSample))]
    public class ScriptableObjectSample : BaseNotesSO
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public string sceneToCheck;
        
#if UNITY_EDITOR
        private void OnDisable()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            if (activeScene.name != null && activeScene.name.Equals(sceneToCheck))
            {
                Debug.Log(this.name + " SO Disable: ");
                Debug.Log("Application.isPlaying: " + Application.isPlaying);
                Debug.Log("EditorUtilitiesHelper.ApplicationIsPlayingAccurate: " +
                          Editor.EditorUtilitiesHelper.ApplicationIsPlayingAccurate);
            }
        }
#endif
    }
}
