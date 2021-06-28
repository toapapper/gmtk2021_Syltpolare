using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Celezt.Timeline
{
    [System.Serializable]
    public class PlayableScriptingMixerBehaviour : PlayableBehaviour
    {
        private PlayableDirector _playableDirector;

        private List<PlayableBehaviour> _oldBehaviours = new List<PlayableBehaviour>();

        public override void OnPlayableCreate(Playable playable)
        {
            _playableDirector = playable.GetGraph().GetResolver() as PlayableDirector;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (Application.isPlaying)
            {
                int inputCount = playable.GetInputCount();

                List<PlayableBehaviour> currentBehaviours = new List<PlayableBehaviour>();

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

                        if (!condition.Condition)
                            _playableDirector.time = (_oldBehaviours[i] as WhileBehaviour).StartTime;
                    }
                }

                _oldBehaviours = currentBehaviours;
            }
        }
    }
}