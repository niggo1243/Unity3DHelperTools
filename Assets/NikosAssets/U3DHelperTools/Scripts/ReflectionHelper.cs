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
        /// Find the direct child of the desired "<paramref name="ancestorToFind"/>"
        /// </summary>
        /// <param name="descendant">The (grand) child we want to move up from</param>
        /// <param name="ancestorToFind">The child of this to find</param>
        /// <returns>
        /// null on fail, otherwise the first child <see cref="Type"/> of "<paramref name="ancestorToFind"/>"
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
        /// Find a certain generic type at index "<paramref name="i"/>" in the "<paramref name="ancestorToFind"/>"
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
        /// Finds all from the given "<paramref name="rootType"/>" derived types across all assemblies in this project.
        /// Note: Doesn't work with interfaces
        /// </summary>
        /// <param name="rootType"></param>
        /// <returns>
        /// A list of the found child <see cref="Type"/>s
        /// </returns>
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
        /// Finds all from the given "<paramref name="rootType"/>" derived types within the same assembly, the "<paramref name="rootType"/>" is in.
        /// Note: Doesn't work with interfaces
        /// </summary>
        /// <param name="rootType"></param>
        /// <returns>
        /// A list of the found child <see cref="Type"/>s
        /// </returns>
        public static List<Type> FindAllDerivedTypes(Type rootType)
        {
            return FindAllDerivedTypes(rootType, Assembly.GetAssembly(rootType));
        }

        /// <summary>
        /// Finds all from the given "<paramref name="rootType"/>" derived types within the given "<paramref name="assembly"/>".
        /// Note: Doesn't work with interfaces
        /// </summary>
        /// <param name="rootType"></param>
        /// <param name="assembly"></param>
        /// <returns>
        /// A list of the found child <see cref="Type"/>s
        /// </returns>
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
        /// Finds all direct (first generation) child <see cref="Type"/>s of the given
        /// "<paramref name="parentType"/>" across all assemblies of this project.
        /// Note: Doesn't work with interfaces
        /// </summary>
        /// <param name="parentType"></param>
        /// <returns>
        /// A list of the found child <see cref="Type"/>s
        /// </returns>
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
        /// Finds all direct (first generation) child <see cref="Type"/>s of the given "<paramref name="parentType"/>" within its assembly.
        /// Note: Doesn't work with interfaces
        /// </summary>
        /// <param name="parentType"></param>
        /// <returns>
        /// A list of the found child <see cref="Type"/>s
        /// </returns>
        public static List<Type> FindDirectChildrenOfType(Type parentType)
        {
            return FindDirectChildrenOfType(parentType, Assembly.GetAssembly(parentType));
        }

        /// <summary>
        /// Finds all direct (first generation) child <see cref="Type"/>s of the given "<paramref name="parentType"/>"
        /// within the given "<paramref name="assembly"/>"
        /// Note: Doesn't work with interfaces
        /// </summary>
        /// <param name="parentType"></param>
        /// <param name="assembly"></param>
        /// <returns>
        /// A list of the found child <see cref="Type"/>s
        /// </returns>
        public static List<Type> FindDirectChildrenOfType(Type parentType, Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(t =>
                    t.BaseType == parentType
                ).ToList();
        }
        
        /// <summary>
        /// Find "child" interfaces of the given "<paramref name="parentInterface"/>" that contain a limited amount of other interfaces
        /// across all assemblies for this project
        /// </summary>
        /// <param name="parentInterface">
        /// The "parent" interface to start the search from
        /// </param>
        /// <param name="maxInterfaces">
        /// You can interpret this value as a maximum "child/ancestor" depth.
        /// It checks how many interfaces the current traversed interface contains/ implements
        /// </param>
        /// <returns>
        /// A list of the found "child" <see cref="Type"/> interfaces
        /// </returns>
        public static List<Type> FindInterfacesAcrossAllAssemblies(Type parentInterface, int maxInterfaces = 1)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> types = new List<Type>();
            
            foreach (Assembly assembly in assemblies)
            {
                types.AddRange(FindInterfaces(parentInterface, assembly, maxInterfaces));
            }

            return types;        
        }
        
        /// <summary>
        /// Find "child" interfaces of the given "<paramref name="parentInterface"/>" that contain a limited amount of other interfaces
        /// within the "<paramref name="parentInterface"/>'s" assembly
        /// </summary>
        /// <param name="parentInterface">
        /// The "parent" interface to start the search from
        /// </param>
        /// <param name="maxInterfaces">
        /// You can interpret this value as a maximum "child/ancestor" depth.
        /// It checks how many interfaces the current traversed interface contains/ implements
        /// </param>
        /// <returns>
        /// A list of the found "child" <see cref="Type"/> interfaces
        /// </returns>
        public static List<Type> FindInterfaces(Type parentInterface, int maxInterfaces = 1)
        {
            return FindInterfaces(parentInterface, Assembly.GetAssembly(parentInterface), maxInterfaces);
        }

        /// <summary>
        /// Find "child" interfaces of the given "<paramref name="parentInterface"/>" that contain a limited amount of other interfaces
        /// within the given "<paramref name="assembly"/>"
        /// </summary>
        /// <param name="parentInterface">
        /// The "parent" interface to start the search from
        /// </param>
        /// <param name="assembly">
        /// The assembly to search the interfaces
        /// </param>
        /// <param name="maxInterfaces">
        /// You can interpret this value as a maximum "child/ancestor" depth.
        /// It checks how many interfaces the current traversed interface contains/ implements
        /// </param>
        /// <returns>
        /// A list of the found "child" <see cref="Type"/> interfaces
        /// </returns>
        public static List<Type> FindInterfaces(Type parentInterface, Assembly assembly, int maxInterfaces = 1)
        {
            return assembly
                .GetTypes()
                .Where(t =>
                {
                    Type[] interfaces = t.GetInterfaces();
                    return t.IsInterface && interfaces.Length <= maxInterfaces && interfaces.Contains(parentInterface);
                }).ToList();
        }
    }
}
