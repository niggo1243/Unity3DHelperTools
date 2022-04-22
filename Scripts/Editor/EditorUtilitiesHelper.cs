using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace NikosAssets.Helpers.Editor
{
    /// <summary>
    /// A helper class for Editor only usage
    /// </summary>
    public static class EditorUtilitiesHelper
    {
        private static bool _applicationPlayingAccurate;
        /// <summary>
        /// <see cref="Application.isPlaying"/> is not accurate for the <see cref="ScriptableObject"/>
        /// (see <a href="https://docs.unity3d.com/ScriptReference/Application-isPlaying.html">Unity's API</a>).
        /// Use this instead for greater accuracy!
        /// </summary>
        public static bool ApplicationIsPlayingAccurate => _applicationPlayingAccurate;
        
        [InitializeOnLoadMethod]
        public static void InitEditor()
        {
            try
            {
                EditorApplication.playModeStateChanged -= EditorApplicationOnplayModeStateChanged;
            }
            catch
            {
                //ignored
            }

            EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
        }

        private static void EditorApplicationOnplayModeStateChanged(PlayModeStateChange state)
        {
            _applicationPlayingAccurate = state != PlayModeStateChange.EnteredEditMode &&
                                          state != PlayModeStateChange.ExitingPlayMode;
        }

        /// <summary>
        /// Restrict the returning path to be within this Unity project
        /// </summary>
        /// <param name="folderPickerTitle">
        /// The title of the file picker window
        /// </param>
        /// <param name="desiredPath">
        /// The path to open in the file picker window
        /// </param>
        /// <param name="localProjectPathOnAbort">
        /// If the file picker was aborted (string is null or empty), return this path
        /// </param>
        /// <returns>
        /// The picked project path on success, otherwise logs errors and returns "Assets/" as default path or if aborted, returns <paramref name="localProjectPathOnAbort"/>
        /// </returns>
        public static string PickFolderInsideProject(string folderPickerTitle, string desiredPath, string localProjectPathOnAbort)
        {
            string path = EditorUtility.OpenFolderPanel(folderPickerTitle, desiredPath, "");
            
            if (!string.IsNullOrEmpty(path))
            {
                if (!path.Contains(Application.dataPath))
                    Debug.LogError("Path must be within the Unity Project!");

                try
                {
                    path = "Assets" + path.Substring(Application.dataPath.Length) + "/";
                    return path;
                }
                catch
                {
                    Debug.LogError("Path must be within the Unity Project!");
                    path = "Assets/";
                }
            }
            //if string was null, the folder picker was aborted
            else
            {
                path = localProjectPathOnAbort;
            }

            return path;
        }
        
        /// <summary>
        /// Create a script using a (.txt) template file, replacing its contents with the key value pairs in <paramref name="replaceContentsDict"/>
        /// </summary>
        /// <param name="pathName">
        /// The script creation path (must include ".cs" or any other desired fileEnding postfix)
        /// </param>
        /// <param name="templateFileName">
        /// The template file to convert (must include fileEnding as well)
        /// </param>
        /// <param name="replaceContentsDict">
        /// What contents to replace in the template for the script file conversion?
        /// <example>
        /// key: "#CLASSNAME#", value: "HelloWorldClass"
        /// </example>
        /// </param>
        /// <returns>
        /// The created script file
        /// </returns>
        public static Object CreateScript(string pathName, string templateFileName, Dictionary<string, string> replaceContentsDict)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(templateFileName);
            if (textAsset != null)
            {
                UTF8Encoding encoding = new UTF8Encoding(true, false);
                string resultText = textAsset.text;

                //replace text snippets
                foreach (KeyValuePair<string, string> kvp in replaceContentsDict)
                    resultText = resultText.Replace(kvp.Key, kvp.Value);

                StreamWriter writer = new StreamWriter(Path.GetFullPath(pathName), false, encoding);
                writer.Write(resultText);
                writer.Close();

                AssetDatabase.ImportAsset(pathName);
                AssetDatabase.SaveAssets();
                return AssetDatabase.LoadAssetAtPath(pathName, typeof(Object));
            }
            else
            {
                Debug.LogError(string.Format("The template file was not found: {0}", templateFileName));
                return null;
            }
        }
    }
}