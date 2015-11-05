using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OrbScript : MonoBehaviour {

    [SerializeField] BoostTypes Type = BoostTypes.FireRate;
    [SerializeField] Text InfoText;
    [SerializeField] AudioSource PowerupSound;
    [SerializeField] PlayerShooting ShootScript;

    // Max spawn pos Z is 22.7
    // Min pos Z is -22.7
    // Max X is 25, min X -25

	// Use this for initialization
	void Start () {
	
	}
	
    void OnTriggerEnter(Collider otherObject)
    {
        PowerupSound.Play();
        switch(Type){
            case BoostTypes.DoubleDamage:
                BulletDamageBoost(80, 5, 40);
                break;
            case BoostTypes.FireRate:
                break;
            case BoostTypes.Shotgun:
                break;
        }
        
        DestroyObject(gameObject);
    }

    enum BoostTypes
    {
        FireRate,
        Shotgun,
        DoubleDamage
    }

    IEnumerator BulletDamageBoost(int startValue, float waitTime, int endValue) // Coroutines are cool.
    {
        Debug.Log("Player got double damage!");
        ShootScript.damagePerShot = startValue;
        yield return new WaitForSeconds(waitTime);
        ShootScript.damagePerShot = endValue;
        Debug.Log("Player lost double damage!");
    }
}
