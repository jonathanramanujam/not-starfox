using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputAction movement;
    [SerializeField] InputAction fire;
    bool isFiring = false;
    [SerializeField] bool invertY = false;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float XRange = 1.8f;
    [SerializeField] float YRange= 1.5f;

    [SerializeField] float positionPitchFactor = -10f;
    [SerializeField] float controlPitchFactor = -30f;
    [SerializeField] float positionYawFactor = 20f;
    [SerializeField] float controlRollFactor = -40f;

    ParticleSystem[] lasers;

    void Start()
    {
        lasers = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        movement.Enable();
        fire.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        fire.Disable();
    }

    void Update()
    {
        float horizontalThrow, verticalThrow;
        AdjustPosition(out horizontalThrow, out verticalThrow);
        AdjustRotation(horizontalThrow, verticalThrow);
        ProcessFire();
    }

    private void AdjustPosition(out float horizontalThrow, out float verticalThrow)
    {
        horizontalThrow = movement.ReadValue<Vector2>().x;
        if (invertY)
        {
            verticalThrow = -movement.ReadValue<Vector2>().y;
        }
        else
        {
            verticalThrow = movement.ReadValue<Vector2>().y;
        }
        float newX = transform.localPosition.x + (horizontalThrow * moveSpeed * Time.deltaTime);
        float newY = transform.localPosition.y + (verticalThrow * moveSpeed * Time.deltaTime);
        newX = Mathf.Clamp(newX, -XRange, XRange);
        newY = Mathf.Clamp(newY, -YRange, YRange);

        transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }
    private void AdjustRotation(float horizontalThrow, float verticalThrow)
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControl = verticalThrow * controlPitchFactor;
        float yawDueToPosition = transform.localPosition.x * positionYawFactor;
        float rollDueToControl = horizontalThrow * controlRollFactor;

        float pitch = pitchDueToPosition + pitchDueToControl;
        float yaw = yawDueToPosition;
        float roll = rollDueToControl;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }
    private void ProcessFire()
    {
        if (fire.IsPressed() && !isFiring)
        {
            Debug.Log("Firing");
            isFiring = true;
            foreach (ParticleSystem laser in lasers)
            {
                laser.Play();
            }
        }
        else if (!fire.IsPressed() && isFiring)
        {
            isFiring = false;
        }
    }
}
