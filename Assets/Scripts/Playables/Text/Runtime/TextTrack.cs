using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace Celezt.Timeline
{
    [TrackColor(1, 0, 1)]
    [TrackBindingType(typeof(Text))]
    [TrackClipType(typeof(TextAsset))]
    public class TextTrack : TrackAsset
    {
        public TextMixerBehaviour Template = new TextMixerBehaviour();

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (TimelineClip clip in GetClips())
            {
                if (clip.asset is TextAsset)
                {
                    TextAsset textClip = clip.asset as TextAsset;
                    TextBehaviour behaviour = textClip.Template;
                    clip.displayName = behaviour.Text;
                }
            }

            ScriptPlayable<TextMixerBehaviour> playable = ScriptPlayable<TextMixerBehaviour>.Create(graph, Template, inputCount);
            return playable;
        }
    }
}
