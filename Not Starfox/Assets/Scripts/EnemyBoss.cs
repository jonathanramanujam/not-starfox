using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] ParticleSystem[] cannons;
    [SerializeField] GameObject coreCannon;
    private float counter;
    [SerializeField] float shotsPerSecond; // shots per second
    private float fireRate;
    [SerializeField] private float range;
    private float distanceToPlayer;
    [SerializeField] private AudioSource SFXlaser;
    private Animator anim;
    private List<string> animations;

    [SerializeField] private GameObject[] headlights;

    [SerializeField] private Health core;
    [SerializeField] private Health leftCannon;
    [SerializeField] private Health topRightCannon;
    [SerializeField] private Health bottomRightCannon;
    [SerializeField] private GameObject shield;

    private int numDeadCannons = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        fireRate = 1 / shotsPerSecond;
        counter = fireRate;
        animations = new List<string>();
        SetAnimations();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInRange())
        {
            counter += Time.deltaTime;
            ProcessFire();
            CheckHealth();
            if (numDeadCannons == 3)
            {
                transform.LookAt(player.transform);
                coreCannon.transform.LookAt(player.transform);
            }
        }
    }

    private void SetAnimations()
    {
        animations.Add("Firing Layer.Idle");
        animations.Add("Firing Layer.Left");
        animations.Add("Firing Layer.Right Both");
        animations.Add("Firing Layer.Right Top");
        animations.Add("Firing Layer.Right Bottom");

    }

    private bool PlayerInRange()
    {
        distanceToPlayer = transform.position.magnitude - player.transform.position.magnitude;

        if (distanceToPlayer < range && distanceToPlayer > 0)
        {
            return true;
        }
        return false;
    }

    private void ProcessFire()
    {
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Firing Layer.Idle"))
        {
            if (anim != null)
            {
                int bossAction = Random.Range(0, animations.Count);

                anim.Play(animations[bossAction]);
            }
            // SFXlaser.Play();
            counter = 0;
        }
    }

    private void CheckHealth()
    {
        int dead = 0;
        if (leftCannon.health <= 0)
        {
            dead++;
            animations.Remove("Firing Layer.Left");
        }

        if (topRightCannon.health <= 0)
        {
            animations.Remove("Firing Layer.Right Top");
            animations.Remove("Firing Layer.Right Both");
            dead++;
        }

        if (bottomRightCannon.health <= 0)
        {
            animations.Remove("Firing Layer.Right Bottom");
            animations.Remove("Firing Layer.Right Both");
            dead++;
        }

        if (leftCannon.health <= 0 && topRightCannon.health <= 0 && bottomRightCannon.health <= 0)
        {
            if (shield.activeSelf)
            {
                animations.Remove("Firing Layer.Idle");
                animations.Add("Firing Layer.Core");
            }
            shield.SetActive(false);
            coreCannon.GetComponent<ParticleSystem>().Play();
        }

        if (dead > numDeadCannons)
        {
            numDeadCannons = dead;
        }

        UpdateHeadlights(numDeadCannons);
    }

    private void UpdateHeadlights(int numDeadCannons)
    {
        if (numDeadCannons == 1)
        {
            headlights[0].SetActive(false);
            //change emission to yellow
            Color yellow = new Vector4(1.5f, 1.3f, 0f, 1f);
            foreach (GameObject headlight in headlights)
            {
                headlight.GetComponent<Renderer>().material.SetColor("_EmissionColor", yellow);
            }
        }

        else if (numDeadCannons == 2)
        {
            headlights[0].SetActive(false);
            headlights[1].SetActive(false);
            //change emission to orange
            Color orange = new Vector4(1.8f, .5f, 0f, 1.3f);
            foreach (GameObject headlight in headlights)
            {
                headlight.GetComponent<Renderer>().material.SetColor("_EmissionColor", orange);
            }
        }

        else if (numDeadCannons == 3)
        {
            headlights[0].SetActive(false);
            headlights[1].SetActive(false);
            headlights[2].SetActive(false);
            //change emission to red
            Color red = new Vector4(1.8f, 0f, 0f, 1.3f);
            foreach (GameObject headlight in headlights)
            {
                headlight.GetComponent<Renderer>().material.SetColor("_EmissionColor", red);
            }
        }
    }

    private void FireCannon(int index)
    {
        cannons[index].Play();
    }
}
