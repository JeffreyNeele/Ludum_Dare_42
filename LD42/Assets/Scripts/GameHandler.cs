using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour {

    public GameObject spacerock;
    public GameObject powerup;
    public GameObject furthest;

    public float Score;

    public List<GameObject> powerups;

	// Use this for initialization
	void Start ()
    {
        powerups = new List<GameObject>
        {
            Instantiate(powerup, new Vector3(0, 0, 5), Quaternion.identity),
            Instantiate(powerup, new Vector3(0, 0, 6), Quaternion.identity),
            Instantiate(powerup, new Vector3(0, 0, 7), Quaternion.identity),
            Instantiate(powerup, new Vector3(0, 0, 8), Quaternion.identity),
            (furthest = Instantiate(powerup, new Vector3(0, 0, 9), Quaternion.identity))
        };
    }

    public void SpawnPowerup()
    {
        powerups.Add(furthest = Instantiate(powerup, new Vector3(0, 0, Vector3.Distance(Vector3.zero, furthest.transform.position) + 1f), Quaternion.identity));
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Random.value < 0.05)
        {
            Instantiate(spacerock);
        }
        if (GameObject.Find("EarthChan"))
        {
            Score += Time.deltaTime;
        }
        FindObjectOfType<Text>().text = "Score: " + Score.ToString("0.00");
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject, 0.2f);
    }
}
