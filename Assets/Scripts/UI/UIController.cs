using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{
    [SerializeField] private UnityEvent<InputContext> _escapeEvent;

    private void Update()
    {
        if (Input.GetButtonDown(InputControls.Escape))
        {
            _escapeEvent.Invoke(new InputContext(1, InputContext.InputState.Performed));
        }
        else if (Input.GetButtonUp(InputControls.Escape))
        {
            _escapeEvent.Invoke(new InputContext(0, InputContext.InputState.Canceled));
        }
    }
}
