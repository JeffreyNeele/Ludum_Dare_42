using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceRock : MonoBehaviour {

    float speed;

	// Use this for initialization
	void Start ()
    {
        speed = Random.Range(1f, 10f);
        transform.position = new Vector3(10f, 0, 0);
        transform.RotateAround(Vector3.zero, Vector3.up, Random.Range(0f, 360f));
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position == Vector3.zero)
        {
            Destroy(this.gameObject);
        }

        Vector3 position = gameObject.transform.position;
        float step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(position, Vector3.zero, step * 0.2f);
        transform.RotateAround(Vector3.zero, Vector3.up, 0.1f);
    }
}
