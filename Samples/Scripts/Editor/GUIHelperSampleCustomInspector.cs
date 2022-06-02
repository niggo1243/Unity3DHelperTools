using NaughtyAttributes.Editor;
using NikosAssets.Helpers.Editor;
using UnityEditor;
using UnityEngine;

namespace NikosAssets.Helpers.Samples
{

    [CustomEditor(typeof(GUIHelperSample))]
    public class GUIHelperSampleCustomInspector : NaughtyInspector
    {
        public override void OnInspectorGUI()
        {
            //you should cache this!!!
            AlternatingListStyleHelper alternatingListStyleHelper = new AlternatingListStyleHelper();
            alternatingListStyleHelper.InitStyles(Color.blue, Color.red, Color.white, Color.black);
            
            base.OnInspectorGUI();
            
            GUILayout.Space(20);
            GUILayout.Label("This is all done in the 'GUIHelperSampleCustomInspector.cs' class. Take a look into the code!");

            GUIHelper.DrawColorBox(Color.magenta, GUI.color, 10, 10, new RectOffset());

            GUIHelper.DrawLineHorizontalCentered(Color.red, 2, .5f);

            GUIHelper.DrawLineVerticalCentered(Color.blue, 2, 75);

            GUILayout.Space(20);
            GUILayout.Label("Here we call the 'AlternatingListStyleHelper.cs' class for this:");
            for (int i = 0; i < 10; i++)
            {
                GUILayout.BeginHorizontal(alternatingListStyleHelper.AlternateListItemStyle(i == 2, i));
                GUILayout.Label("i: " + i);
                GUILayout.EndHorizontal();
            }
        }
    }
}
