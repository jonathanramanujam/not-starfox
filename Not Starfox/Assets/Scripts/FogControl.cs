using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogControl : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float end = 0f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collided with {other.gameObject}");
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ChangeFogEnd(speed, end));
        }
    }

    private IEnumerator ChangeFogEnd(float speed, float fogEnd)
    {
        float timeElapsed = 0;

        while (RenderSettings.fogEndDistance < fogEnd - 5)
        {
            RenderSettings.fogEndDistance = Mathf.Lerp(RenderSettings.fogEndDistance, fogEnd, timeElapsed * speed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        RenderSettings.fogEndDistance = fogEnd;
    }
}
