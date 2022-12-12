using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTracker : MonoBehaviour
{
    private int score;
    [SerializeField] private TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString("D3");
    }

    public int GetScore()
    {
        return score;
    }
}
