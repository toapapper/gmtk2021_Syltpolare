using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{
    public abstract class PlayableScriptingMarker : Marker
    {
        public ExposedReference<ConditionBehaviour> ConditionSource;

        public abstract void ProcessMixerFrame(PlayableDirector playableDirector, Playable playable, FrameData info, object playerData);
    }
}
