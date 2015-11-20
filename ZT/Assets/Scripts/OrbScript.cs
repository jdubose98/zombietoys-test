using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

public class OrbScript : MonoBehaviour {
    // Unity accepts these as being able to set in the prefab...
    [SerializeField] BoostTypes Type = BoostTypes.FireRate;
    [SerializeField] AudioMixer Mixer;
    [SerializeField] float Duration;
    // but the rest of these friggin things can't be set! Why!?
    AudioSource PowerupSound;

    PlayerShooting ShootScript;
    Text DoubleDamageText;
    Text FirerateText;
    Text ShotgunText;
    Text SpeedText;

    PlayerMovement MoveScript;
    

    // Max spawn pos Z is 22.7
    // Min pos Z is -22.7
    // Max X is 25, min X -25

    // Use this for initialization
    void Start () {
        PowerupSound = GameObject.FindWithTag("Gun Tip").GetComponent<AudioSource>();

        MoveScript = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        ShootScript = GameObject.FindWithTag("Player").GetComponentInChildren<PlayerShooting>();

        DoubleDamageText = GameObject.FindWithTag("Damage").GetComponent<Text>();
        FirerateText = GameObject.FindWithTag("Firerate").GetComponent<Text>();
        ShotgunText = GameObject.FindWithTag("Shotgun").GetComponent<Text>();
        SpeedText = GameObject.FindWithTag("Speed").GetComponent<Text>();
    }
	
    void OnTriggerEnter(Collider otherObject)
    {
        if (otherObject.tag == "Player")
        {
            PowerupSound.Play();
            switch (Type)
            {
                case BoostTypes.DoubleDamage:
                    StartCoroutine(Booster("DD", 80, Duration, 40));
                    break;
                case BoostTypes.FireRate:
                    StartCoroutine(Booster("FR", .075, Duration, .125));
                    break;
                case BoostTypes.Shotgun:
                    StartCoroutine(Booster("SG", 1, Duration, 0));
                    break;
                case BoostTypes.Speed:
                    StartCoroutine(Booster("SB", 16, Duration, 6));
                    break;
            }

            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            gameObject.GetComponentInChildren<ParticleSystem>().Stop();
            gameObject.GetComponentInChildren<Canvas>().enabled = false;
            Destroy(gameObject, Duration + 1);
        }
    }

    enum BoostTypes
    {
        FireRate,
        Shotgun,
        DoubleDamage,
        Speed
    }

    IEnumerator Booster(string boostType, double startValue, float waitTime, double endValue) // Coroutines are cool.
    {
        if (boostType == "DD") { // double damage
            ShootScript.damagePerShot = (int)startValue; 
            DoubleDamageText.color = new Color(1, 1, 1, 1);
            Mixer.SetFloat("GunPitch", .875f);
            yield return new WaitForSeconds(waitTime);
            ShootScript.damagePerShot = (int)endValue;
            Mixer.SetFloat("GunPitch", 1.16f);
            DoubleDamageText.color = new Color(1, 1, 1, .1f);
        }

        else if (boostType == "FR") { // fire rate
            ShootScript.timeBetweenBullets = (float)startValue;
            FirerateText.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(waitTime);
            FirerateText.color = new Color(1, 1, 1, .1f);
            ShootScript.timeBetweenBullets = (float)endValue;
        }

        else if (boostType == "SG")
        { // shotgun
            ShootScript.Shotgun = true;
            ShotgunText.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(waitTime);
            ShotgunText.color = new Color(1, 1, 1, .1f);
            ShootScript.Shotgun = false;
        }
        else if (boostType == "SB")
        { // run fast!
            MoveScript.speed = (float)startValue;
            SpeedText.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(waitTime);
            SpeedText.color = new Color(1, 1, 1, .1f);
            MoveScript.speed = (float)endValue;
        }
    }
}
