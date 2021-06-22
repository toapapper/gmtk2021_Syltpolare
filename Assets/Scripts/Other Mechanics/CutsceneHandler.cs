using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneHandler : MonoBehaviour
{
    private PlayableDirector _currentDirector;

    private bool _isSceneSkipped = true;
    private float _timeToSkipTo;

    public void GetDirector(PlayableDirector director)
    {
        _isSceneSkipped = false;
        _currentDirector = director;
    }

    public void OnSkipCurrent()
    {
        _currentDirector.time = _currentDirector.playableAsset.duration;
    }
}
