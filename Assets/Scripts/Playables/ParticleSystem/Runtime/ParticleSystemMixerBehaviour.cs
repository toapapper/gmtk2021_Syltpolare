using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Celezt.Timeline
{
    [System.Serializable]
    public class ParticleSystemMixerBehaviour : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            ParticleSystem trackBinding = playerData as ParticleSystem;
            bool isPLaying = false;

            if (!trackBinding)
                return;

            int inputCount = playable.GetInputCount();

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);

                if (inputWeight > 0.0f)
                {
                    ScriptPlayable<ParticleSystemBehaviour> inputPlayable = (ScriptPlayable<ParticleSystemBehaviour>)playable.GetInput(i);

                    ParticleSystemBehaviour input = inputPlayable.GetBehaviour();
                    isPLaying = true;
                }
            }

            if (isPLaying && !trackBinding.isPlaying)
                trackBinding.Play();
            else if (!isPLaying && !trackBinding.isStopped)
                trackBinding.Stop();
        }
    }
}
