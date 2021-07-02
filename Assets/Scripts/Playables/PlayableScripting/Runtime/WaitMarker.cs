using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{
    [CustomStyle("Pause")]
    public class WaitMarker : PlayableScriptingMarker
    {
        public override string name => "Wait";

        public bool Invert = false;
        [Tooltip("Only need the condition to be eligible once whiles inside the clip.")]
        public bool OnlyEligibleOnce = true;

        private bool _hasBeenEligible;
        private bool _isWaiting;

        public override void ProcessMixerFrame(PlayableDirector playableDirector, Playable playable, FrameData info, object playerData)
        {
            ConditionBehaviour conditionBehaviour = ConditionSource.Resolve(playable.GetGraph().GetResolver());
            bool condition = conditionBehaviour.Condition;
            double currentTime = playableDirector.time;

            if (conditionBehaviour == null)
                return;

            if (!conditionBehaviour.isActiveAndEnabled)
                return;

            if (OnlyEligibleOnce && (Invert ? condition : !condition))
                _hasBeenEligible = true;

            if ((OnlyEligibleOnce ? _hasBeenEligible : true) && (Invert ? condition : !condition) && (currentTime >= time && currentTime - info.deltaTime < time))
            {
                _isWaiting = true;
                playableDirector.time = time;
            }
            else if (_isWaiting)
            {
                _isWaiting = false;
                _hasBeenEligible = false;
            }
        }
    }
}
