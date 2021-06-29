using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace Celezt.Timeline
{
    [TrackColor(0.5f, 0.5f, 0.5f)]
    [TrackClipType(typeof(WhileClip))]
    public class PlayableScriptingTrack : TrackAsset
    {
        public PlayableScriptingMixerBehaviour Template = new PlayableScriptingMixerBehaviour();

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (TimelineClip clip in GetClips())
            {
                if (clip.asset is WhileClip)
                {
                    WhileClip whileClip = clip.asset as WhileClip;
                    WhileBehaviour behaviour = whileClip.Template;
                    ConditionBehaviour condition = behaviour.ConditionSource.Resolve(graph.GetResolver());
                    clip.displayName = "While " + (condition != null ? condition.name : "");

                    behaviour.StartTime = clip.start;
                    behaviour.EndTime = clip.end;
                }
            }

            Template.Markers.Clear();
            foreach (IMarker marker in GetMarkers())
                Template.Markers.Add(marker);

            ScriptPlayable<PlayableScriptingMixerBehaviour> playable = ScriptPlayable<PlayableScriptingMixerBehaviour>.Create(graph, Template, inputCount);
            return playable;
        }
    }
}
