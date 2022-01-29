using UnityEditor;
using UnityEngine;

namespace NikosAssets.Helpers.Editor
{
    public static class GUIHelper
    {
        private static GUIStyle _colorBoxStyle;

        public static GUIStyle ColorBoxStyle
        {
            get
            {
                if (_colorBoxStyle == null)
                {
                    _colorBoxStyle = new GUIStyle();
                    _colorBoxStyle.border = new RectOffset(1, 1, 1, 1);
                    _colorBoxStyle.normal.background = GUIHelper.WhiteTexBorder1PX;
                }
                
                return _colorBoxStyle;
            }
        }
        
        public static Texture2D BlackTex1PX { get { return _blackTex1PX != null ? _blackTex1PX : _blackTex1PX = Resources.Load<Texture2D>("black1px"); } }
        private static Texture2D _blackTex1PX;
        
        public static Texture2D WhiteTexBorder1PX { get { return _whiteTexBorder1PX != null ? _whiteTexBorder1PX : _whiteTexBorder1PX = Resources.Load<Texture2D>("white_box_border1px"); } }
        private static Texture2D _whiteTexBorder1PX;
        
        public static Texture2D WhiteTexBorder2PX { get { return _whiteTexBorder2PX != null ? _whiteTexBorder2PX : _whiteTexBorder2PX = Resources.Load<Texture2D>("white_box_border2px"); } }
        private static Texture2D _whiteTexBorder2PX;
        
        public static void DrawLineHorizontalCentered(Color color, float height = 1, float widthMultiplier = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, height );
            rect.height = height;
            
            float fullWidth = rect.width;
            rect.width *= widthMultiplier;
            
            float diffWidth = fullWidth - rect.width;
            rect.x += diffWidth * .5f;

            EditorGUI.DrawRect(rect, color);
        }
        
        public static void DrawLineVerticalCentered(Color color, float width = 1, float height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, height);
            rect.width = width;

            EditorGUI.DrawRect(rect, color);
        }
        
        public static void DrawColorBox(Color boxColor, Color prevGUIColor, int boxWidth, int boxHeight, RectOffset margin)
        {
            ColorBoxStyle.margin = margin;

            GUI.color = boxColor;
            GUILayout.Label(GUIContent.none, ColorBoxStyle, GUILayout.Width(boxWidth), GUILayout.Height(boxHeight));
            //revert to prev color
            GUI.color = prevGUIColor;
        }
    }
}
