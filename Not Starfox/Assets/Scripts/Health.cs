using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int healthPoints;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player Laser"))
        {
            Debug.Log("Enemy Hit!");
        }
    }
}
