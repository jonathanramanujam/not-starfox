using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int healthPoints;
    [SerializeField] private GameObject explosion;

    private void OnParticleCollision(GameObject other)
    {
        if (other.layer != gameObject.layer)
        {
            healthPoints--;
            DeathCheck();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"Collided: {other.collider.gameObject.tag}");
        if (other.collider.gameObject.CompareTag("Environment") || other.collider.gameObject.CompareTag("Enemy"))
        {
            healthPoints -= 3;
            DeathCheck();
        }
    }

    private void DeathCheck()
    {
        if (healthPoints <= 0)
            {
                if (!gameObject.CompareTag("Player"))
                {
                    GetComponent<MeshRenderer>().enabled = false;
                }
                explosion.SetActive(true);
            }
    }
}
