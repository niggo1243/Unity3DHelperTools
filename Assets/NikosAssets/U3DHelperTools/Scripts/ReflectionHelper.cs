using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// A helper class for reflections and type handling, such as finding parent types with generic arguments
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Find the direct child of the desired <paramref name="ancestorToFind"/>
        /// </summary>
        /// <param name="descendant"></param>
        /// <param name="ancestorToFind"></param>
        /// <returns>
        /// null on fail, otherwise the first child <see cref="Type"/> of <paramref name="ancestorToFind"/>
        /// </returns>
        public static Type FindChildOfAncestor(Type descendant, Type ancestorToFind)
        {
            if (descendant == null || ancestorToFind == null)
                return null;

            if (descendant.BaseType == ancestorToFind)
                //found the child
                return descendant;

            return FindChildOfAncestor(descendant.BaseType, ancestorToFind);
        }
        
        /// <summary>
        /// Find a certain generic type at index <paramref name="i"/> in the <paramref name="ancestorToFind"/>
        /// </summary>
        /// <param name="descendant"></param>
        /// <param name="ancestorToFind"></param>
        /// <param name="i"></param>
        /// <returns>
        /// null on fail, otherwise the generic <see cref="Type"/> at index i
        /// </returns>
        public static Type FindGenericTypeOfParentClassAtIndex (Type descendant, Type ancestorToFind, int i = 0)
        {
            if (descendant == null || ancestorToFind == null)
                return null;

            //Debug.Log("need to find: " + parentToFind.Name);
            if (descendant.BaseType != null)
            {
                //Debug.Log(descendant.BaseType.Name);
                if (descendant.BaseType.Name.Equals(ancestorToFind.Name))
                {
                    Type[] generics = descendant.BaseType.GetGenericArguments();

                    if (CollectionHelper.CollectionAndIndexChecker(generics, i))
                    {
                        //Debug.Log("Found Generic! : " + generics[i].Name);
                        return generics[i];
                    }
                }
                else
                {
                    return FindGenericTypeOfParentClassAtIndex(descendant.BaseType, ancestorToFind, i);
                }
            }

            return null;
        }

        /// <summary>
        /// Note: Doesn't work with interfaces
        /// </summary>
        /// <returns></returns>
        public static List<Type> FindAllDerivedTypesAcrossAllAssemblies(Type rootType)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> types = new List<Type>();
            
            foreach (Assembly assembly in assemblies)
            {
                types.AddRange(FindAllDerivedTypes(rootType, assembly));
            }

            return types;
        }
        
        /// <summary>
        /// Note: Doesn't work with interfaces
        /// </summary>
        /// <returns></returns>
        public static List<Type> FindAllDerivedTypes(Type rootType)
        {
            return FindAllDerivedTypes(rootType, Assembly.GetAssembly(rootType));
        }

        /// <summary>
        /// Note: Doesn't work with interfaces
        /// </summary>
        /// <returns></returns>
        public static List<Type> FindAllDerivedTypes(Type rootType, Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(t =>
                    t != rootType &&
                    rootType.IsAssignableFrom(t)
                ).ToList();
        } 
        
        /// <summary>
        /// Note: Doesn't work with interfaces
        /// </summary>
        /// <returns></returns>
        public static List<Type> FindDirectChildrenOfTypeAcrossAllAssemblies(Type parentType)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> types = new List<Type>();
            
            foreach (Assembly assembly in assemblies)
            {
                types.AddRange(FindDirectChildrenOfType(parentType, assembly));
            }

            return types;
        }
        
        /// <summary>
        /// Note: Doesn't work with interfaces
        /// </summary>
        /// <returns></returns>
        public static List<Type> FindDirectChildrenOfType(Type parentType)
        {
            return FindDirectChildrenOfType(parentType, Assembly.GetAssembly(parentType));
        }

        /// <summary>
        /// Note: Doesn't work with interfaces
        /// </summary>
        /// <param name="parentType"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static List<Type> FindDirectChildrenOfType(Type parentType, Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(t =>
                    t.BaseType == parentType
                ).ToList();
        }
        
        public static List<Type> FindInterfacesAcrossAllAssemblies(Type interfaceToContain, int maxInterfaces = 1)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> types = new List<Type>();
            
            foreach (Assembly assembly in assemblies)
            {
                types.AddRange(FindInterfaces(interfaceToContain, assembly, maxInterfaces));
            }

            return types;        
        }
        
        public static List<Type> FindInterfaces(Type interfaceToContain, int maxInterfaces = 1)
        {
            return FindInterfaces(interfaceToContain, Assembly.GetAssembly(interfaceToContain), maxInterfaces);
        }
        
        public static List<Type> FindInterfaces(Type interfaceToContain, Assembly assembly, int maxInterfaces = 1)
        {
            return assembly
                .GetTypes()
                .Where(t =>
                {
                    Type[] interfaces = t.GetInterfaces();
                    return t.IsInterface && interfaces.Length == maxInterfaces && interfaces.Contains(interfaceToContain);
                }).ToList();
        }
    }
}
