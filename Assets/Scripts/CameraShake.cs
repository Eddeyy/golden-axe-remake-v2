using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    private Transform _cameraTransform;

    [SerializeField] private float _shakeMagnitude = 0.5f;

    private float cameraZPosition;

    private Vector3 _originalPosition;

    public bool isShaking = false;
    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isShaking)
            ShakeCamera();
    }

    public void ShakeCamera()
    {

        var shakeBy = Random.insideUnitSphere * _shakeMagnitude;
        shakeBy.z = 0;
        _cameraTransform.position = _originalPosition + shakeBy;


        if(_shakeMagnitude <= 0)
        {
            _shakeMagnitude = 0;
        }
        
    }

    public void StartShake(float magnitutde)
    {
        isShaking = true;
        _shakeMagnitude = magnitutde;
        _originalPosition = _cameraTransform.position;
    }

    public void StopShake()
    {
        isShaking = false;
        _cameraTransform.position = _originalPosition;
    }
}
