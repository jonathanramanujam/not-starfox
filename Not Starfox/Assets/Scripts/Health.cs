using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] private int healthPoints;
    private int maxHealth;
    private GameObject explosion;
    [SerializeField] private Image energyMeter;
    private ScoreTracker scoreTracker;
    private GameObject gameOver;
    public bool isAlive = true;

    private void Start()
    {
        maxHealth = healthPoints;
        UpdateHealthMeter();
        scoreTracker = FindObjectOfType<ScoreTracker>();
        explosion = transform.Find("Explosion").gameObject;
        if (CompareTag("Player"))
        {
            gameOver = GameObject.FindGameObjectWithTag("Game Over");
            gameOver.SetActive(false);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("Lasers"))
        {
            if (!other.CompareTag(tag))
            {
                healthPoints--;
                UpdateHealthMeter();
                DeathCheck();
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.CompareTag("Environment") || other.collider.gameObject.CompareTag("Enemy"))
        {
            healthPoints -= 3;
            UpdateHealthMeter();
            DeathCheck();
        }
    }

    private void DeathCheck()
    {
        if (healthPoints <= 0 && isAlive)
            {
                if (gameObject.CompareTag("Enemy"))
                {
                    GetComponentInChildren<MeshRenderer>().enabled = false;
                    GetComponent<CapsuleCollider>().enabled = false;
                    GetComponent<Enemy>().enabled = false;
                    scoreTracker.AddScore();
                }
                else
                {
                    gameOver.SetActive(true);
                    FindObjectOfType<PlayableDirector>().Pause();
                }
                explosion.SetActive(true);
                isAlive = false;
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
