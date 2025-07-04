using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public int scoreValue;
    private int maxScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        if(scoreValue > maxScore)
        {
            maxScore = scoreValue;
        }
        scoreText.text = maxScore.ToString();
    }
}
