using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{
    [CustomStyle("Goto")]
    public class GotoMarker : PlayableScriptingMarker
    {
        public override string name => "Wait";

        public bool Invert = true;
        public bool EmitOnce = true;

        private bool _oldCondition;

        public override void ProcessMixerFrame(PlayableDirector playableDirector, Playable playable, FrameData info, object playerData)
        {
            ConditionBehaviour conditionBehaviour = ConditionSource.Resolve(playable.GetGraph().GetResolver());
            bool condition = conditionBehaviour.Condition;

            if (conditionBehaviour == null)
                return;

            if (!conditionBehaviour.isActiveAndEnabled)
                return;

            if ((Invert ? condition : !condition) && (EmitOnce ? _oldCondition != condition : true))
                playableDirector.time = time;

            _oldCondition = condition;
        }
    }
}
