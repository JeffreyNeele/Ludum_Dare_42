using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCaseRotation : MonoBehaviour {

    public float speed;
    public Vector3 rotationAxis;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float step = speed * Time.deltaTime;
        transform.Rotate(rotationAxis, step * 100f);
    }
}
