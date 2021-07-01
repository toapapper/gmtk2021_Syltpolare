using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Celezt.Timeline
{
    [System.Serializable]
    public class WhileBehaviour : PlayableScriptingBehaviour
    {
        public bool Invert = true;
        [Tooltip("Only need the condition to be eligible once whiles inside the clip.")]
        public bool OnlyEligibleOnce = true;

        private bool _hasBeenEligible;

        public override void ProcessMixerFrame(PlayableDirector playableDirector, Playable playable, FrameData info, object playerData)
        {
            ConditionBehaviour conditionBehaviour = ConditionSource.Resolve(playable.GetGraph().GetResolver());

            if (conditionBehaviour == null)
                return;

            if (OnlyEligibleOnce && (Invert ? conditionBehaviour.Condition : !conditionBehaviour.Condition))
                _hasBeenEligible = true;
        }

        public override void PostMixerFrame(PlayableDirector playableDirector, Playable playable, FrameData info, object playerData)
        {
            ConditionBehaviour conditionBehaviour = ConditionSource.Resolve(playable.GetGraph().GetResolver());

            if (conditionBehaviour == null)
                return;

            if (OnlyEligibleOnce ? _hasBeenEligible : false)
            {
                _hasBeenEligible = false;
                return;
            }

            if (Invert ? !conditionBehaviour.Condition : conditionBehaviour.Condition)
                playableDirector.time = StartTime;
        }
    }
}
