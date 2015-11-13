using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;


public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot;                  // The damage inflicted by each bullet.
    public float timeBetweenBullets;        // The time between each shot.
    [SerializeField] float range = 100f;            // The distance the gun can fire.


    float timer;                                    // A timer to determine when to fire.
    Ray shootRay;                                   // A ray from the gun end forwards.
    RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    Ray shootRay2;                                  // shotgun pellet 1
    RaycastHit shootHit2;                            // A raycast hit to get information about what was hit.
    Ray shootRay3;                                  // shotgun pellet 2
    RaycastHit shootHit3;                            // A raycast hit to get information about what was hit.
    Ray shootRay4;                                  // shotgun pellet 3
    RaycastHit shootHit4;                            // A raycast hit to get information about what was hit.

    int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
    ParticleSystem gunParticles;                    // Reference to the particle system.
    LineRenderer gunLine;                           // Reference to the line renderer.
    [SerializeField] LineRenderer gunLine1;                           // Reference to the line renderer.
    [SerializeField] LineRenderer gunLine2;                           // Reference to the line renderer.
    [SerializeField] LineRenderer gunLine3;                           // Reference to the line renderer.
    AudioSource gunAudio;                           // Reference to the audio source.
    Light gunLight;                                 // Reference to the light component.
    [SerializeField] Light faceLight;               // Duh
    float effectsDisplayTime = 0.2f;                // The fraction of time between shots that the effects will display for.
    public bool Shotgun;                            // tells us if we're in shotgun mode.


    void Awake()
    {
        // Create a layer mask for the Shootable layer.
        shootableMask = LayerMask.GetMask("Shootable");

        // Set up the references.
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        //faceLight = GetComponentInChildren<Light> ();
    }


    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

#if !MOBILE_INPUT
        // If the Fire1 button is being press and it's time to fire...
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            // ... shoot the gun.
            Shoot();
        }
#else
            // If there is input on the shoot direction stick and it's time to fire...
            if ((CrossPlatformInputManager.GetAxisRaw("Mouse X") != 0 || CrossPlatformInputManager.GetAxisRaw("Mouse Y") != 0) && timer >= timeBetweenBullets)
            {
                // ... shoot the gun
                Shoot();
            }
#endif
        // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            // ... disable the effects.
            DisableEffects();
        }
    }


    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
        gunLine1.enabled = false;
        gunLine2.enabled = false;
        gunLine3.enabled = false;
        faceLight.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot()
    {
        //Reset timer
        timer = 0f;

        //pew!
        gunAudio.Play();

        //Lights
        gunLight.enabled = true;
        faceLight.enabled = true;

        // Reset the muzzle flash particles
        gunParticles.Stop();
        gunParticles.Play();

        // Turn on the renderer and set its first position to the muzzle
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        // Copypaste of the earlier for the shotgun mode
        if (Shotgun == true) {
        gunLine1.enabled = true;
        gunLine1.SetPosition(0, transform.position);
        gunLine2.enabled = true;
        gunLine2.SetPosition(0, transform.position);
        gunLine3.enabled = true;
        gunLine3.SetPosition(0, transform.position); }

        // Sets the ray to aim forwards from the barrel
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Shotgun == true)
        {
            shootRay2.origin = transform.position;
            shootRay2.direction = transform.forward + new Vector3(Random.Range(-.2f, .2f), 0, Random.Range(-.6f, .6f)); // Randomness is cool

            shootRay3.origin = transform.position;
            shootRay3.direction = transform.forward + new Vector3(Random.Range(-.2f, .2f), 0, Random.Range(-.6f, .6f));

            shootRay4.origin = transform.position;
            shootRay4.direction = transform.forward + new Vector3(Random.Range(-.2f, .2f), 0, Random.Range(-.6f, .6f));
        }

        
        RayShoot(shootRay, shootHit, range, shootableMask, gunLine); // Raycasts to see if we hit something

        if (Shotgun == true)
        {
            //SG Ray 1
            RayShoot(shootRay2, shootHit2, range, shootableMask, gunLine1);

            //SG Ray 2
            RayShoot(shootRay3, shootHit3, range, shootableMask, gunLine2);

            //SG Ray 3
            RayShoot(shootRay4, shootHit4, range, shootableMask, gunLine3);

            
        }
    }

    void RayShoot(Ray shotRay, RaycastHit rayHitPoint, float range, int shootMask, LineRenderer line ) // Greatly reduced copypasta. Takes the ray that's being shot out,
    {                                                                                                  // where it hits, its range, what it can hit, and the renderer itself.
        if (Physics.Raycast(shotRay, out rayHitPoint, range, shootMask)) // Looks to see if we shot something that is on the layer (enemies)
        {
            EnemyHealth enemyHealth = rayHitPoint.collider.GetComponent<EnemyHealth>(); // Finds enemy health component

            if (enemyHealth != null) // Does it exist?
            {
                enemyHealth.TakeDamage(damagePerShot, rayHitPoint.point); // Murdertime.
            }

            line.SetPosition(1, rayHitPoint.point); // Set line render position 2 to where we hit
        }

        else // If we hit nothing
        {
            line.SetPosition(1, shotRay.origin + shotRay.direction * range); // Set second position to the maximum range
        }
    }
}