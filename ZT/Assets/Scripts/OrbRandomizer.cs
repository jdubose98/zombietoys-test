using UnityEngine;
using System.Collections;

public class OrbRandomizer : MonoBehaviour {

    [SerializeField] GameObject SpeedBoosterPrefab;
    [SerializeField] GameObject ShotgunPrefab;
    [SerializeField] GameObject DoubleDamagePrefab;
    [SerializeField] GameObject FireBoosterPrefab;


    // Use this for initialization
    void Start () {
        RandomizePositions();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void RandomizePositions()
    {
        if (!GameObject.Find("SpeedBoostOrb")) { 
            Instantiate(SpeedBoosterPrefab, new Vector3(0, 200, 0), SpeedBoosterPrefab.transform.rotation);
            Debug.Log("New orb generated"); }
        if (!GameObject.Find("ShotgunOrb")) { 
            Instantiate(ShotgunPrefab, new Vector3(0, 200, 0), ShotgunPrefab.transform.rotation);
            Debug.Log("New orb generated");
        }
        if (!GameObject.Find("DamageBoosterOrb")) { 
            Instantiate(DoubleDamagePrefab, new Vector3(0, 200, 0), DoubleDamagePrefab.transform.rotation);
            Debug.Log("New orb generated");
        }
        if (!GameObject.Find("FireSpeedBoosterOrb")){
            Instantiate(FireBoosterPrefab, new Vector3(0, 200, 0), FireBoosterPrefab.transform.rotation);
            Debug.Log("New orb generated");
        }

GameObject[] orbs = GameObject.FindGameObjectsWithTag("Boosters");

        foreach (GameObject target in orbs)
        {
            Debug.Log(target.name);
            target.transform.position = new Vector3(Random.Range(-12.2f, 12.2f), 1.2f, Random.Range(-11.7f,11.7f));
        }
    }

    IEnumerator UpdateOrbs()
    {
        yield return new WaitForSeconds(60);
        RandomizePositions();
    }
}
