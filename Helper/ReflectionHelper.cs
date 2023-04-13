using System;
using System.Collections.Generic;
using System.Reflection;

namespace TMI.Helper {

    public static class ReflectionHelper {

        public const BindingFlags defaultBindingFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public static List<Type> FindAllAssignableClassTypesForClass<TClass>() where TClass : class {
            Type classType = typeof(TClass);
            return FindAllAssignableClassTypesForClass(classType);
        }
        
        public static List<Type> FindAllAssignableClassTypesForClass(Type classType) {
            List<Type> foundTypes = new List<Type>();
            Type[] allAssemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
            foreach(Type type in allAssemblyTypes) {
                bool isAssignable = classType.IsAssignableFrom(type) && !type.IsAbstract;
                if(isAssignable) {
                    foundTypes.Add(type);
                }
            }
            return foundTypes;
        }
        
        

        public static List<Type> FindAllClassesWithAttribute<TAttribute>() where TAttribute : Attribute {
            Type attributeType = typeof(TAttribute);

            List<Type> foundTypes = new List<Type>();
            Type[] allAssemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
            foreach(Type type in allAssemblyTypes) {
                object[] foundAttributes = type.GetCustomAttributes(attributeType, false);

                if(foundAttributes.Length > 0) {
                    foundTypes.Add(type);
                }
            }
            return foundTypes;
        }

        public static List<FieldInfo> FindAllClassFieldsWithAttribute<TClass, TAttribute>() where TClass : class where TAttribute : Attribute {
            Type classType = typeof(TClass);
            Type attributeType = typeof(TAttribute);

            List<FieldInfo> foundFields = new List<FieldInfo>();
            FieldInfo[] allClassFields = classType.GetFields(defaultBindingFlag);
            foreach(FieldInfo field in allClassFields) {
                object[] foundAttributes = field.GetCustomAttributes(attributeType, false);

                if(foundAttributes.Length > 0) {
                    foundFields.Add(field);
                }
            }

            return foundFields;
        }

        public static Type FindTypeInAnyAssembly(string fullClassName) {

            // Try Type.GetType() first. This will work with types defined
            // by the Mono runtime, in the same assembly as the caller, etc.
            Type type = Type.GetType(fullClassName);

            // If it worked, then we're done here
            if(type != null) {
                return type;
            }

            // Attempt to search for type on the loaded assemblies
            Assembly[] currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(Assembly assembly in currentAssemblies) {
                type = assembly.GetType(fullClassName);
                if(type != null) {
                    return type;
                }
            }

            // If we still haven't found the proper type, we can enumerate all of the
            // loaded assemblies and see if any of them define the type
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            AssemblyName[] referencedAssemblies = currentAssembly.GetReferencedAssemblies();
            foreach(var assemblyName in referencedAssemblies) {
                // Load the referenced assembly
                var assembly = Assembly.Load(assemblyName);
                if(assembly != null) {
                    // See if that assembly defines the named type
                    type = assembly.GetType(fullClassName);
                    if(type != null) {
                        return type;
                    }
                }
            }

            // The type just couldn't be found... throw an exception
            throw new ArgumentException("Cannot find the type of [" + fullClassName + "] anywhere!");
        }

    }


}

