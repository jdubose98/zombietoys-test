using UnityEngine;
using UnityEngine.UI;
using System.Collections;

    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] int startingHealth = 100;                              // Amount of health at start
        public int currentHealth;                                               // Current HP
        [SerializeField] Slider healthSlider;                                   // UI health bar
        [SerializeField] Image damageImage;                                     // Reference to the image
        [SerializeField] AudioClip deathClip;                                   // Sound to play
        [SerializeField] float flashSpeed = 5f;                                 // The speed the damageImage will fade at.
        [SerializeField] Color flashColour = new Color(1f, 0f, 0f, 0.1f);       // The colour the damageImage is set to, to flash.
        float timeFromLastHit = Time.time;                                      // Captures the time
        [SerializeField] int regenTime = 5;    	    				        	// how long it takes to start healing
        [SerializeField] int regenPts = 1;    	    				        	// how many health points we get per "tick"
        [SerializeField] Text healthText;                                       // a reference to the text


        Animator anim;                                                          // Reference to the Animator component.
        AudioSource playerAudio;                                                // Reference to the AudioSource component.
        PlayerMovement playerMovement;                                          // Reference to the player's movement.
        PlayerShooting playerShooting;                                          // Reference to the PlayerShooting script.
        bool isDead;                                                            // Whether the player is dead.
        bool damaged;                                                           // True when the player gets damaged.


        void Awake ()
        {
            // References
            anim = GetComponent <Animator> ();
            playerAudio = GetComponent <AudioSource> ();
            playerMovement = GetComponent <PlayerMovement> ();
            playerShooting = GetComponentInChildren <PlayerShooting> ();

            currentHealth = startingHealth;
        }


        void Update ()
        {
			// If the player has just been damaged...
			if (damaged) {
				damageImage.color = flashColour;
			}
            else {
				damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
			}

			// reset flag
			damaged = false;
       
			//let's be nice and have the player regenerate health after a while.
			if ((Time.time - timeFromLastHit) >= regenTime && !isDead) { // check if dead and time's passed enough
				if (currentHealth < startingHealth) {
					currentHealth = currentHealth + regenPts;
					healthSlider.value = currentHealth;
                    healthText.text = currentHealth.ToString();
                }
			}
		}

        public void TakeDamage (int amount)
        {
            // set flag
            damaged = true;

            currentHealth -= amount;

            // Set the health bar's value to the current health.
            healthSlider.value = currentHealth;
            healthText.text = currentHealth.ToString();

            // Play the hit sound
            playerAudio.Play ();

			//Now set the time since last hit.
			timeFromLastHit = Time.time;

            // doublecheck
            if(currentHealth <= 0 && !isDead)
            {
                // ... it should die.
                Death ();
            }
        }


        void Death ()
        {
            // Set the death flag so this function won't be called again.
            isDead = true;

            // Turn off any remaining shooting effects.
            playerShooting.DisableEffects ();

            // Tell the animator that the player is dead.
            anim.SetTrigger ("Die");

            // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
            playerAudio.clip = deathClip;
            playerAudio.Play ();

            // Turn off the movement and shooting scripts.
            playerMovement.enabled = false;
            playerShooting.enabled = false;
        }


        public void RestartLevel ()
        {
            // Reload the level that is currently loaded.
            Application.LoadLevel (Application.loadedLevel);
        }
    }
