using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour {

    int index = 0;
	// Use this for initialization
	void Start () {

        if (!PlayerPrefs.HasKey("Player"))
            PlayerPrefs.SetInt("Player", index);
        else
            index = PlayerPrefs.GetInt("Player");
        PlayerPrefs.SetInt("OwnEarth", 1);
        PlayerPrefs.SetInt("OwnSun", 1);
        PlayerPrefs.SetInt("OwnMars", 1);

        transform.GetChild(index).gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
