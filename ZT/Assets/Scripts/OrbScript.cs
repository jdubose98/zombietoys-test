using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OrbScript : MonoBehaviour {

    [SerializeField] BoostTypes Type = BoostTypes.FireRate;
    [SerializeField] AudioSource PowerupSound;
    [SerializeField] PlayerShooting ShootScript;
    [SerializeField] Text DoubleDamageText;
    [SerializeField] Text FirerateText;
    [SerializeField] Text ShotgunText;
    [SerializeField] float Duration;

    // Max spawn pos Z is 22.7
    // Min pos Z is -22.7
    // Max X is 25, min X -25

    // Use this for initialization
    void Start () {
        Time.timeScale = 1;
	}
	
    void OnTriggerEnter(Collider otherObject)
    {
        PowerupSound.Play();
        switch(Type){
            case BoostTypes.DoubleDamage:
                StartCoroutine(Booster("DD",80, Duration, 40));
                break;
            case BoostTypes.FireRate:
                StartCoroutine(Booster("FR", .075, Duration, .125));
                break;
            case BoostTypes.Shotgun:
                StartCoroutine(Booster("SG", 1, Duration, 0));
                break;
        }

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponentInChildren<ParticleSystem>().Stop();
        gameObject.GetComponentInChildren<Canvas>().enabled = false;
        Destroy(gameObject, 6f);
    }

    enum BoostTypes
    {
        FireRate,
        Shotgun,
        DoubleDamage
    }

    IEnumerator Booster(string boostType, double startValue, float waitTime, double endValue) // Coroutines are cool.
    {
        if (boostType == "DD") { // double damage
            Debug.Log("Player got double damage!");
            ShootScript.damagePerShot = (int)startValue; // eww casting
            DoubleDamageText.color = new Color(255, 255, 255, 255);
            yield return new WaitForSeconds(waitTime);
            ShootScript.damagePerShot = (int)endValue;
            DoubleDamageText.color = new Color(255, 255, 255, .1f);
            Debug.Log("Player lost double damage!");
        }

        else if (boostType == "FR") { // fire rate
            Debug.Log("Player got firespeed boost!");
            ShootScript.timeBetweenBullets = (float)startValue; // ewww casting
            FirerateText.color = new Color(255, 255, 255, 255);
            yield return new WaitForSeconds(waitTime);
            FirerateText.color = new Color(255, 255, 255, .1f);
            ShootScript.timeBetweenBullets = (float)endValue;
            Debug.Log("Player lost firespeed boost!");
        }

        else if (boostType == "SG")
        { // fire rate
            Debug.Log("Player got shotgun!");
            ShootScript.Shotgun = true;
            FirerateText.color = new Color(255, 255, 255, 255);
            yield return new WaitForSeconds(waitTime);
            FirerateText.color = new Color(255, 255, 255, .1f);
            ShootScript.Shotgun = false;
            Debug.Log("Player lost shotgun!");
        }
    }
}
