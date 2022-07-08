using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Samples
{
    [CreateAssetMenu(menuName = "Tests/" + nameof(GUIDRegenScriptableObjectSample), fileName = nameof(GUIDRegenScriptableObjectSample))]
    public class GUIDRegenScriptableObjectSample : BaseNotesSO
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_DESCRIPTIONS)]
        public List<Object> Objects;

        [Button("Regen guids only of scriptable objects")]
        public void RegenerateOnlyScriptableObjectsRecursive()
        {
            #if UNITY_EDITOR
            Editor.GUIDHelper.RegenerateGuids(Editor.EditorUtilitiesHelper.PickFolderInsideProject("Regen GUIDS", 
                "Assets/", "Assets/"), true, 
                Editor.GUIDHelper.AcceptedMetaFiles.Any, true, new []{".asset"});
            #endif
        }
    }
}
