using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] ParticleSystem laser;
    private float counter;
    [SerializeField] float shotsPerSecond; // shots per second
    private float fireRate;
    [SerializeField] private float range;
    private float distanceToPlayer;

    // Start is called before the first frame update
    void Start()
    {
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
            transform.LookAt(player.transform);
            return true;
        }
        return false;
    }

    private void ProcessFire()
    {
        if (counter > fireRate)
        {
            laser.Play();
            counter = 0;
        }
    }
}
