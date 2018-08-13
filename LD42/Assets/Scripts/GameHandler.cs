using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour {

    public GameObject spacerock;
    public GameObject powerup;
    public GameObject furthest;
    public GameObject PlayerObject;
    public GameObject[] Rocks;
    public GameObject PlanetRock;
    public Material[] PlanetMaterials;
    public GameObject bigmomma;
    public GameObject Canvas;

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
            if (Random.value < 0.02)
            {
                System.Random rnd = new System.Random();
                GameObject planet = Instantiate(PlanetRock) as GameObject;
                planet.AddComponent<SphereCollider>();
                planet.AddComponent<SpaceRock>();
                Renderer rend = planet.GetComponent<Renderer>();
                rend.material = PlanetMaterials[rnd.Next(0, PlanetMaterials.Length - 1)];
                float randomScale = Random.Range(0.2f, 1.2f);
                planet.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            }
            else
            {
                System.Random rnd = new System.Random();
                GameObject rock = Instantiate(Rocks[rnd.Next(0, 3)]) as GameObject;
                rock.AddComponent<MeshCollider>();
                rock.AddComponent<SpaceRock>();
                float randomScale = Random.Range(0.5f, 0.75f);
                rock.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            }
        }
        if (PlayerObject.GetComponent<StartUp_Game>().Player)
        {
            Score += Time.deltaTime;
        }
        Canvas.GetComponentInChildren<Text>().text = "Score: " + Score.ToString("0.00");
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject, 0.2f);
    }
}
