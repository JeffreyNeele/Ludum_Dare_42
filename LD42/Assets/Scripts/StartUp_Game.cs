using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp_Game : MonoBehaviour {

    int index;
    GameObject playerObject;
	// Use this for initialization
	void Start () {
        index = PlayerPrefs.GetInt("Player");

        playerObject = transform.GetChild(index).gameObject;
        playerObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject Player
    {
        get { return playerObject; }
    }
}
