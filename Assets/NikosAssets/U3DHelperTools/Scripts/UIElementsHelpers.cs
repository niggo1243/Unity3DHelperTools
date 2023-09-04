using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace NikosAssets.Helpers
{
    public static class UIElementsHelpers
    {
        public static void ToggleVisualElementVisibility(VisualElement ve, bool visible)
        {
            ve.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            ve.visible = visible;
            ve.SetEnabled(visible);
        }
        
        public static Child GetFirstChildOfFoundElement<Parent, Child>(VisualElement root, string parentName, string[] classNames = null) 
            where Parent : VisualElement where Child : VisualElement
        {
            return (Child)GetFoundElement<Parent>(root, parentName, classNames).Children().First();
        }
        
        public static T GetFoundElement<T>(VisualElement root, string parentName, string[] classNames = null) 
            where T : VisualElement
        {
            return (T)root.Q(parentName, classNames);
        }
        
        public static List<Child> GetAllFirstChildrenOfFoundElements<Parent, Child>(VisualElement root, string parentName, string[] classNames = null)
            where Parent : VisualElement where Child : VisualElement
        {
            return root.Query<Parent>(parentName, classNames)
                .Build().ToList().Select(ve => (Child)ve.Children().First()).ToList();
        }
        
        public static List<T> GetAllFoundElements<T>(VisualElement root, string parentName, string[] classNames = null)
            where T : VisualElement
        {
            return root.Query<T>(parentName, classNames).Build().ToList();
        }
    }
}
