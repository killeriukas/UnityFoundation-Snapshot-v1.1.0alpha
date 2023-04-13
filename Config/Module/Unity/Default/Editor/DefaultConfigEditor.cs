using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TMI.ConfigManagement.Unity {

    //[CustomEditor(typeof(DefaultConfig))]
    public class DefaultConfigEditor : Editor
    {
        [SerializeField]
        private VisualTreeAsset defaultInspector;
        
        [SerializeField]
        private VisualTreeAsset defaultEntryInspector;
        
        public override VisualElement CreateInspectorGUI() {
            
            //serializedObject
          //  defaultInspector.CloneTree();
            
            //defaultInspector.CloneTree()
            
            // Create a new VisualElement to be the root of our inspector UI
           // VisualElement newInspector = new VisualElement();

            // Add a simple label
          //  newInspector.Add(new Label("This is a custom inspector"));

            // Load and clone a visual tree from UXML
           // defaultInspector.CloneTree(newInspector);
        
           
           TemplateContainer newInspector = defaultInspector.Instantiate();
           ListView listView = newInspector.Q<ListView>("config-list");

           listView.makeItem = () =>
           {
               TemplateContainer newEntryContainer = defaultEntryInspector.Instantiate();
               ObjectField objectField = newEntryContainer.Q<ObjectField>();

               objectField.label = "Testing fresh name";
               
               return newEntryContainer;
           };

           // listView.Add(new Label("Test Addition"));
         // listView.makeItem = () => new Label("Test addition");

            //listView.itemsSource;
          
           // Return the finished inspector UI
            return newInspector;
        }

    }

    
}

