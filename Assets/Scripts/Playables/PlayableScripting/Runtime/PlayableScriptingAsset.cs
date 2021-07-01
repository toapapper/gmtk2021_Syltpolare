using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{
    public abstract class PlayableScriptingAsset : PlayableAsset, ITimelineClipAsset
    {
        public abstract PlayableScriptingBehaviour BehaviourReference { get; }
        public ClipCaps clipCaps => ClipCaps.None;
    }
}