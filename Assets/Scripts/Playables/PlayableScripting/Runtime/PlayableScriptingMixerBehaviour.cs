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

        public override void OnPlayableCreate(Playable playable)
        {
            _playableDirector = playable.GetGraph().GetResolver() as PlayableDirector;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            int inputCount = playable.GetInputCount();

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);

                if (inputWeight > 0.0f)
                {
                    ScriptPlayable<WhileBehaviour> inputPlayable = (ScriptPlayable<WhileBehaviour>)playable.GetInput(i);
                    WhileBehaviour input = inputPlayable.GetBehaviour();
                    ConditionBehaviour condition = input.From.Resolve(playable.GetGraph().GetResolver());

                    if (condition != null)
                    {
                        if (!condition.Condition)
                        {
                            if (_playableDirector.time + info.deltaTime >= input.EndTime)
                                _playableDirector.time = input.StartTime;
                        }
                    }
                }
            }
        }
    }
}