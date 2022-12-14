using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class GameManager : MonoBehaviour
{
    private PlayableDirector timeline;
    [SerializeField] private AudioSource music;
    [SerializeField] private GameObject scoreDisplay;
    [SerializeField] private GameObject energyDisplay;

    private void StartGame()
    {
        timeline.Play();
        music.Play();
        scoreDisplay.SetActive(true);
        energyDisplay.SetActive(true);

    }
}
