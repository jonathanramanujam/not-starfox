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

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        fireRate = 1 / shotsPerSecond;
        counter = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInRange())
        {
            counter += Time.deltaTime;
            ProcessFire();
        }
    }

    private bool PlayerInRange()
    {
        distanceToPlayer = transform.position.magnitude - player.transform.position.magnitude;

        if (distanceToPlayer < range && distanceToPlayer > 0)
        {
            // transform.LookAt(player.transform);
            return true;
        }
        return false;
    }

    private void ProcessFire()
    {
        // if cannons are gone
        // Choose a random number
        // Play a random fire animation

        // else, fire the main cannon

        // if (counter > fireRate)
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Firing Layer.Idle"))
        {
            if (anim != null)
            {
                int bossAction = Random.Range(0, 3);

                switch (bossAction)
                {
                    case 0:
                        anim.Play("Firing Layer.Idle");
                        break;
                    case 1:
                        anim.Play("Firing Layer.Left");
                        break;
                    case 2:
                        anim.Play("Firing Layer.Right");
                        break;
                }

            }
            // SFXlaser.Play();
            counter = 0;
        }
    }

    private void FireCannon(int index)
    {

        cannons[index].Play();
    }
}
