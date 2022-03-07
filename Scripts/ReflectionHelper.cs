using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace NikosAssets.Helpers
{
    public static class ReflectionHelper
    {
        public static Type FindChildOfAncestor(Type descendant, Type ancestorToFind)
        {
            if (descendant == null || ancestorToFind == null)
                return null;

            if (descendant.BaseType == ancestorToFind)
                //found the child
                return descendant;

            return FindChildOfAncestor(descendant.BaseType, ancestorToFind);
        }
        
        public static Type FindGenericTypeOfParentClassAtIndex (Type descendant, Type parentToFind, int i = 0)
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
                    return ReflectionHelper.FindGenericTypeOfParentClassAtIndex(descendant.BaseType, parentToFind, i);
                }
            }

            return null;
        }

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
        
        public static List<Type> FindAllDerivedTypes(Type rootType)
        {
            return FindAllDerivedTypes(rootType, Assembly.GetAssembly(rootType));
        }

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
        /// Doesnt work with interfaces
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
        /// Doesnt work with interfaces
        /// </summary>
        /// <returns></returns>
        public static List<Type> FindDirectChildrenOfType(Type parentType)
        {
            return FindDirectChildrenOfType(parentType, Assembly.GetAssembly(parentType));
        }

        /// <summary>
        /// Doesnt work with interfaces
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
