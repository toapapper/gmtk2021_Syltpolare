using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneHandler : MonoBehaviour
{
    private PlayableDirector _currentDirector;

    public void GetDirector(PlayableDirector director)
    {
        _currentDirector = director;
    }

    public void OnSkipCurrent()
    {
        _currentDirector.time = _currentDirector.playableAsset.duration;
    }
}
