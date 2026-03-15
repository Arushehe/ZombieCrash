using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform carTransform;

    [Range(1,10)]
    public float followSpeed = 5;

    Vector3 initialCameraPosition;

    void Start()
    {
        initialCameraPosition = transform.position;
    }

    void LateUpdate()
    {
        Vector3 targetPos = new Vector3(
            carTransform.position.x,
            initialCameraPosition.y,
            carTransform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
}