using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Celezt.Timeline
{
    [System.Serializable]
    public class PlayableScriptingBehaviour : PlayableBehaviour
    {
        public double StartTime { get; set; }
        public double EndTime { get; set; }

        public ExposedReference<ConditionBehaviour> ConditionSource;

        public virtual void ProcessMixerFrame(PlayableDirector playableDirector, Playable playable, FrameData info, object playerData) { }
        public virtual void PostMixerFrame(PlayableDirector playableDirector, Playable playable, FrameData info, object playerData) { }
    }
}
