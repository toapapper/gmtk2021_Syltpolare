using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Celezt.Timeline
{
    public class IfAsset : PlayableScriptingAsset
    {
        public override string name => "If";
        public override PlayableScriptingBehaviour BehaviourReference => Template;

        public IfBehaviour Template = new IfBehaviour();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<IfBehaviour> playable = ScriptPlayable<IfBehaviour>.Create(graph, Template);
            return playable;
        }
    }
}
