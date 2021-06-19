using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public bool IsStarted => _isStarted;

    [SerializeField] private CinemachineTargetGroup _cmFollow;

    private List<Transform> _targets = new List<Transform>();

    private GameObject _empty;

    private bool _emptyIsUsed;
    private bool _isStarted;

    public int Count => _emptyIsUsed ? _targets.Count - 1 : _targets.Count;

    public Transform First => (_targets.Count > 0 && !_emptyIsUsed) ? _targets[0] : null;
    public Transform Last => (_targets.Count > 0 && !_emptyIsUsed) ? _targets.Last() : null;

    /// <summary>
    /// Remove all members.
    /// </summary>
    public void Clear()
    {
        if (_targets.Count > 0 && !_emptyIsUsed && _empty != null)
        {
            Transform firstTransform = First;
            _empty.transform.position = firstTransform.position;

            for (int i = 0; i < _targets.Count; i++)
                _cmFollow.RemoveMember(_targets[i]);

            _targets.Clear();

            AddMember(_empty.transform);
            _emptyIsUsed = true;
        }
    }

    public bool Contains(Transform trans) => _targets.Contains(trans);

    /// <summary>
    /// Add transform target to camera targets.
    /// </summary>
    /// <param name="trans"></param>
    /// <returns>If the transform does not already exist</returns>
    public bool AddMember(Transform trans)
    {
        if (_emptyIsUsed)
        {
            RemoveMember(_empty.transform);
            _emptyIsUsed = false;
        }

        if (!_targets.Contains(trans))
        {
            _targets.Add(trans);
            _cmFollow.AddMember(trans, 1, 0);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Remove transform target from camera targets.
    /// </summary>
    /// <param name="trans">Transform.</param>
    /// <returns>If transform exist.</returns>
    public bool RemoveMember(Transform trans)
    {
        if (!_emptyIsUsed && _targets.Count == 1 && _empty != null)
        {
            Transform firstTransform = First;
            _empty.transform.position = firstTransform.position;

            AddMember(_empty.transform);
            _emptyIsUsed = true;
        }

        if (_targets.Remove(trans))
        {
            _cmFollow.RemoveMember(trans);

            return true;
        }

        return false;
    }

    private void Awake()
    {
        for (int i = 0; i < _cmFollow.m_Targets.Length; i++)
            _targets.Add(_cmFollow.m_Targets[i].target);
    }

    private void Start()
    {
        _empty = new GameObject("EmptyPivot");

        _isStarted = true;
    }

    private void OnDestroy()
    {
        Destroy(_empty);
    }
}
