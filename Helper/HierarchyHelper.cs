using UnityEngine;
using TMI.Core;
using System.Collections.Generic;
using System.Linq;

namespace TMI.Helper {

	public static class HierarchyHelper {

		public static TBehaviour InstantiateAndSetupBehaviour<TBehaviour>(IInitializer initializer, TBehaviour prefab) where TBehaviour : UnityBehaviour {
			Assert.IsNull(initializer, "Initializer cannot be null!");

            TBehaviour behaviour = Instantiate(prefab);
			behaviour.Setup(initializer);

			return behaviour;
		}

        public static TBehaviour InstantiateAndSetupBehaviour<TBehaviour>(
            IInitializer initializer,
            TBehaviour prefab,
            Transform parent,
            bool worldPositionStays) where TBehaviour : UnityBehaviour {

            Assert.IsNull(initializer, "Initializer cannot be null!");

            TBehaviour behaviour = Instantiate(prefab, parent, worldPositionStays);
            behaviour.Setup(initializer);

            return behaviour;
        }

        public static TBehaviour Instantiate<TBehaviour>(TBehaviour prefab, Transform parent, bool worldPositionStays) where TBehaviour : Object {
            Assert.IsNull(prefab, "Prefab cannot be null!");

            TBehaviour behaviour = Object.Instantiate(prefab, parent, worldPositionStays);
            Assert.IsNull(behaviour, "Instantiated object doesn't have a behaviour of type: " + typeof(TBehaviour));

            return behaviour;
        }

        public static TBehaviour Instantiate<TBehaviour>(TBehaviour prefab) where TBehaviour : Object {
			Assert.IsNull(prefab, "Prefab cannot be null!");

			TBehaviour behaviour = Object.Instantiate(prefab);
			Assert.IsNull(behaviour, "Instantiated object doesn't have a behaviour of type: " + typeof(TBehaviour));

			return behaviour;
		}

        public static void SetLayerRecursively(this GameObject go, int layerNumber) {
            if(go == null) return;
            foreach(Transform trans in go.GetComponentsInChildren<Transform>(true)) {
                trans.gameObject.layer = layerNumber;
            }
        }

        public static TComponent[] GetComponentsInChildrenOnly<TComponent>(this Component component, bool includeInactive) {
            List<TComponent> allComponents = new List<TComponent>();
            foreach(Transform childTransform in component.transform) {
                TComponent[] foundComponents = childTransform.GetComponentsInChildren<TComponent>(includeInactive);
                allComponents.AddRange(foundComponents);
            }
            return allComponents.ToArray();
        }

        public static IEnumerable<TComponent> GetComponentsInChildrenExcludingOne<TComponent, TComponentExclude>(Component component) where TComponentExclude : Component {
            //get all ui components in children and parent itself
            List<TComponent> allComponents = component.GetComponentsInChildren<TComponent>(true).ToList();
            return FilterComponentList<TComponent, TComponentExclude>(allComponents, component);
        }

        public static IEnumerable<TComponent> GetComponentsInChildrenOnlyExcludingOne<TComponent, TComponentExclude>(Component component) where TComponentExclude : Component {
            //get all ui components in children only
            List<TComponent> allComponents = component.GetComponentsInChildrenOnly<TComponent>(true).ToList();
            return FilterComponentList<TComponent, TComponentExclude>(allComponents, component);
        }

        private static IEnumerable<TComponent> FilterComponentList<TComponent, TComponentExclude>(List<TComponent> allComponents, Component component) where TComponentExclude : Component {

            //get all components within the exclude component
            List<TComponent> allExcludedComponents = new List<TComponent>();

            TComponentExclude[] foundExcludedComponents = component.GetComponentsInChildrenOnly<TComponentExclude>(true);
            foreach(TComponentExclude compExclude in foundExcludedComponents) {
                TComponent[] compExcludeComponents = compExclude.GetComponentsInChildren<TComponent>(true);
                allExcludedComponents.AddRange(compExcludeComponents);
            }

            //exclude all selected components from all ui components
            return allComponents.Except(allExcludedComponents);
        }

    }


}

