using UnityEditor;
using UnityEngine;

namespace NikosAssets.Helpers.Editor
{
    public static class GUIHelper
    {
        public static void GUILine(Color color, float height = 1, float widthMultiplier = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, height );
            rect.height = height;

            float fullWidth = rect.width;
            rect.width *= widthMultiplier;

            float diffWidth = fullWidth - rect.width;
            rect.x += diffWidth * .5f;

            EditorGUI.DrawRect(rect, color);
        }
    }
}
