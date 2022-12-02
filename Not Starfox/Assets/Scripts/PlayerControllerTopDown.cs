using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerTopDown : MonoBehaviour
{
    [SerializeField] InputAction movement;
    [SerializeField] InputAction fire;
    bool isFiring = false;
    [SerializeField] bool invertY = false;
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float XRange = 7f;
    [SerializeField] float minZRange= -1f;
    [SerializeField] float maxZRange= 15f;

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
        float newZ = transform.localPosition.z + (verticalThrow * moveSpeed * Time.deltaTime);
        newX = Mathf.Clamp(newX, -XRange, XRange);
        newZ = Mathf.Clamp(newZ, minZRange, maxZRange);

        transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, newZ);
    }
    private void AdjustRotation(float horizontalThrow, float verticalThrow)
    {
        float rollDueToControl = horizontalThrow * controlRollFactor;
        float roll = rollDueToControl;

        transform.localRotation = Quaternion.Euler(0.0f, 0.0f, roll);
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
