using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{
    public class ParticleSystemClip : PlayableAsset, ITimelineClipAsset
    {
        public ParticleSystemBehaviour Template = new ParticleSystemBehaviour();

        public ClipCaps clipCaps => ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<ParticleSystemBehaviour> playable = ScriptPlayable<ParticleSystemBehaviour>.Create(graph, Template);
            return playable;
        }
    }
}
