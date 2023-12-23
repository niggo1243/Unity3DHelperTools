using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;
using UnityEngine;

namespace NikosAssets.Helpers.Editor
{
    /// <summary>
    /// A helper class to regenerate file GUIDs while keeping their references in other files (Scenes, Assets, etc.).
    /// Inspired by <a href="https://github.com/nvjob/unityguidregenerator/blob/master/Assets/Editor/UnityGuidRegenerator.cs">this repo</a>
    /// </summary>
    public static class GUIDHelper
    {
        public enum AcceptedMetaFiles
        {
            /// <summary>
            /// Any file or folder asset
            /// </summary>
            Any = 0,
            /// <summary>
            /// Only search for files and ignore folder meta assets
            /// </summary>
            IgnoreFolders = 1,
            /// <summary>
            /// Only search for folder meta assets and ignore files
            /// </summary>
            IgnoreFiles = 2
        }
        
        /// <summary>
        /// Unity files that contain GUID (references), feel free to add unlisted ones
        /// </summary>
        public static List<string> DefaultNonBinaryUnityFileExtensions = new List<string>()
        {
            "*.meta",
            "*.mat",
            "*.prefab",
            "*.unity",
            "*.asset",
            "*.manifest",
            "*.inputactions",
            "*.anim",
            "*.controller",
            "*.overrideController",
            "*.mask",
            "*.flare",
            "*.brush",
            "*.preset",
            "*.mixer",
            "*.asmdef",
            "*.asmref",
            "*.shadervariants",
            "*.shadergraph",
            "*.vfx",
            "*.renderTexture",
            "*.cubemap",
            "*.fontsettings",
            "*.spriteatlas",
            "*.guiskin",
            "*.physicMaterial",
            "*.physicMaterial2D",
            "*.signal",
            "*.terrainlayer",
            "*.playable",
            "*.wlt"
        };
        
