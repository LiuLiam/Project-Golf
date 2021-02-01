using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 0.2f;

    private void Update()
    {
        if(Input.GetMouseButton(0))
            RotateCamera(Input.GetAxis("Mouse X"));
    }

    public void RotateCamera(float xAxis)
    {
        transform.Rotate(Vector3.down, -xAxis * _rotationSpeed);
    }
}
