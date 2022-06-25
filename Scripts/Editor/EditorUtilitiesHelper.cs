using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

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
        private static void InitEditor()
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
        /// The picked LOCAL project path on success, otherwise logs errors and returns "Assets/" as default path or if aborted, returns <paramref name="localProjectPathOnAbort"/>
        /// </returns>
        public static string PickFolderInsideProject(string folderPickerTitle, string desiredPath, string localProjectPathOnAbort)
        {
            string path = EditorUtility.OpenFolderPanel(folderPickerTitle, desiredPath, "");
            
            if (!string.IsNullOrEmpty(path))
            {
                if (!path.Contains(Application.dataPath))
                {
                    Debug.LogError("Path must be within the Unity Project!");
                    return "";
                }
                
                path = "Assets" + path.Substring(Application.dataPath.Length) + "/";
                return path;
            }
            
            //if string was null, the folder picker was aborted
            return localProjectPathOnAbort;
        }
        
        /// <summary>
        /// Create a script using a (.txt) template file, replacing its contents with the key value pairs in <paramref name="replaceContentsDict"/>
        /// </summary>
        /// <param name="localFilePath">
        /// The script creation path (must include ".cs" or any other desired fileEnding suffix)
        /// </param>
        /// <param name="templateFileName">
        /// The template file to convert
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
        public static Object CreateScript(string localFilePath, string templateFileName, Dictionary<string, string> replaceContentsDict)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(templateFileName);
            if (textAsset != null)
            {
                UTF8Encoding encoding = new UTF8Encoding(true, false);
                string resultText = textAsset.text;

                //replace text snippets
                foreach (KeyValuePair<string, string> kvp in replaceContentsDict)
                    resultText = resultText.Replace(kvp.Key, kvp.Value);

                StreamWriter writer = new StreamWriter(Path.GetFullPath(localFilePath), false, encoding);
                writer.Write(resultText);
                writer.Close();

                AssetDatabase.ImportAsset(localFilePath);
                AssetDatabase.SaveAssets();
                return AssetDatabase.LoadAssetAtPath(localFilePath, typeof(Object));
            }
            else
            {
                Debug.LogError(string.Format("The template file was not found: {0}", templateFileName));
                return null;
            }
        }

        /// <summary>
        /// Regenerate GUIDs of the files found from the picked folder and keep all file references within this project
        /// </summary>
        /// <param name="recursive">
        /// Go into sub folders as well?
        /// </param>
        public static void PickFolderAndRegenerateGUIDs(bool recursive)
        {
            string path = PickFolderInsideProject("Regenerate GUIDs in folder", "Assets/", null);

            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning("Regenerating GUIDs aborted or failed");
                return;
            }
            
            GUIDHelper.RegenerateGuids(path, recursive);
        }

        /// <summary>
        /// Get files at the given <paramref name="localRootPath"/>
        /// </summary>
        /// <param name="localRootPath">
        /// The local project path to start gathering the files
        /// </param>
        /// <param name="recursive">
        /// Go further into sub folders?
        /// </param>
        /// <param name="forExtensions">
        /// Only gather files with the given extensions, for example ".cs, .mat, .asset"
        /// </param>
        /// <param name="containsName">
        /// Only for files that contain the given string, for example "_albedo"
        /// </param>
        /// <returns>
        /// A list of filePaths (strings)
        /// </returns>
        public static List<string> GetFiles(string localRootPath, bool recursive = true, string[] forExtensions = null,
            string containsName = "")
        {
            localRootPath = string.IsNullOrEmpty(localRootPath) ? "Assets/" : localRootPath;
            string globalPath = Path.GetFullPath(localRootPath);

            // Get the list of working files
            List<string> allFilesPaths = new List<string>();
            if (forExtensions == null)
            {
                forExtensions = new[] {"*.*"};
            }

            foreach (string extension in forExtensions)
            {
                allFilesPaths.AddRange(
                    Directory.GetFiles(
                            globalPath,
                            "*" + extension,
                            recursive ?
                            SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                        .Where(p => p.Contains(containsName))
                        .Select(p => p.Replace(@"\", "/")));
            }


            return allFilesPaths;
        }

        /// <summary>
        /// Renames the files at the given local path and keeps all references (same GUIDs)
        /// </summary>
        /// <param name="localRootPath">
        /// The local path to start the search
        /// </param>
        /// <param name="oldFileNamePart">
        /// The part of the files we want to rename
        /// </param>
        /// <param name="newFileNamePart">
        /// This will replace <paramref name="oldFileNamePart"/> of each found file
        /// </param>
        /// <param name="recursive">
        /// Should we go into sub folders?
        /// </param>
        /// <param name="forExtensions">
        /// Only for specific file types?
        /// <example>
        /// .mat, .asset
        /// </example>
        /// </param>
        public static void RenameFilesRecursive(string localRootPath, string oldFileNamePart, string newFileNamePart, 
            bool recursive = true, string[] forExtensions = null)
        {
            // Get the list of working files
            List<string> allFilesPaths = GetFiles(localRootPath, recursive, forExtensions, oldFileNamePart);

            allFilesPaths.ForEach(path =>
            {
                try
                {
                    string file = Path.GetFileName(path);
                    file = file.Replace(oldFileNamePart, newFileNamePart);
                    string newPath = Path.GetDirectoryName(path).Replace(@"\", "/") + "/" + file;
                    
                    //delete any file that would collide with the move TODO hint to the user that this is the case (or abort on collide?)
                    if (File.Exists(newPath)) File.Delete(newPath);
                    File.Move(path, newPath);
                    //Should never happen but in case the AssetDatabase reimports the old asset (for savety)
                    if (File.Exists(path)) File.Delete(path);
                }
                catch (Exception e)
                {
                    Debug.LogError(path + " \n error:" +
                                   e.Message);
                }
            });
            
            AssetDatabase.Refresh();
        }

        #region Menu Items

        [MenuItem("Tools/Helpers/RegenerateGUIDsOfPickedFolder/Recursive")]
        private static void RegenerateGUIDsRecursive()
        {
            if (EditorUtility.DisplayDialog("GUIDs regeneration recursive",
                "First a folder picker will appear and after that the process of GUID regeneration will begin. " +
                "\n\nMake a backup of your project beforehand!",
                "Regenerate GUIDs recursive", "Cancel"))
            {
                PickFolderAndRegenerateGUIDs(true);
            }
        }
        
        [MenuItem("Tools/Helpers/RegenerateGUIDsOfPickedFolder/NonRecursive")]
        private static void RegenerateGUIDsNonRecursive()
        {
            if (EditorUtility.DisplayDialog("GUIDs regeneration non-recursive",
                "First a folder picker will appear and after that the process of GUID regeneration will begin. " +
                "\n\nMake a backup of your project beforehand!",
                "Regenerate GUIDs non-recursive", "Cancel"))
            {
                PickFolderAndRegenerateGUIDs(false);
            }
        }
        
        [MenuItem("Tools/Helpers/ApplyGUIDsOfPickedFolders/Recursive")]
        private static void ApplyGUIDsRecursive()
        {
            if (EditorUtility.DisplayDialog("GUIDs replacement recursive",
                "The first folder picker is for GUIDs to replace in this project and the second one is from the other project to read the GUIDs from." +
                "\nAfter that the process of GUID replacement will begin. " +
                "\n\nMake a backup of your project beforehand!",
                "Apply GUIDs recursive", "Cancel"))
            {
                GUIDHelper.ApplyGUIDsFrom(PickFolderInsideProject("Replace GUIDs in folder", "Assets/", null),
                    EditorUtility.OpenFolderPanel("Read GUIDs from folder", "Assets/", ""));
            }
        }
        

        #endregion
    }
}