using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// A helper class to query UIElements by certain criteria
    /// </summary>
    public static class UIElementsHelpers
    {
        /// <summary>
        /// Set the <see cref="VisualElement"/> "<paramref name="ve"/>" visible or invisible
        /// </summary>
        /// <param name="ve"><see cref="VisualElement"/></param>
        /// <param name="visible">Make visible or invisible</param>
        public static void ToggleVisualElementVisibility(VisualElement ve, bool visible)
        {
            ve.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            ve.visible = visible;
            ve.SetEnabled(visible);
        }
        
        /// <summary>
        /// Get the first found child of the parent <see cref="VisualElement"/> "<paramref name="parentName"/>"
        /// </summary>
        /// <param name="root">The root to start the query</param>
        /// <param name="parentName">The <see cref="VisualElement"/> to find that will be the parent of the first child we need</param>
        /// <param name="classNames"></param>
        /// <typeparam name="ChildType">The specific <see cref="VisualElement"/> we search</typeparam>
        /// <returns>A <see cref="VisualElement"/> or null</returns>
        public static ChildType GetFirstChildOfFoundElement<ChildType>(VisualElement root, string parentName, string[] classNames = null) 
            where ChildType : VisualElement
        {
            return (ChildType)(root.Q(parentName, classNames).Children().FirstOrDefault());
        }
        
        /// <summary>
        /// Find all ParentTypes of the "<paramref name="root"/>" <see cref="VisualElement"/> and return their first ChildType in a list
        /// </summary>
        /// <param name="root">The root to start the query</param>
        /// <param name="parentName">The parents to find</param>
        /// <param name="classNames"></param>
        /// <typeparam name="ParentType"></typeparam>
        /// <typeparam name="ChildType"></typeparam>
        /// <returns>A list of first children of the parents list</returns>
        public static List<ChildType> GetAllFirstChildrenOfFoundElements<ParentType, ChildType>(VisualElement root, string parentName, string[] classNames = null)
            where ParentType : VisualElement where ChildType : VisualElement
        {
            return GetAllFoundElements<ParentType>(root, parentName, classNames)
                .Select(ve => (ChildType)ve.Children().FirstOrDefault()).ToList();
        }
        
        /// <summary>
        /// Get all found <see cref="VisualElement"/>s
        /// </summary>
        /// <param name="root">The root to start the query</param>
        /// <param name="name">The name to search</param>
        /// <param name="classNames"></param>
        /// <typeparam name="T">The <see cref="VisualElement"/> type we seek</typeparam>
        /// <returns>A list of <see cref="VisualElement"/>s</returns>
        public static List<T> GetAllFoundElements<T>(VisualElement root, string name, string[] classNames = null)
            where T : VisualElement
        {
            return root.Query<T>(name, classNames).Build().ToList();
        }
    }
}
