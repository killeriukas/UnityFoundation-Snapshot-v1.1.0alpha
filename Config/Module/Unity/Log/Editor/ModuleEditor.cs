using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TMI.ConfigManagement.Unity.Log {
    
    //[CustomEditor(typeof(LoggingConfig))]
    public class ModuleEditor : Editor {

        [SerializeField]
        private VisualTreeAsset outputInspector;
        
        [SerializeField]
        private VisualTreeAsset categoryInspector;
        
        [SerializeField]
        private VisualTreeAsset moduleInspector;
        
        public override VisualElement CreateInspectorGUI() {
            //DrawDefaultInspector();
            
          //  serializedObject.Update();

            //Selection.activeObject;
            
            VisualElement newInspector = moduleInspector.Instantiate();
            
            // VisualElement defaultInspector = new VisualElement();
            //InspectorElement.FillDefaultInspector(defaultInspector, serializedObject, this);
            // newInspector.Add(defaultInspector);
            
            // ListView outputListView = newInspector.Q<ListView>("output-list");
            // outputListView.makeItem = () => outputInspector.Instantiate();

            ListView categoryListView = newInspector.Q<ListView>("category-list");
            categoryListView.makeItem = () => categoryInspector.Instantiate();
          

            // SerializedProperty logCategoryNameListProperty = serializedObject.FindProperty("logCategoryNameList");
            //
            //
            //
            //
            // int categoryNameListSize = logCategoryNameListProperty.arraySize;
            //
            // for(int i = 0; i < categoryNameListSize; ++i) {
            //     SerializedProperty toggleText = logCategoryNameListProperty.GetArrayElementAtIndex(i);
            //     
            //     TemplateContainer entryTemplate = entryInspector.Instantiate();
            //     Toggle toggle = entryTemplate.Q<Toggle>("entry-toggle");
            //     toggle.text = toggleText.stringValue;
            //
            //   //  toggle.BindProperty(toggleText);
            //
            //     scrollView.Add(entryTemplate);
            // }
            
            return newInspector;
        }

    }

    
}

