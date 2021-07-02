using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace Celezt.Timeline
{
    [TrackColor(0.5f, 0.5f, 0.5f)]
    [TrackClipType(typeof(PlayableScriptingAsset))]
    public class PlayableScriptingTrack : TrackAsset
    {
        public PlayableScriptingMixerBehaviour Template = new PlayableScriptingMixerBehaviour();

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (TimelineClip clip in GetClips())
            {
                if (clip.asset is PlayableScriptingAsset)
                {
                    PlayableScriptingAsset asset = clip.asset as PlayableScriptingAsset;
                    PlayableScriptingBehaviour behaviour = asset.BehaviourReference;
                    ConditionBehaviour condition = behaviour.ConditionSource.Resolve(graph.GetResolver());
                    clip.displayName = asset.name + " " + (condition != null ? condition.name : "");

                    behaviour.StartTime = clip.start;
                    behaviour.EndTime = clip.end;
                }
            }

            CreateMarkers();

            ScriptPlayable<PlayableScriptingMixerBehaviour> playable = ScriptPlayable<PlayableScriptingMixerBehaviour>.Create(graph, Template, inputCount);
            return playable;
        }

        public void CreateMarkers()
        {
            Template.Markers.Clear();
            foreach (IMarker marker in GetMarkers())
                if (marker is PlayableScriptingMarker)
                    Template.Markers.Add(marker as PlayableScriptingMarker);
        }
    }
}
