using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Samples
{
    public class EditorUtilitiesSample : BaseNotesMono
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_DESCRIPTIONS)]
        public string pathChosen = "Assets/";

        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_DESCRIPTIONS)]
        public string scriptClassAndFileName = "GeneratedScriptTest";
        
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_DESCRIPTIONS)]
        public string scriptMethodName = "CallMe";
        
        #if UNITY_EDITOR
        
        private void OnDestroy()
        {
            Debug.Log(this.name + " Mono Destroy: ");
            Debug.Log("Application.isPlaying: " + Application.isPlaying);
            Debug.Log("EditorUtilitiesHelper.ApplicationIsPlayingAccurate: " + Editor.EditorUtilitiesHelper.ApplicationIsPlayingAccurate);
        }

        [Button("Pick and save Folder path inside this project")]
        public void PickFolder()
        {
            pathChosen =
                Editor.EditorUtilitiesHelper.PickFolderInsideProject("Folder Picker Sample", pathChosen, pathChosen);
            Debug.Log(pathChosen);
        }

        [Button("Generate script at chosen path with setup field values")]
        public void GenerateScriptAtChosenPath()
        {
            Dictionary<string, string> replaceMarkingsInTemplate = new Dictionary<string, string>();
            replaceMarkingsInTemplate.Add("##CLASS_NAME##", scriptClassAndFileName);
            replaceMarkingsInTemplate.Add("##METHOD_NAME##", scriptMethodName);

            Editor.EditorUtilitiesHelper.CreateScript(pathChosen + scriptClassAndFileName + ".cs",
                "SampleScriptCreationTemplate", replaceMarkingsInTemplate);
        }
        
        #endif
    }
}
