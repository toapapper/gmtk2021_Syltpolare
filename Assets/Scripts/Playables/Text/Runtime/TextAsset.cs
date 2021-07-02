using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{
    public class TextAsset : PlayableAsset, ITimelineClipAsset
    {
        public TextBehaviour Template = new TextBehaviour();

        public ClipCaps clipCaps => ClipCaps.Blending | ClipCaps.SpeedMultiplier;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<TextBehaviour> playable = ScriptPlayable<TextBehaviour>.Create(graph, Template);
            return playable;
        }
    }
}
