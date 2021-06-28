using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Celezt.Timeline
{
    [System.Serializable]
    public class TextMixerBehaviour : PlayableBehaviour
    {
        [Tooltip("Fade between clips.")] public bool FadeIn = true;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Text trackBinding = playerData as Text;
            string finalText = "";
            float finalAlpha = 0.0f;
            Color finalColor = Color.black;

            if (!trackBinding)
                return;

            bool activate = false;
            int inputCount = playable.GetInputCount();
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);

                if (inputWeight > 0.0f)
                {
                    activate = true;

                    ScriptPlayable<TextBehaviour> inputPlayable = (ScriptPlayable<TextBehaviour>)playable.GetInput(i);

                    TextBehaviour input = inputPlayable.GetBehaviour();
                    finalText = input.Text;
                    finalAlpha = input.FontColor.a * (FadeIn ? inputWeight : 1);
                    finalColor += input.FontColor * inputWeight;
                }
            }

            trackBinding.gameObject.SetActive(activate);

            trackBinding.text = finalText;
            trackBinding.color = new Color(finalColor.r, finalColor.g, finalColor.b, finalAlpha);
        }
    }
}
