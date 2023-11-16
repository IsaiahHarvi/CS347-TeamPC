using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float amount = 0.02f;
    public float maxAmount = 0.06f;
    public float smoothAmount = 6f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        float movementX = -Input.GetAxis("Mouse X") * amount;
        float movementY = -Input.GetAxis("Mouse Y") * amount;

        movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);

        Quaternion rotationX = Quaternion.AngleAxis(movementX, Vector3.up);
        Quaternion rotationY = Quaternion.AngleAxis(movementY, Vector3.right);
        Quaternion finalRotation = initialRotation * rotationX * rotationY;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, finalRotation, Time.deltaTime * smoothAmount);
    }
}
