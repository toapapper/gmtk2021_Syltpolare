using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Celezt.Timeline
{
    [System.Serializable]
    public class IfBehaviour : PlayableScriptingBehaviour
    {
        public bool Invert;
        [Tooltip("Only need the condition to be eligible once whiles inside the clip.")]
        public bool OnlyEligibleOnce = true;

        private bool _hasBeenEligible = false;

        public override void ProcessMixerFrame(PlayableDirector playableDirector, Playable playable, FrameData info, object playerData)
        {
            ConditionBehaviour conditionBehaviour = ConditionSource.Resolve(playable.GetGraph().GetResolver());

            if (conditionBehaviour == null)
                return;

            if (OnlyEligibleOnce && (Invert ? conditionBehaviour.Condition : !conditionBehaviour.Condition))
            {
                OnlyEligibleOnce = true;
                playableDirector.time = EndTime;
            }
        }

        public override void PostMixerFrame(PlayableDirector playableDirector, Playable playable, FrameData info, object playerData)
        {
            if (OnlyEligibleOnce ? _hasBeenEligible : false)
            {
                _hasBeenEligible = false;
                return;
            }
        }
    }
}

