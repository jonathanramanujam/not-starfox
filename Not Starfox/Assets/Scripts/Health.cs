using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] private int healthPoints;
    public int health;
    private GameObject explosion;
    [SerializeField] private Image energyMeter;
    private ScoreTracker scoreTracker;
    private GameObject gameOver;
    public bool isAlive = true;
    [SerializeField] private AudioSource SFXdamage;
    [SerializeField] private ParticleSystem damageSparks;
    [SerializeField] private MeshRenderer[] meshes;

    private void Start()
    {
        health = healthPoints;
        UpdateHealthMeter();
        scoreTracker = FindObjectOfType<ScoreTracker>();
        explosion = transform.Find("Explosion").gameObject;

        meshes = GetComponentsInChildren<MeshRenderer>();
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
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.enabled = true;
        }
        if (TryGetComponent<CapsuleCollider>(out CapsuleCollider capsuleCollider))
        {
            capsuleCollider.enabled = true;
        }
        else if (TryGetComponent<BoxCollider>(out BoxCollider boxCollider))
        {
            boxCollider.enabled = true;
        }
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
                foreach (MeshRenderer mesh in meshes)
                {
                    mesh.enabled = false;
                }
                if (TryGetComponent<CapsuleCollider>(out CapsuleCollider capsuleCollider))
                {
                    capsuleCollider.enabled = false;
                }
                if (TryGetComponent<BoxCollider>(out BoxCollider boxCollider))
                {
                    boxCollider.enabled = false;
                }
                if (TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.enabled = false;
                }
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
            Transform[] children = GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if (child.gameObject.layer == LayerMask.NameToLayer("Lasers"))
                {
                    child.GetComponent<ParticleSystem>().Stop();
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
