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
        public List<IMarker> Markers = new List<IMarker>();

        private List<PlayableBehaviour> _oldBehaviours = new List<PlayableBehaviour>();

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
            List<PlayableBehaviour> currentBehaviours = new List<PlayableBehaviour>();

            int inputCount = playable.GetInputCount();
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);

                if (inputWeight > 0.0f)
                {
                    ScriptPlayable<WhileBehaviour> inputPlayable = (ScriptPlayable<WhileBehaviour>)playable.GetInput(i);
                    WhileBehaviour input = inputPlayable.GetBehaviour();

                    currentBehaviours.Add(input);
                }
            }

            for (int i = 0; i < currentBehaviours.Count; i++)
                _oldBehaviours.Remove(currentBehaviours[i]);

            for (int i = 0; i < _oldBehaviours.Count; i++)
            {
                if (_oldBehaviours[i] is WhileBehaviour)
                {
                    WhileBehaviour input = _oldBehaviours[i] as WhileBehaviour;
                    ConditionBehaviour condition = input.ConditionSource.Resolve(playable.GetGraph().GetResolver());

                    if (condition == null)
                        return;

                    if (input.Invert ? !condition.Condition : condition.Condition)
                        _playableDirector.time = (_oldBehaviours[i] as WhileBehaviour).StartTime;
                }
            }

            _oldBehaviours = currentBehaviours;
        }

        private void ProcessMarkers(Playable playable, FrameData info, object playerData)
        {
            for (int i = 0; i < Markers.Count; i++)
            {
                if (Markers[i] is GotoMarker)
                {
                    GotoMarker input = Markers[i] as GotoMarker;
                    ConditionBehaviour condition = input.ConditionSource.Resolve(playable.GetGraph().GetResolver());

                    if (condition == null)
                        return;

                    if (input.Invert ? !condition.Condition : condition.Condition)
                    {
                        if (input.EmitOnce)
                            condition.Condition = input.Invert;

                        _playableDirector.time = input.time;
                    }
                }
            }
        }
    }
}