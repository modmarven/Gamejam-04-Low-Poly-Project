using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookSetting : MonoBehaviour
{

    [Header("Mouse Look Setting")]
    public Transform playerBody;
    public float rotationSpeed = 100f;

    private Movement inputSystem;

    void Awake()
    {
        inputSystem = new Movement();
    }

    
    void FixedUpdate()
    {
        HandlingMouseLook();
    }

    private void HandlingMouseLook()
    {
        Vector2 inputLook = inputSystem.Player.Look.ReadValue<Vector2>();

        playerBody.transform.rotation *= Quaternion.AngleAxis(inputLook.x * rotationSpeed * Time.fixedDeltaTime, Vector3.up);
        playerBody.transform.rotation *= Quaternion.AngleAxis(inputLook.y * rotationSpeed * Time.fixedDeltaTime, Vector3.up);

        var angles = playerBody.transform.localEulerAngles;
        angles.z = 0;

        var angle = playerBody.transform.localEulerAngles.x;

        if (angle > 180 && angle < 360) angles.x = 340;
        else if (angle < 180 && angle > 40) angles.x = 40;

        transform.rotation = Quaternion.Euler(0, playerBody.transform.rotation.eulerAngles.y, 0);
        playerBody.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

}