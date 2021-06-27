using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Timeline;

namespace Celezt.Timeline
{
    [CustomEditor(typeof(TextClip)), CanEditMultipleObjects]
    public class TextClipEditor : Editor
    {
        private SerializedProperty _text;
        private SerializedProperty _fontColor;

        private void OnEnable()
        {
            _text = serializedObject.FindProperty("Template.Text");
            _fontColor = serializedObject.FindProperty("Template.FontColor");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_text);
            EditorGUILayout.PropertyField(_fontColor);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
