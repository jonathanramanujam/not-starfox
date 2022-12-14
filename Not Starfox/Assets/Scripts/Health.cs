using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] private int healthPoints;
    private int health;
    private GameObject explosion;
    [SerializeField] private Image energyMeter;
    private ScoreTracker scoreTracker;
    private GameObject gameOver;
    public bool isAlive = true;
    [SerializeField] private AudioSource SFXdamage;
    [SerializeField] private ParticleSystem damageSparks;

    private void Start()
    {
        health = healthPoints;
        UpdateHealthMeter();
        scoreTracker = FindObjectOfType<ScoreTracker>();
        explosion = transform.Find("Explosion").gameObject;
        if (CompareTag("Player"))
        {
            gameOver = GameObject.FindGameObjectWithTag("Game Over");
            gameOver.SetActive(false);
        }
    }

    public void ResetHealth()
    {
        isAlive = true;
        health = healthPoints;
        UpdateHealthMeter();
        GetComponentInChildren<MeshRenderer>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
        explosion.SetActive(false);
        if (TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.enabled = true;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("Lasers"))
        {
            if (!other.CompareTag(tag))
            {
                health--;
                UpdateHealthMeter();
                DeathCheck();
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.CompareTag("Environment") || other.collider.gameObject.CompareTag("Enemy"))
        {
            health -= 3;
            UpdateHealthMeter();
            SFXdamage.Play();
            DeathCheck();
        }
    }

    private void DeathCheck()
    {
        if (health <= 0 && isAlive)
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
                if (FindObjectOfType<PlayableDirector>().state == PlayState.Playing)
                {
                    gameOver.SetActive(true);
                    FindObjectOfType<PlayableDirector>().Pause();
                }
            }
            explosion.SetActive(true);
            isAlive = false;
        }
        else
        {
            damageSparks.Play();
            SFXdamage.Play();
        }
    }

    private void UpdateHealthMeter()
    {
        if (gameObject.tag == "Player")
        {
            energyMeter.fillAmount = (float)health / (float)healthPoints;
        }
    }
}
