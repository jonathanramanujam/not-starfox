using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] ParticleSystem[] cannons;
    private float counter;
    [SerializeField] float shotsPerSecond; // shots per second
    private float fireRate;
    [SerializeField] private float range;
    private float distanceToPlayer;
    [SerializeField] private AudioSource SFXlaser;
    private Animator anim;
    private List<string> animations;

    [SerializeField] private Health core;
    [SerializeField] private Health leftCannon;
    [SerializeField] private Health topRightCannon;
    [SerializeField] private Health bottomRightCannon;
    [SerializeField] private GameObject shield;

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
        }
    }

    private void SetAnimations()
    {
        animations.Add("Firing Layer.Idle");
        animations.Add("Firing Layer.Left");
        animations.Add("Firing Layer.Right");
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
        if (leftCannon.health <= 0)
        {
            animations.Remove("Firing Layer.Left");
        }

        if (topRightCannon.health <= 0 && bottomRightCannon.health <= 0)
        {
            animations.Remove("Firing Layer.Right");
        }

        if (leftCannon.health <= 0 && topRightCannon.health <= 0 && bottomRightCannon.health <= 0)
        {
            Debug.Log("Removing Shield");
            shield.SetActive(false);
            // Add the new core animation
        }
    }

    private void FireCannon(int index)
    {

        cannons[index].Play();
    }
}
