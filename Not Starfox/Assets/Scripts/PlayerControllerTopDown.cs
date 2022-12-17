using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControllerTopDown : MonoBehaviour
{
    [SerializeField] InputAction movement;
    [SerializeField] InputAction menuSelect;
    [SerializeField] InputAction fire;
    [SerializeField] InputAction proceed;
    [SerializeField] InputAction pause;
    bool isFiring = false;
    [SerializeField] bool invertY = false;
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float XRange = 7f;
    [SerializeField] float minZRange= -1f;
    [SerializeField] float maxZRange= 15f;
    [SerializeField] float controlRollFactor = -40f;

    [SerializeField] ParticleSystem[] lasers;
    [SerializeField] ParticleSystem afterburner;
    ParticleSystem.EmissionModule afterburnerEmission;
    [SerializeField] private Health playerHealth;

    [SerializeField] AudioSource SFXshipHum;
    [SerializeField] AudioSource SFXlaser;
    [SerializeField] AudioSource SFXboost;
    [SerializeField] AudioSource SFXbrake;

    [SerializeField] GameObject pauseMenu;

    private bool isBoosting = false;
    private bool isBraking = false;
    private bool isPlaying = false;

    void Start()
    {
        afterburnerEmission = afterburner.emission;
    }

    private void OnEnable()
    {
        movement.Enable();
        menuSelect.Enable();
        fire.Enable();
        proceed.Enable();
        pause.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        menuSelect.Disable();
        fire.Disable();
        proceed.Disable();
        pause.Disable();
    }

    void FixedUpdate()
    {
        float horizontalThrow, verticalThrow;
        AdjustPosition(out horizontalThrow, out verticalThrow);
        AdjustRotation(horizontalThrow, verticalThrow);
        ProcessFire();
    }

    private void Update()
    {
        ProcessPause();
    }

    private void AdjustPosition(out float horizontalThrow, out float verticalThrow)
    {
        if (movement.ReadValue<Vector2>().y > .5f)
        {
            afterburnerEmission.rateOverTime = 250;
            if (!isBoosting)
            {
                SFXboost.Play();
                SFXshipHum.pitch = 1.3f;
            }
            isBoosting = true;
            isBraking = false;
        }
        else if (movement.ReadValue<Vector2>().y < -.5f)
        {
            afterburnerEmission.rateOverTime = 10;
            if (!isBraking)
            {
                SFXbrake.Play();
                SFXshipHum.pitch = .7f;
            }
            isBraking = true;
            isBoosting = false;
        }
        else
        {
            afterburnerEmission.rateOverTime = 100;
            if (isBraking || isBoosting)
            {
                SFXshipHum.pitch = 1f;
            }
            isBraking = false;
            isBoosting = false;
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

        transform.localRotation = Quaternion.Euler(0.0f, 0.0f, roll);
    }
    private void ProcessFire()
    {
        if (fire.IsPressed() && !isFiring)
        {
            isFiring = true;
            foreach (ParticleSystem laser in lasers)
            {
                laser.Play();
            }
            SFXlaser.Play();
        }
        else if (!fire.IsPressed() && isFiring)
        {
            isFiring = false;
        }
    }

    private void ProcessPause()
    {
        if (pause.WasPressedThisFrame() && isPlaying)
        {
            if (!pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
                SelectFirstButton(pauseMenu);
            }
            else
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }

    public void SetGameState(bool isPlaying)
    {
        this.isPlaying = isPlaying;
    }

    public void SelectFirstButton(GameObject menu)
    {
        Button[] buttons = menu.GetComponentsInChildren<Button>();
        buttons[0].Select();
    }

    public void Unpause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ProcessRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
