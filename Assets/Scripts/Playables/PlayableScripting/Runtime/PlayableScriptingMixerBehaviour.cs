using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace Celezt.Timeline
{
    [System.Serializable]
    public class PlayableScriptingMixerBehaviour : PlayableBehaviour
    {
        public List<PlayableScriptingMarker> Markers = new List<PlayableScriptingMarker>();

        private List<(Playable, PlayableScriptingBehaviour)> _oldBehaviours = new List<(Playable, PlayableScriptingBehaviour)>();

        private PlayableDirector _playableDirector;

        public override void OnPlayableCreate(Playable playable)
        {
            _playableDirector = playable.GetGraph().GetResolver() as PlayableDirector;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (Application.isPlaying)
            {
                ProcessClips(playable, info, playerData);
                ProcessMarkers(playable, info, playerData);
            }
        }
        private void ProcessClips(Playable playable, FrameData info, object playerData)
        {
            List<(Playable, PlayableScriptingBehaviour)> currentBehaviours = new List<(Playable, PlayableScriptingBehaviour)>();

            int inputCount = playable.GetInputCount();
            for (int i = 0; i < inputCount; i++)
            {
                if (playable.GetInputWeight(i) > 0.0f)
                {
                    PlayableScriptingBehaviour input = null;

                    Playable play = playable.GetInput(i);

                    if (GetBehaviour<WhileBehaviour>(play, out input)) { }
                    else if (GetBehaviour<IfBehaviour>(play, out input)) { }

                    if (input != null)
                    {
                        input.ProcessMixerFrame(_playableDirector, play, info, playerData);
                        currentBehaviours.Add((play, input));
                    }
                }
            }

            // Remove until only clips no longer inside the scope is left.
            for (int i = 0; i < currentBehaviours.Count; i++)
                _oldBehaviours.Remove(currentBehaviours[i]);

            // Calls after exiting a clip.
            for (int i = 0; i < _oldBehaviours.Count; i++)
                _oldBehaviours[i].Item2.PostMixerFrame(_playableDirector, _oldBehaviours[i].Item1, info, playerData);

            _oldBehaviours = currentBehaviours;
        }

        private void ProcessMarkers(Playable playable, FrameData info, object playerData)
        {
            Debug.Log(Markers.Count);
            for (int i = 0; i < Markers.Count; i++)
                Markers[i].ProcessMixerFrame(_playableDirector, playable, info, playerData);
        }

        private static bool GetBehaviour<T>(Playable playable, out PlayableScriptingBehaviour behaviour) where T : PlayableScriptingBehaviour, new()
        {
            if (playable.GetPlayableType().Equals(typeof(T)))
            {
                behaviour = ((ScriptPlayable<T>)playable).GetBehaviour();
                return true;
            }

            behaviour = null;
            return false;
        }
    }
}