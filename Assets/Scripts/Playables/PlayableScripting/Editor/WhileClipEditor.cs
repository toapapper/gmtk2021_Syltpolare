using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Timeline;

namespace Celezt.Timeline
{
    [CustomEditor(typeof(WhileClip)), CanEditMultipleObjects]
    public class WhileClipEditor : Editor
    {
        private SerializedProperty _conditionSource;

        private void OnEnable()
        {
            _conditionSource = serializedObject.FindProperty("Template.ConditionSource");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_conditionSource);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
