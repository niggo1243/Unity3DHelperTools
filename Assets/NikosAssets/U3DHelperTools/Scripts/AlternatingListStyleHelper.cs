using UnityEngine;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// A helper class to draw a GUI list (for the Unity inspector) with alternating backgrounds foreach list item
    /// using the <see cref="AlternateListItemStyle"/> method.
    /// It is important to call the <see cref="InitStyles"/> method before any other method once after instantiation (or when a refresh is needed)!
    /// </summary>
    public class AlternatingListStyleHelper
    {
        protected Texture2D _selectedListItemTexture;
        protected Texture2D[] _listAlternatingTexArray;
        
        protected GUIStyle _listHorizontalItemStyle, _listWrappingStyle;

        public RectOffset paddingHorizontalItemStyle = new RectOffset(), paddingWrappingStyle = new RectOffset();
        public RectOffset marginHorizontalItemStyle = new RectOffset(), marginWrappingStyle = new RectOffset();
        
        /// <summary>
        /// It is very important to call this before any other method once after instantiation (or when a refresh is needed)!
        /// </summary>
        /// <param name="selectedItemColor">
        /// The special color for selected items
        /// </param>
        /// <param name="listItemAColor">
        /// First and every odd list item's color
        /// </param>
        /// <param name="listItemBColor">
        /// Second and every even list item's color
        /// </param>
        /// <param name="wrappingColor">
        /// The header and footer color of the list body (if used)
        /// </param>
        public virtual void InitStyles(Color selectedItemColor, Color listItemAColor, Color listItemBColor, Color wrappingColor)
        {
            _selectedListItemTexture = new Texture2D(1, 1);
            _selectedListItemTexture.SetPixel(0, 0, selectedItemColor);
            _selectedListItemTexture.Apply();
            
            //list style behavior (selected and even+odd list items)
            _listAlternatingTexArray = new Texture2D[2] {new Texture2D(1, 1), 
                new Texture2D(1, 1)};
            _listAlternatingTexArray[0].SetPixel(0, 0, listItemAColor);
            _listAlternatingTexArray[0].Apply();
            _listAlternatingTexArray[1].SetPixel(0, 0, listItemBColor);
            _listAlternatingTexArray[1].Apply();
            
            _listHorizontalItemStyle = new GUIStyle();
            _listHorizontalItemStyle.padding = this.paddingHorizontalItemStyle;
            _listHorizontalItemStyle.margin = this.marginHorizontalItemStyle;
            
            Texture2D listHeaderBckgTex = new Texture2D(1, 1);
            listHeaderBckgTex.SetPixel(0, 0, wrappingColor);
            listHeaderBckgTex.Apply();
            
            _listWrappingStyle = new GUIStyle(_listHorizontalItemStyle);
            _listWrappingStyle.normal.background = listHeaderBckgTex;
            _listWrappingStyle.padding = this.paddingWrappingStyle;
            _listWrappingStyle.margin = this.marginWrappingStyle;
        }
        
        /// <summary>
        /// Use this style as a background style for example when drawing with <see cref="GUILayout"/>.BeginHorizontal
        /// </summary>
        /// <param name="isActiveSelection">
        /// A special (blue by default) <see cref="GUIStyle"/> when the element is selected by the developer
        /// </param>
        /// <param name="i">
        /// The list item's index
        /// </param>
        /// <returns>
        /// The (alternating) list item <see cref="GUIStyle"/>
        /// </returns>
        public virtual GUIStyle AlternateListItemStyle(bool isActiveSelection, int i)
        {
            if (isActiveSelection)
            {
                _listHorizontalItemStyle.normal.background = _selectedListItemTexture;
            }
            else
            {
                _listHorizontalItemStyle.normal.background = _listAlternatingTexArray[i % 2];
            }
            
            return _listHorizontalItemStyle;
        }

        /// <summary>
        /// Used to draw the list header
        /// </summary>
        /// <returns>
        /// <see cref="GUIStyle"/>
        /// </returns>
        public virtual GUIStyle GetWrappingStyle() => _listWrappingStyle;

        /// <summary>
        /// Used to draw the item's content
        /// </summary>
        /// <returns>
        /// <see cref="GUIStyle"/>
        /// </returns>
        public virtual GUIStyle GetBodyItemStyle() => _listHorizontalItemStyle;
        
        /// <summary>
        /// Special case when Unity flushed the <see cref="Texture2D"/>s. This happens for example if we return from playmode to edit mode again...
        /// </summary>
        /// <returns></returns>
        public virtual bool RefreshNeeded() => _selectedListItemTexture == null;
    }
}
