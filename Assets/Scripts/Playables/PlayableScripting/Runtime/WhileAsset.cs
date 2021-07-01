using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{
    public class WhileAsset : PlayableScriptingAsset
    {
        public override PlayableScriptingBehaviour BehaviourReference => Template;

        public WhileBehaviour Template = new WhileBehaviour();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<WhileBehaviour> playable = ScriptPlayable<WhileBehaviour>.Create(graph, Template);
            return playable;
        }

        public override string ToString() => "While";
    }
}
