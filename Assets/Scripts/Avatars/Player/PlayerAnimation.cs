using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private readonly int _runID = Animator.StringToHash("Run");

    public void OnHorizontalMove(InputContext context)
    {
        if (context.Value == 0)
            _animator.SetBool(_runID, false);
        else
            _animator.SetBool(_runID, true);
    }
}
