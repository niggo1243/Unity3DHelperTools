using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace NikosAssets.Helpers.Editor
{
    public static class EditorUtilitiesHelper
    {
        public static string PickFolderInsideProject(string folderPickerTitle, string oldPath)
        {
            string path = EditorUtility.OpenFolderPanel(folderPickerTitle, oldPath, "");
            
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
                }

            }

            return oldPath;
        }
        
        public static UnityEngine.Object CreateScript(string pathName, string templateFileName, Dictionary<string, string> replaceContentsDict)
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