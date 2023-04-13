using System;
using UnityEngine;
using System.Text;
using TMI.LogManagement;

namespace TMI.Helper {

    [Loggable]
    public static class GeneralHelper {

        private static readonly string fullTypeName = typeof(GeneralHelper).FullName;

        public static TType CreateInstance<TType>(string namespaceName, string className, params object[] constructorParameters) {
            string fullClassName;

            if(string.IsNullOrEmpty(namespaceName)) {
                fullClassName = className;
            } else {
                fullClassName = namespaceName + "." + className;
            }

            Logging.Log(fullTypeName, "Preparing to create an instance of [{0}].", fullClassName);

            Type classType = ReflectionHelper.FindTypeInAnyAssembly(fullClassName);

            object instance = Activator.CreateInstance(classType, constructorParameters);

            Logging.Log(fullTypeName, "Type found as [{0}]. Instance created.", classType.Name);

            return (TType)instance;
        }

        public static TType CreateInstance<TType>(params object[] constructorParameters) {
            Type classType = typeof(TType);

            Logging.Log(fullTypeName, "Preparing to create an instance of [{0}].", classType.Name);

            object instance = Activator.CreateInstance(classType, constructorParameters);

            Logging.Log(fullTypeName, "Type found as [{0}]. Instance created.", classType.Name);

            return (TType)instance;
        }

        public static string GenerateTransformHierarchyString(Transform transform) {
            //TODO: inverse this string later on

			Assert.IsNull (transform, "Cannot generate transform hierarchy when the passed transform is null!");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(transform.name);
			stringBuilder.Append("<-");
            Transform transformParent = transform.parent;
			while(transformParent != null) {
				stringBuilder.Append(transformParent.name);
				stringBuilder.Append("<-");
                transformParent = transformParent.parent;
			}
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString();
		}

		public static void DisposeAndMakeDefault<TDisposable>(ref TDisposable disposable) where TDisposable : IDisposable {
			if(disposable != null) {
				disposable.Dispose();
				disposable = default(TDisposable);
			}
		}

    }

}