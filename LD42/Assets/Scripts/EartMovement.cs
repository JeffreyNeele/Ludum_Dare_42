using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EartMovement : MonoBehaviour {

    public Transform target;
    public float speed;
    private bool jumping;
    private int multikill;

	// Use this for initialization
	void Start ()
    {
        jumping = false;
        multikill = 1;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (jumping)
        {
            Vector3 position = gameObject.transform.position;
            float step = speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(position, target.position, -step * 2f);

            if (Vector3.Distance(Vector3.zero, transform.position) >= Vector3.Distance(Vector3.zero, GameObject.Find("BlackHole").GetComponent<GameHandler>().powerups[0].transform.position))
            {
                jumping = false;
                multikill = 1;
            }
        }
        else
        {
            Vector3 position = gameObject.transform.position;
            float step = speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(position, target.position, step * 0.2f);
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(target.position, Vector3.up, -speed * 100 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(target.position, Vector3.up, speed * 100 * Time.deltaTime);
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "PowerUp(Clone)")
        {
            GameObject.Find("BlackHole").GetComponent<GameHandler>().powerups.Remove(collision.gameObject);
            Destroy(collision.gameObject);
            GameObject.Find("BlackHole").GetComponent<GameHandler>().SpawnPowerup();
            jumping = true;
        }
        
        else if(collision.gameObject.name == "SpaceRock(Clone)")
        {
            if(!jumping)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(collision.gameObject);
                multikill++;
                GameObject.Find("BlackHole").GetComponent<GameHandler>().Score += 10 * (float)Math.Pow(1.5f, multikill);
            }
        }
    }
}
