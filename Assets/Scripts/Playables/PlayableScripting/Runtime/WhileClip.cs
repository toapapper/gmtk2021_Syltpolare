using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{
    public class WhileClip : PlayableAsset, ITimelineClipAsset
    {
        public WhileBehaviour Template = new WhileBehaviour();

        public ClipCaps clipCaps => ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<WhileBehaviour> playable = ScriptPlayable<WhileBehaviour>.Create(graph, Template);
            return playable;
        }
    }
}
