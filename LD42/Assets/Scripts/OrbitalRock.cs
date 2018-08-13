using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalRock : MonoBehaviour {

    float speed;

	// Use this for initialization
	void Start ()
    {
        
	}

    public void Setup()
    {
        speed = Random.Range(0.1f, 0.1f);
        transform.localPosition = new Vector3(Random.Range(0.5f, 0.9f), 0, 0);
        transform.RotateAround(transform.parent.position, Vector3.up, Random.Range(0f, 360f));
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.RotateAround(transform.parent.position, Vector3.up, 8f);
        transform.Rotate(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)).normalized, speed * Time.deltaTime * 2f);
    }
}
