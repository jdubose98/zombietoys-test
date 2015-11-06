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
        // Reset the timer.
        timer = 0f;

        // Play the gun shot audioclip.
        gunAudio.Play();

        // Enable the lights.
        gunLight.enabled = true;
        faceLight.enabled = true;

        // Stop the particles from playing if they were, then start the particles.
        gunParticles.Stop();
        gunParticles.Play();

        // Enable the line renderer and set it's first position to be the end of the gun.
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        // more shotgun spam.
        if (Shotgun == true) {
        gunLine1.enabled = true;
        gunLine1.SetPosition(0, transform.position);
        gunLine2.enabled = true;
        gunLine2.SetPosition(0, transform.position);
        gunLine3.enabled = true;
        gunLine3.SetPosition(0, transform.position); }

        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Shotgun == true)
        {
            shootRay2.origin = transform.position;
            shootRay2.direction = transform.forward + new Vector3(0, 0, Random.Range(-.6f, .6f));

            shootRay3.origin = transform.position;
            shootRay3.direction = transform.forward + new Vector3(0, 0, Random.Range(-.6f, .6f));

            shootRay4.origin = transform.position;
            shootRay4.direction = transform.forward + new Vector3(0, 0, Random.Range(-.6f, .6f));
        }

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            // Try and find an EnemyHealth script on the gameobject hit.
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

            // If the EnemyHealth component exist...
            if (enemyHealth != null)
            {
                // ... the enemy should take damage.
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            }

            // Set the second position of the line renderer to the point the raycast hit.
            gunLine.SetPosition(1, shootHit.point);
        }
        // If the raycast didn't hit anything on the shootable layer...
        else
        {
            // ... set the second position of the line renderer to the fullest extent of the gun's range.
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }

        if (Shotgun == true)
        {
            //Ray 1
            if (Physics.Raycast(shootRay2, out shootHit2, range, shootableMask))
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                EnemyHealth enemyHealth = shootHit2.collider.GetComponent<EnemyHealth>();

                // If the EnemyHealth component exist...
                if (enemyHealth != null)
                {
                    // ... the enemy should take damage.
                    enemyHealth.TakeDamage(damagePerShot, shootHit2.point);
                }

                // Set the second position of the line renderer to the point the raycast hit.
                gunLine1.SetPosition(1, shootHit2.point);
            }
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                gunLine1.SetPosition(1, shootRay2.origin + shootRay2.direction * range);
            }
            
            //Ray 2
            if (Physics.Raycast(shootRay3, out shootHit3, range, shootableMask))
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                EnemyHealth enemyHealth = shootHit3.collider.GetComponent<EnemyHealth>();

                // If the EnemyHealth component exist...
                if (enemyHealth != null)
                {
                    // ... the enemy should take damage.
                    enemyHealth.TakeDamage(damagePerShot, shootHit3.point);
                }

                // Set the second position of the line renderer to the point the raycast hit.
                gunLine2.SetPosition(1, shootHit3.point);
            }
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                gunLine2.SetPosition(1, shootRay3.origin + shootRay3.direction * range);
            }

            //Ray 3
            if (Physics.Raycast(shootRay4, out shootHit4, range, shootableMask))
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                EnemyHealth enemyHealth = shootHit4.collider.GetComponent<EnemyHealth>();

                // If the EnemyHealth component exist...
                if (enemyHealth != null)
                {
                    // ... the enemy should take damage.
                    enemyHealth.TakeDamage(damagePerShot, shootHit4.point);
                }

                // Set the second position of the line renderer to the point the raycast hit.
                gunLine3.SetPosition(1, shootHit4.point);
            }
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                gunLine3.SetPosition(1, shootRay4.origin + shootRay4.direction * range);
            }
        }
    }
}