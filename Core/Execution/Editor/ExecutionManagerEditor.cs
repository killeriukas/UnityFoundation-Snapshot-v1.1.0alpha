using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace TMI.Core {

    [CustomEditor(typeof(ExecutionManager))]
    public class ExecutionManagerEditor : Editor {

        //TODO: implement this one later on!!!!!!!!!!!!!!!!!!!!!

        //TODO: something crashes sometimes here, but don't have time to investigate at the moment

     //   private NotificationManager.Editor editor;

        //private Dictionary<Type, bool> foldoutByType = new Dictionary<Type, bool>();

        //private void OnEnable() {
        //    ExecutionManager executionManager = (ExecutionManager)target;
        //    //editor = new NotificationManager.Editor(notificationManager);

        //    //IEnumerable<Type> allNotificationTypes = editor.GetAllRegisteredNotificationTypes();
        //    //foreach(Type type in allNotificationTypes) {
        //    //    foldoutByType.Add(type, false);
        //    //}
        //}

        //public override void OnInspectorGUI() {
        //    //IEnumerable<Type> allNotificationTypes = editor.GetAllRegisteredNotificationTypes();
        //    //IOrderedEnumerable<Type> allNotificationTypesOrdered = allNotificationTypes.OrderBy(x => x.Name);

        //    //EditorGUILayout.HelpBox("Available: " + allNotificationTypesOrdered.Count(), MessageType.Info);

        //    //foreach(Type notificationType in allNotificationTypesOrdered) {
        //    //    IEnumerable<Type> listenerTypes = editor.GetListenerTypesByNotificationType(notificationType);

        //    //    foldoutByType[notificationType] = EditorGUILayout.Foldout(foldoutByType[notificationType], notificationType.Name + " (" + listenerTypes.Count() + ")", true);
        //    //    if(foldoutByType[notificationType]) {
        //    //        ++EditorGUI.indentLevel;
        //    //        IOrderedEnumerable<Type> orderedListenerTypes = listenerTypes.OrderBy(x => x.Name);
        //    //        foreach(Type listenerType in orderedListenerTypes) {
        //    //            EditorGUILayout.LabelField(listenerType.Name);
        //    //        }
        //    //        --EditorGUI.indentLevel;
        //    //    }
        //    //}
        //}

        //private void OnDisable() {
        //    foldoutByType.Clear();
        //}

    }

}