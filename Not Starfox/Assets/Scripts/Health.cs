using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int healthPoints;
    private int maxHealth;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Image energyMeter;

    private void Start()
    {
        maxHealth = healthPoints;
        UpdateHealthMeter();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("Lasers"))
        {
            if (!other.CompareTag(tag))
            {
                Debug.Log($"Hit: {LayerMask.LayerToName(gameObject.layer)}");
                healthPoints--;
                UpdateHealthMeter();
                DeathCheck();
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"Collided: {other.collider.gameObject.tag}");
        if (other.collider.gameObject.CompareTag("Environment") || other.collider.gameObject.CompareTag("Enemy"))
        {
            healthPoints -= 3;
            UpdateHealthMeter();
            DeathCheck();
        }
    }

    private void DeathCheck()
    {
        if (healthPoints <= 0)
            {
                if (gameObject.CompareTag("Enemy"))
                {
                    GetComponent<MeshRenderer>().enabled = false;
                    GetComponent<BoxCollider>().enabled = false;
                    GetComponent<Enemy>().enabled = false;
                }
                explosion.SetActive(true);

            }
    }

    private void UpdateHealthMeter()
    {
        if (gameObject.tag == "Player")
        {
            energyMeter.fillAmount = (float)healthPoints / maxHealth;
        }
    }
}
