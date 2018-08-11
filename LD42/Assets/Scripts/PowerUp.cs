using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start ()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, Random.Range(0f, 360f));
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 position = gameObject.transform.position;
        float step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(position, Vector3.zero, step * 0.2f);
    }
}
