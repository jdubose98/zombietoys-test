using UnityEngine;
using System.Collections;

public class OrbRandomizer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        RandomizePositions();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void RandomizePositions()
    {
        GameObject[] orbs = GameObject.FindGameObjectsWithTag("Boosters");
        foreach (GameObject target in orbs)
        {
            Debug.Log(target.name);
            target.transform.position = new Vector3(Random.Range(-120, 120), 1.2f, Random.Range(-120, 120));
        }
    }

    IEnumerator UpdateOrbs()
    {
        yield return new WaitForSeconds(60);
        RandomizePositions();
    }
}
