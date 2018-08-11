using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EartMovement : MonoBehaviour {

    public Transform target;
    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 position = gameObject.transform.position;
        float step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(position, target.position, step * 0.2f);

        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(target.position, Vector3.up, -speed * 200 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(target.position, Vector3.up, speed * 200 * Time.deltaTime);
        }
	}
}
