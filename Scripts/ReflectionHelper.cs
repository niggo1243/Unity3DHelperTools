using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace NikosAssets.Helpers
{
    public static class ReflectionHelper
    {
        public static Type GetFoundChildOfAncestor(Type descendant, Type ancestorToFind)
        {
            if (descendant == null || ancestorToFind == null)
                return null;

            if (descendant.BaseType == ancestorToFind)
                //found the child
                return descendant;

            return GetFoundChildOfAncestor(descendant.BaseType, ancestorToFind);
        }
        
        public static Type GetFoundGenericTypeOfParentClassAtIndex (Type descendant, Type parentToFind, int i = 0)
        {
            if (descendant == null || parentToFind == null)
                return null;

            //Debug.Log("need to find: " + parentToFind.Name);

            if (descendant.BaseType != null)
            {
                //Debug.Log(descendant.BaseType.Name);

                if (descendant.BaseType.Name.Equals(parentToFind.Name))
                {
                    Type[] generics = descendant.BaseType.GetGenericArguments();

                    if (generics.Length > 0 && i >= 0 && i < generics.Length)
                    {
                        //Debug.Log("Found Generic! : " + generics[i].Name);

                        return generics[i];
                    }
                }
                else
                {
                    return ReflectionHelper.GetFoundGenericTypeOfParentClassAtIndex(descendant.BaseType, parentToFind, i);
                }
            }

            return null;
        }

        public static bool TypeIsOtherType(Type t, Type otherType)
        {
            if (t == null || otherType == null)
                return false;

            if (t.Name.Equals(otherType.Name))
                return true;

            return ReflectionHelper.TypeIsOtherType(t.BaseType, otherType);
        }

        public static List<Type> FindAllDerivedTypesAcrossAllAssemblies<T>()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> types = new List<Type>();
            
            foreach (Assembly assembly in assemblies)
            {
                types.AddRange(FindAllDerivedTypes<T>(assembly));
            }

            return types;
        }
        
        public static List<Type> FindAllDerivedTypes<T>()
        {
            return FindAllDerivedTypes<T>(Assembly.GetAssembly(typeof(T)));
        }

        public static List<Type> FindAllDerivedTypes<T>(Assembly assembly)
        {
            var derivedType = typeof(T);
            return assembly
                .GetTypes()
                .Where(t =>
                    t != derivedType &&
                    derivedType.IsAssignableFrom(t)
                ).ToList();
        } 
    }
}
