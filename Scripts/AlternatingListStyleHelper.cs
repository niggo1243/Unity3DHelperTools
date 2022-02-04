using UnityEngine;

namespace NikosAssets.Helpers
{
    public class AlternatingListStyleHelper
    {
        protected Texture2D _selectedListItemTexture;
        protected Texture2D[] _listAlternatingTexArray;
        
        protected GUIStyle _listHorizontalItemStyle, _listWrappingStyle;

        public RectOffset paddingHorizontalItemStyle = new RectOffset(), paddingWrappingStyle = new RectOffset();
        public RectOffset marginHorizontalItemStyle = new RectOffset(), marginWrappingStyle = new RectOffset();
        
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

        public virtual GUIStyle GetWrappingStyle() => _listWrappingStyle;

        public virtual GUIStyle GetBodyItemStyle() => _listHorizontalItemStyle;
        
        /// <summary>
        /// special case when unity flushed the textures. This happens for example if we return from playmode to edit mode again...
        /// </summary>
        /// <returns></returns>
        public virtual bool RefreshNeeded() => _selectedListItemTexture == null;
    }
}
