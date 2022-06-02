using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Samples
{
    public class ReflectionHelperSample : BaseNotesMono
    {
        [Button("Log all derived scriptable object types across all assemblies")]
        public void LogAllDerivedTypesOfScriptableObjects()
        {
            CollectionHelper.LogCollection(ReflectionHelper.FindAllDerivedTypesAcrossAllAssemblies(typeof(ScriptableObject)));
            Debug.Log("HINT: you can also specify the desired assembly either manually or automatically by just handing over the type");
        }

        [Button("What is the child of the MonoBehaviour in this line?")]
        public void LogChildOfMonoBehaviourInThisLine()
        {
            Debug.Log(ReflectionHelper.FindChildOfAncestor(this.GetType(), typeof(MonoBehaviour)).Name);
        }

        [Button("Log direct children of BaseNotesMono (Only the core Helpers assembly)")]
        public void LogDirectChildrenOfBaseNotesMono()
        {
            CollectionHelper.LogCollection(ReflectionHelper.FindDirectChildrenOfType(typeof(BaseNotesMono)));
            Debug.Log("HINT: there is also a method to find children across all assemblies!! Or you can specify one on your own!");
        }

        [Button("Log all interfaces that contain the IDisposable interface across all assemblies")]
        public void LogAllInterfacesThatContainIDisposable()
        {
            CollectionHelper.LogCollection(ReflectionHelper.FindInterfacesAcrossAllAssemblies(typeof(IDisposable)));
            Debug.Log("HINT: there is also a method to find 'children' (interfaces dont have a hierarchy) across all assemblies!! Or you can specify one on your own!");
        }

        [Button("Log the first generic of the TransformDestinationContainer.cs sample")]
        public void LogGenericOfSomeParent()
        {
            Debug.Log(ReflectionHelper.FindGenericTypeOfParentClassAtIndex(typeof(DestinationContainerTransformSample), 
                typeof(BaseDestinationContainerMono<>), 0).Name);
        }
    }
}
