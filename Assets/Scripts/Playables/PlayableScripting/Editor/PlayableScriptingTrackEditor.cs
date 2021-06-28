using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Timeline;

namespace Celezt.Timeline
{
    [CustomEditor(typeof(PlayableScriptingTrack)), CanEditMultipleObjects]
    public class PlayableScriptingTrackEditor : Editor
    {
        private void OnEnable()
        {

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            serializedObject.ApplyModifiedProperties();
        }
    }
}