        /// <summary>
        /// Checks if the given asset at <paramref name="filePath"/> is a GUID owner (.meta file)
        /// </summary>
        /// <param name="isInLocalDirectory">
        /// Must be within directory
        /// </param>
        /// <param name="filePath">
        /// The asset to check if it is a GUID owner file
        /// </param>
        /// <param name="recursive">
        /// Can the asset be in a sub directory of <paramref name="isInLocalDirectory"/>?
        /// </param>
        /// <param name="acceptedMetaFiles">
        /// Should we accept only folders or only file assets or both?
        /// </param>
        /// <param name="whiteListExtensions">
        /// Must include extensions like (.mat, .cs) or accept all file types.
        /// Also, .meta doesn't have to be defined.
        /// </param>
        /// <returns>
        /// If the given file at <paramref name="filePath"/> is a GUID owner (meta) file (true or false)
        /// </returns>
        private static bool IsAcceptedMetaFile(string isInLocalDirectory, string filePath, bool recursive, 
            AcceptedMetaFiles acceptedMetaFiles, string[] whiteListExtensions)
        {
            if (!Path.GetExtension(filePath).Equals(".meta")) return false;
            
            string directory = Path.GetDirectoryName(filePath).Replace(@"\", "/") + "/";

            //any meta file
            bool isAccepted = true;
            string fileOrFolder = filePath.Substring(0, filePath.Length - 5);
            switch (acceptedMetaFiles)
            {
                case AcceptedMetaFiles.IgnoreFolders:
                    isAccepted = File.Exists(fileOrFolder);
                    break;
                case AcceptedMetaFiles.IgnoreFiles:
                    isAccepted = Directory.Exists(fileOrFolder);
                    break;
            }
            
            return 
                (isAccepted
                 &&
                 (
                     //not recursive means that the directory ends with the localAssetPath
                     (!recursive && directory.EndsWith(isInLocalDirectory))
                     //recursive means that the path must be found somewhere in the string
                     || (recursive && directory.Contains(isInLocalDirectory))
                 )
                 &&
                 (
                     //either all extensions or only selected ones
                     whiteListExtensions == null ||
                     whiteListExtensions.Any(ext => filePath.EndsWith(ext + ".meta"))
                 )
                );
        }

        /// <summary>
        /// Is this a GUID?
        /// </summary>
        /// <param name="guid">
        /// The potential guid
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsGuid(string guid)
        {
            for (int i = 0; i < guid.Length; i++)
            {
                char c = guid[i];
                if (
                    !((c >= '0' && c <= '9') ||
                      (c >= 'a' && c <= 'z'))
                )
                    return false;
            }

            return true;
        }
        
        /// <summary>
        /// Replaces every GUID defined in <paramref name="guidOldToNewMap"/> in a file
        /// found in <paramref name="guidsInFileMap"/>, keeping the file references intact
        /// </summary>
        /// <param name="guidsInFileMap">
        /// The potential files to change the GUIDs if found in <paramref name="guidOldToNewMap"/>
        /// </param>
        /// <param name="guidOldToNewMap">
        /// The old to new GUIDs map
        /// </param>
        /// <param name="logChangedAssets">
        /// Should the changed files be logged?
        /// </param>
        public static void SetGuids(
            Dictionary<string, List<string>> guidsInFileMap, 
            Dictionary<string, string> guidOldToNewMap,
            bool logChangedAssets = false)
        {
            try
            {
                AssetDatabase.StartAssetEditing();

                int counter = 0;
                // Traverse the files and replace the old GUIDs
                int guidsInFileMapKeysCount = guidsInFileMap.Keys.Count;
                foreach (string filePath in guidsInFileMap.Keys)
                {
                    EditorUtility.DisplayProgressBar("Regenerating GUIDs", filePath,
                        counter / (float) guidsInFileMapKeysCount);
                    counter++;

                    string contents = File.ReadAllText(filePath);

                    foreach (string oldGuid in guidsInFileMap[filePath])
                    {
                        guidOldToNewMap.TryGetValue(oldGuid, out var newGuid);
                        if (string.IsNullOrEmpty(newGuid))
                            continue;

                        if (logChangedAssets)
                            Debug.Log("replacing GUID in: " + filePath);

                        contents = contents.Replace("guid: " + oldGuid, "guid: " + newGuid);
                        contents = contents.Replace("guid:" + oldGuid, "guid:" + newGuid);

                        //asmdef specific
                        contents = contents.Replace("GUID: " + oldGuid, "GUID: " + newGuid);
                        contents = contents.Replace("GUID:" + oldGuid, "GUID:" + newGuid);
                    }

                    File.WriteAllText(filePath + ".temp", contents);
                    File.Delete(filePath);
                    File.Move(filePath + ".temp", filePath);
                }

                EditorUtility.ClearProgressBar();

            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                EditorUtility.ClearProgressBar();
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// Search and apply GUIDs form another project for files with the same name in this project.
        /// Every GUID change will be traced in every other Unity file, so no references will be lost!
        /// Can be used for recovery or package (update) consistency
        /// </summary>
        /// <param name="localPathToApply">
        /// The local project path where the GUIDs will be applied to
        /// </param>
        /// <param name="globalPathToRead">
        /// The global path to read the files from that match the local file names and finally extract the GUIDs from
        /// </param>
        /// <param name="recursive">
        /// Should we search for files inside other found folders of the <paramref name="localPathToApply"/>?
        /// </param>
        /// <param name="acceptedMetaFiles">
        /// Should we accept only folders or only file assets or both?
        /// </param>
        /// <param name="logChangedAssets">
        /// Should we log the changed assets?
        /// </param>
        /// <param name="whiteListExtensions">
        /// If null, no filter is applied, otherwise only the pre defined assets (no .meta definition required).
        /// </param>
        public static void ApplyGUIDsFrom(string localPathToApply, string globalPathToRead,
            bool recursive = true,
            AcceptedMetaFiles acceptedMetaFiles = AcceptedMetaFiles.IgnoreFolders,
            bool logChangedAssets = false,
            string[] whiteListExtensions = null)
        {
            // Get the list of working files
            List<string> allFilesPaths = EditorUtilitiesHelper.GetFiles("", true, DefaultNonBinaryUnityFileExtensions.ToArray());
            List<string> allGlobalFilesToRead = new List<string>();
            foreach (string extension in DefaultNonBinaryUnityFileExtensions)
            {
                allGlobalFilesToRead.AddRange(
                    Directory.GetFiles(globalPathToRead,
                            extension,
                            recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                        .Select(p => p.Replace(@"\", "/")));
            }

            // Create dictionary to hold old-to-new GUID map
            Dictionary<string, string> guidOldToNewMap = new Dictionary<string, string>();
            Dictionary<string, List<string>> guidsInFileMap = new Dictionary<string, List<string>>();

            try
            {
                // Traverse all files, remember which GUIDs are in which files and generate new GUIDs
                for (int i = 0; i < allFilesPaths.Count; i++)
                {
                    string filePath = allFilesPaths[i];
                    string fileNameLocal = Path.GetFileName(filePath);

                    EditorUtility.DisplayProgressBar("Scanning Assets folder", localPathToApply,
                        i / (float) allFilesPaths.Count);
                    string contents = File.ReadAllText(filePath);
                    List<string> guids = GetGuidsFromFileContents(contents);

                    //can we add this file to the guidOldToNewMap?
                    if (IsAcceptedMetaFile(localPathToApply, filePath, recursive, acceptedMetaFiles,
                        whiteListExtensions))
                    {
                        int matchingGlobalFilePathIndex =
                            allGlobalFilesToRead.FindIndex(gp => Path.GetFileName(gp).Equals(fileNameLocal));

                        if (matchingGlobalFilePathIndex < 0) continue;

                        string matchingGlobalPath = allGlobalFilesToRead[matchingGlobalFilePathIndex];
                        allGlobalFilesToRead.RemoveAt(matchingGlobalFilePathIndex);

                        string readFileContents = File.ReadAllText(matchingGlobalPath);
                        List<string> guidsReadGlobal = GetGuidsFromFileContents(readFileContents);
                        string replaceGuid = guidsReadGlobal.First();

                        string oldGuid = guids.First();

                        // Generate and save new GUID if we haven't added it before
                        if (!guidOldToNewMap.ContainsKey(oldGuid))
                        {
                            guidOldToNewMap.Add(oldGuid, replaceGuid);
                        }
                    }

                    //for .meta and all other file extensions
                    //store all global files, since we may have not have yet found all owned old guids, so we filter later
                    foreach (string oldGuid in guids)
                    {
                        if (!guidsInFileMap.ContainsKey(filePath))
                            guidsInFileMap[filePath] = new List<string>();

                        if (!guidsInFileMap[filePath].Contains(oldGuid))
                        {
                            guidsInFileMap[filePath].Add(oldGuid);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                SetGuids(guidsInFileMap, guidOldToNewMap, logChangedAssets);
            }
        }

        /// <summary>
        /// Creates new random GUIDs for every found file in the given local path.
        /// Every GUID change will be traced in every other Unity file, so no references will be lost
        /// </summary>
        /// <param name="localPath">
        /// The local project path
        /// </param>
        /// <param name="recursive">
        /// Should we search for files inside other found folders of the <paramref name="localPath"/>?
        /// </param>
        /// <param name="acceptedMetaFiles">
        /// Should we accept only folders or only file assets or both?
        /// </param>
        /// <param name="logChangedAssets">
        /// Should we log the changed assets?
        /// </param>
        /// <param name="whiteListExtensions">
        /// If null, no filter is applied, otherwise only the pre defined assets (no .meta definition required).
        /// </param>
        public static void RegenerateGuids(
            string localPath,
            bool recursive = true,
            AcceptedMetaFiles acceptedMetaFiles = AcceptedMetaFiles.Any,
            bool logChangedAssets = false,
            string[] whiteListExtensions = null)
        {
            // Get the list of working files
            List<string> allFilesPaths = EditorUtilitiesHelper.GetFiles("", true, DefaultNonBinaryUnityFileExtensions.ToArray());

            // Create dictionary to hold old-to-new GUID map
            Dictionary<string, string> guidOldToNewMap = new Dictionary<string, string>();
            Dictionary<string, List<string>> guidsInFileMap = new Dictionary<string, List<string>>();

            // Traverse all files, remember which GUIDs are in which files and generate new GUIDs
            try
            {
                for (int i = 0; i < allFilesPaths.Count; i++)
                {
                    string filePath = allFilesPaths[i];

                    EditorUtility.DisplayProgressBar("Scanning Assets folder", localPath,
                        i / (float) allFilesPaths.Count);
                    string contents = File.ReadAllText(filePath);
                    List<string> guids = GetGuidsFromFileContents(contents);

                    //can we add this file to the guidOldToNewMap?
                    if (IsAcceptedMetaFile(localPath, filePath, recursive, acceptedMetaFiles, whiteListExtensions))
                    {
                        string oldGuid = guids.First();
                        // Generate and save new GUID if we haven't added it before
                        if (!guidOldToNewMap.ContainsKey(oldGuid))
                        {
                            string newGuid = Guid.NewGuid().ToString("N");
                            guidOldToNewMap.Add(oldGuid, newGuid);
                        }
                    }

                    //for .meta and all other file extensions
                    //store all global files, since we may have not have yet found all owned old guids, so we filter later
                    foreach (string oldGuid in guids)
                    {
                        if (!guidsInFileMap.ContainsKey(filePath))
                            guidsInFileMap[filePath] = new List<string>();

                        if (!guidsInFileMap[filePath].Contains(oldGuid))
                        {
                            guidsInFileMap[filePath].Add(oldGuid);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                SetGuids(guidsInFileMap, guidOldToNewMap, logChangedAssets);
            }
        }

        /// <summary>
        /// Return found GUIDs of the given file contents (text/ string) for every found "guid:" prefix
        /// </summary>
        /// <param name="fileContents">
        /// The string contents of a file
        /// </param>
        /// <returns>
        /// A list of found GUID strings
        /// </returns>
        public static List<string> GetGuidsFromFileContents(string fileContents)
        {
            List<string> guids = new List<string>();

            //also checks for "GUID: ", "GUID:", "guid: "
            string fileContentsSimple = fileContents.ToLower().Replace(" ", "");
            guids.AddRange(GetGuidsFromFileContents(fileContentsSimple, "guid:"));

            return guids;
        }
        
        /// <summary>
        /// Return found GUIDs of the given file contents (text/ string)
        /// </summary>
        /// <param name="fileContents">
        /// The string contents of a file
        /// </param>
        /// <param name="guidStart">
        /// Search for the GUID prefix
        /// </param>
        /// <returns>
        /// A list of found GUID strings
        /// </returns>
        public static List<string> GetGuidsFromFileContents(string fileContents, string guidStart)
        {
            const int guidLength = 32;
            int textLength = fileContents.Length;
            int guidStartLength = guidStart.Length;
            List<string> guids = new List<string>();

            int index = 0;
            while (index + guidStartLength + guidLength < textLength)
            {
                index = fileContents.IndexOf(guidStart, index, StringComparison.Ordinal);
                if (index == -1)
                    break;

                index += guidStartLength;
                string guid = fileContents.Substring(index, guidLength);
                index += guidLength;

                if (IsGuid(guid))
                {
                    guids.Add(guid);
                }
            }

            return guids;
        }
    }
}