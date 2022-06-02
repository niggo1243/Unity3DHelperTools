using UnityEditor;
using UnityEngine;

namespace NikosAssets.Helpers.Editor
{
    /// <summary>
    /// A custom Editor GUI helper to draw stuff in the Editor Window & Inspector 
    /// </summary>
    public static class GUIHelper
    {
        private static GUIStyle _colorBoxStyle;

        /// <summary>
        /// A color box with a 1px border
        /// </summary>
        public static GUIStyle ColorBoxStyle
        {
            get
            {
                if (_colorBoxStyle == null)
                {
                    _colorBoxStyle = new GUIStyle();
                    _colorBoxStyle.border = new RectOffset(1, 1, 1, 1);
                    _colorBoxStyle.normal.background = WhiteTexBorder1PX;
                }
                
                return _colorBoxStyle;
            }
        }
        
        /// <summary>
        /// A black texture (non transparent, unlike <see cref="Texture2D"/>.<see cref="Texture2D.blackTexture"/>)
        /// </summary>
        public static Texture2D BlackTex1PX { get { return _blackTex1PX != null ? _blackTex1PX : _blackTex1PX = Resources.Load<Texture2D>("black1px"); } }
        private static Texture2D _blackTex1PX;
        
        /// <summary>
        /// A white texture with a 1px square border working on different resolutions unlike when calculating a border artificially
        /// </summary>
        public static Texture2D WhiteTexBorder1PX { get { return _whiteTexBorder1PX != null ? _whiteTexBorder1PX : _whiteTexBorder1PX = Resources.Load<Texture2D>("white_box_border1px"); } }
        private static Texture2D _whiteTexBorder1PX;
        
        /// <summary>
        /// Draws a horizontal line centered in the x axis (rect from <see cref="EditorGUILayout.GetControlRect"/>)
        /// </summary>
        /// <param name="color">
        /// The color of the line
        /// </param>
        /// <param name="height">
        /// The height of the line in px
        /// </param>
        /// <param name="widthMultiplier">
        /// Multiplies the current rect width with this value
        /// </param>
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
        
        /// <summary>
        /// Draws a vertical line 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void DrawLineVerticalCentered(Color color, float width = 1, float height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, height);
            rect.width = width;

            EditorGUI.DrawRect(rect, color);
        }
        
        /// <summary>
        /// Draws a colored box with a 1px (black) border
        /// </summary>
        /// <param name="boxColor">
        /// The desired box color
        /// </param>
        /// <param name="prevGUIColor">
        /// The previous GUI.color to restore after the box is drawn
        /// </param>
        /// <param name="boxWidth">
        /// The width of the box
        /// </param>
        /// <param name="boxHeight">
        /// The height of the box
        /// </param>
        /// <param name="margin">
        /// The margin applied to the box <see cref="GUIStyle"/>
        /// </param>
        public static void DrawColorBox(Color boxColor, Color prevGUIColor, int boxWidth, int boxHeight, RectOffset margin)
        {
            GUIStyle style = new GUIStyle(ColorBoxStyle);
            style.margin = margin;

            GUI.color = boxColor;
            GUILayout.Label(GUIContent.none, style, GUILayout.Width(boxWidth), GUILayout.Height(boxHeight));
            //revert to prev color
            GUI.color = prevGUIColor;
        }
    }
}
