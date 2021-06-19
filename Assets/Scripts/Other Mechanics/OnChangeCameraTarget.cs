using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChangeCameraTarget : MonoBehaviour
{
    [SerializeField] Transform _target;

    private CameraManager _cameraManager;

    public void OnChangeTarget()
    {
        _cameraManager.Clear();
        _cameraManager.AddMember(_target);

        StartCoroutine(WaitUntilOnTarget());
    }

    private void Start()
    {
        _cameraManager = Camera.main.GetComponent<CameraManager>();
    }

    private IEnumerator WaitUntilOnTarget()
    {
        Debug.Log("Started");

        Vector3 oldDamping = _cameraManager.Damping;
        _cameraManager.Damping = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        yield return new WaitUntil(() => _cameraManager.Velocity.magnitude < 1.0f);
        _cameraManager.Damping = oldDamping;

        Debug.Log("Done");
    }
}
