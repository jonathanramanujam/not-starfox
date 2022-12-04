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

    // [SerializeField] float positionPitchFactor = -10f;
    // [SerializeField] float controlPitchFactor = -30f;
    // [SerializeField] float positionYawFactor = 20f;
    [SerializeField] float controlRollFactor = -40f;

    [SerializeField] ParticleSystem[] lasers;
    [SerializeField] ParticleSystem afterburner;
    ParticleSystem.EmissionModule afterburnerEmission;

    void Start()
    {
        afterburnerEmission = afterburner.emission;
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
        if (movement.ReadValue<Vector2>().y > 0)
        {
            afterburnerEmission.rateOverTime = 250;
        }
        else if (movement.ReadValue<Vector2>().y < 0)
        {
            afterburnerEmission.rateOverTime = 10;
        }
        else
        {
            afterburnerEmission.rateOverTime = 100;
        }

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

        Debug.Log("roll");

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

    // private void OnParticleCollision(GameObject other)
    // {
    //     Debug.Log($"Collision: {other.name}");
    // }
}
