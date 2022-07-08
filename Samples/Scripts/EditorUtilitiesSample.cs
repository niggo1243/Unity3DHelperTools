using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NikosAssets.Helpers.Samples
{
    public class EditorUtilitiesSample : BaseNotesMono
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public string fileNameToReplace = "_RENAME_TEST_1";

        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public string desiredFileName = "_RENAME_TEST_2";

        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public bool regenGuidsRecursive = true;

#if UNITY_EDITOR
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public Editor.GUIDHelper.AcceptedMetaFiles acceptedMetaFilesForGuidRegen =
            Editor.GUIDHelper.AcceptedMetaFiles.Any;
#endif

        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_DESCRIPTIONS)]
        public string pathChosen = "Assets/";

        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_DESCRIPTIONS)]
        public string scriptClassAndFileName = "GeneratedScriptTest";

        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_DESCRIPTIONS)]
        public string scriptMethodName = "CallMe";

        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_DESCRIPTIONS)]
        public Object referenceForGUIDRegen;

#if UNITY_EDITOR

        private void OnDestroy()
        {
            Debug.Log(this.name + " Mono Destroy: ");
            Debug.Log("Application.isPlaying: " + Application.isPlaying);
            Debug.Log("EditorUtilitiesHelper.ApplicationIsPlayingAccurate: " +
                      Editor.EditorUtilitiesHelper.ApplicationIsPlayingAccurate);
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

        [Button("Log files from chosen path recursive")]
        public void LogFilesFromChosenPathRecursive()
        {
            CollectionHelper.LogCollection(Editor.EditorUtilitiesHelper.GetFiles(pathChosen));
        }

        [Button("Rename Files at chosen path")]
        public void RenameFilesAtChosenPath()
        {
            Editor.EditorUtilitiesHelper.RenameFiles(pathChosen,
                fileNameToReplace, desiredFileName);
        }

        [Button("Regenerate GUIDs recursive and keep the file references at the chosen path")]
        public void RegenerateGUIDsAtChosenPathRecursive()
        {
            Editor.GUIDHelper.RegenerateGuids(pathChosen, regenGuidsRecursive, acceptedMetaFilesForGuidRegen);
        }
#endif
    }
}
