using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EartMovement : MonoBehaviour {

    public Transform target;
    public float speed;
    private bool jumping, ghost;
    public bool alive;
    private int multikill;
    public Button againButton, exitButton, highscoreButton;
    public GameObject ghostParticles;

    float GhostDuration;

	// Use this for initialization
	void Start ()
    {
        jumping = false;
        alive = true;
        multikill = 1;
        GhostDuration = 5;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (alive)
        {
            Vector3 position = gameObject.transform.position;
            float step = speed * Time.deltaTime;
            if (ghost)
            {
                GhostDuration -= Time.deltaTime;
                if (GhostDuration < 0)
                {
                    ghost = false;
                    GhostDuration = 3;
                    Destroy(transform.Find("Ghost(Clone)").gameObject);
                }
            }
            
            if (jumping)
            {
                transform.position = Vector3.MoveTowards(position, target.position, -step * 2f);

                if (Vector3.Distance(Vector3.zero, transform.position) >= Vector3.Distance(Vector3.zero, GameObject.Find("BlackHole").GetComponent<GameHandler>().powerups[0].transform.position))
                {
                    jumping = false;
                    multikill = 1;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(position, target.position, step * 0.2f);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.RotateAround(target.position, Vector3.up, -speed * (Input.GetKey(KeyCode.LeftShift) ? 50 : 100) * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.RotateAround(target.position, Vector3.up, speed * (Input.GetKey(KeyCode.LeftShift) ? 50 : 100) * Time.deltaTime);
            }
            if (gameObject.name != "Sun")
            {
                transform.Rotate(new Vector3(1, 1, 0), step * 100f);
            }
        }
        else
        {

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
            GameObject.Find("BlackHole").GetComponent<GameHandler>().PlayPickup();
        }

        else if (collision.gameObject.name.Contains("Ghost"))
        {
            if (ghost)
            {
                Destroy(transform.Find("Ghost(Clone)").gameObject);
            }
            ghost = true;
            Destroy(collision.gameObject);
            GameObject.Find("BlackHole").GetComponent<GameHandler>().Score += 10;
            GameObject shield = Instantiate(ghostParticles) as GameObject;
            shield.transform.position = transform.position;
            shield.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            shield.transform.SetParent(transform);
            GameObject.Find("BlackHole").GetComponent<GameHandler>().PlayPickup();
        }

        else if (collision.gameObject.name.Contains("Rock"))
        {
            if(!jumping && !ghost)
            {
                Destroy(gameObject);
                alive = false;
                againButton.gameObject.SetActive(true);
                exitButton.gameObject.SetActive(true);
                highscoreButton.gameObject.SetActive(true);
            }
            else
            {
                Destroy(collision.gameObject);
                multikill++;
                GameObject.Find("BlackHole").GetComponent<GameHandler>().Score += 10 * (float)Math.Pow(1.5f, multikill);
            }
            GameObject.Find("BlackHole").GetComponent<GameHandler>().PlayExplosion();
        }
    }
}
