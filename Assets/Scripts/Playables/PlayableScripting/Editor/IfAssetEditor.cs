using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Timeline;

namespace Celezt.Timeline
{
    [CustomEditor(typeof(IfAsset)), CanEditMultipleObjects]
    public class IfAssetEditor : Editor
    {
        private SerializedProperty _conditionSource;
        private SerializedProperty _invert;
        private SerializedProperty _onlyEligibleOnce;

        private void OnEnable()
        {
            _conditionSource = serializedObject.FindProperty("Template.ConditionSource");
            _invert = serializedObject.FindProperty("Template.Invert");
            _onlyEligibleOnce = serializedObject.FindProperty("Template.OnlyEligibleOnce");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_conditionSource);
            EditorGUILayout.PropertyField(_invert);
            EditorGUILayout.PropertyField(_onlyEligibleOnce);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
