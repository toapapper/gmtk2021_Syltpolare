using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Timeline;

namespace Celezt.Timeline
{
    [CustomEditor(typeof(TextTrack)), CanEditMultipleObjects]
    public class TextTrackEditor : Editor
    {
        private SerializedProperty _fadeIn;

        private void OnEnable()
        {
            _fadeIn = serializedObject.FindProperty("Template.FadeIn");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_fadeIn);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
