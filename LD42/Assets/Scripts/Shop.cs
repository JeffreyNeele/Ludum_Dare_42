using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

    public List<Button> ButtonList;
    public Button ExitButton;
    int index;
    int[] options = new int[3];

	// Use this for initialization
	void Start () {
        index = PlayerPrefs.GetInt("Player");
        options[0] = PlayerPrefs.GetInt("OwnEarth");
        options[1] = PlayerPrefs.GetInt("OwnSun");
        options[2] = PlayerPrefs.GetInt("OwnMars");

        InitializeBoxes();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitializeBoxes()
    {
        for(int i = 0; i < options.Length; i++)
        {
            if (options[i] == 1)
                ButtonList[i].GetComponentInChildren<Text>().text = "Choose"; 
        }

        ButtonList[index].GetComponentInChildren<Text>().text = "Chosen";
    }

    public void LoadInPrefs()
    {
        PlayerPrefs.SetInt("Player", index);
        ExitButton.GetComponent<Buttons>().ChangeScene();
    }

    public void ChangePrefs(int buttonIndex)
    {
        ButtonList[index].GetComponentInChildren<Text>().text = "Choose";
        ButtonList[buttonIndex].GetComponentInChildren<Text>().text = "Chosen";
        index = buttonIndex;
    }
}
