using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScores : MonoBehaviour {

    float[] highScores = new float[10];
    public Text[] TextList = new Text[10];

	// Use this for initialization
	void Start () {

		for(int i = 0; i < highScores.Length; i++)
        {
            if (PlayerPrefs.HasKey("HighScore" + (i + 1)))
                highScores[i] = PlayerPrefs.GetFloat("HighScore" + (i + 1));
            else
                break;

            //highScores[i] = 200 - i * 10;
        }

        InitializeHighScoreList();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitializeHighScoreList()
    {
        for(int i = 0; i < TextList.Length; i++)
        {
            if (highScores[i] != 0)
                TextList[i].text = highScores[i].ToString();
            else
                TextList[i].text = "-";
        }
    }

    public void StoreHighScore(Text scoreText)
    {
        float score = float.Parse(scoreText.text.Split(' ')[1]);
        for(int i = 0; i < highScores.Length; i++)
        {
            if (PlayerPrefs.HasKey("HighScore" + (i + 1)))
                highScores[i] = PlayerPrefs.GetFloat("HighScore" + (i + 1));
            else
                break;
        }

        for (int i = 0; i < highScores.Length; i++)
            if (highScores[i] > score)
                continue;
            else
            {
                for(int j = i; j < highScores.Length; j++)
                {
                    float oldScore = highScores[j];
                    highScores[j] = score;
                    score = oldScore;
                }

                for (int j = 0; j < highScores.Length; j++)
                    PlayerPrefs.SetFloat("HighScore" + (j + 1), highScores[j]);

                break;
            }
    }
}
