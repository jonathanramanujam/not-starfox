using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int healthPoints;

    private void OnParticleCollision(GameObject other)
    {
        if (other.layer != gameObject.layer)
        {
            healthPoints--;
            if (healthPoints <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
