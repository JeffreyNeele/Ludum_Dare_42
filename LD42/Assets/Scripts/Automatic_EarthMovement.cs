using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automatic_EarthMovement : MonoBehaviour {

    public Transform target;
    public float speed;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(target.position, Vector3.up, -speed * 100 * Time.deltaTime);
    }
}
