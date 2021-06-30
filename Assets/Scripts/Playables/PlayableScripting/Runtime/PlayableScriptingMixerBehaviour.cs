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

            // -------------------------------------------------------
            //                   While inside clip
            // -------------------------------------------------------
            int inputCount = playable.GetInputCount();
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                if (inputWeight > 0.0f)
                {
                    Playable play = playable.GetInput(i);
                    if (play.GetPlayableType().Equals(typeof(WhileBehaviour)))
                    {
                        WhileBehaviour input = ((ScriptPlayable<WhileBehaviour>)play).GetBehaviour();
                        ConditionBehaviour conditionBehaviour = input.ConditionSource.Resolve(playable.GetGraph().GetResolver());

                        if (conditionBehaviour == null)
                            return;

                        if (input.OnlyEligibleOnce && (input.Invert ? conditionBehaviour.Condition : !conditionBehaviour.Condition))
                            input.HasBeenEligible = true;

                        currentBehaviours.Add(input);
                    }
                }
            }

            // ------------------------------------------------------
            //                  When exit clip
            // ------------------------------------------------------

            // Remove until only clips no longer inside the scope is left.
            for (int i = 0; i < currentBehaviours.Count; i++)
                _oldBehaviours.Remove(currentBehaviours[i]);

            for (int i = 0; i < _oldBehaviours.Count; i++)
            {
                switch (_oldBehaviours[i])
                {
                    case WhileBehaviour input:
                        ConditionBehaviour conditionBehaviour = input.ConditionSource.Resolve(playable.GetGraph().GetResolver());

                        if (conditionBehaviour == null)
                            break;

                        if (input.OnlyEligibleOnce ? input.HasBeenEligible : false)
                        {
                            input.HasBeenEligible = false;
                            break;
                        }

                        if (input.Invert ? !conditionBehaviour.Condition : conditionBehaviour.Condition)
                            _playableDirector.time = input.StartTime;
                        break;
                    default:
                        break;
                }
            }

            _oldBehaviours = currentBehaviours;
        }

        private void ProcessMarkers(Playable playable, FrameData info, object playerData)
        {
            for (int i = 0; i < Markers.Count; i++)
            {
                switch (Markers[i])
                {
                    case GotoMarker input:
                        ConditionBehaviour conditionBehaviour = input.ConditionSource.Resolve(playable.GetGraph().GetResolver());
                        bool condition = conditionBehaviour.Condition;

                        if (conditionBehaviour == null)
                            return;

                        if ((input.Invert ? !condition : condition) &&
                            (input.EmitOnce ? input.OldCondition != condition : true))
                        {
                            _playableDirector.time = input.time;
                        }

                        input.OldCondition = condition;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}