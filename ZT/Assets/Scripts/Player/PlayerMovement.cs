using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;


    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 6f;            // controls the player's speed (public due to orbscript)

        Vector3 movement;                   // stores player's movement
        Animator anim;                      // to the animator...
        Rigidbody playerRigidbody;          // to the rigidbody! (to the rigidbody)

        int floorMask;                      // so rays only hit the floor layer
        float camRayLength = 100f;          // kind of arbitrary, but needed for the raycast

        void Awake ()
        {
            floorMask = LayerMask.GetMask ("Floor"); // make a mask for our raycasts...

            // make references, maybe one day they will get jobs and support themselves. freeloaders.
            anim = GetComponent <Animator> ();
            playerRigidbody = GetComponent <Rigidbody> ();
        }


        void FixedUpdate ()
        {
            // Store the input axes.
            float m_Horizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            float m_Vertical = CrossPlatformInputManager.GetAxisRaw("Vertical");

            // Move the player around the scene.
            Move (m_Horizontal, m_Vertical);

            // Turn the player to face the mouse cursor.
            Turning ();

            // Animate the player.
            Animating (m_Horizontal, m_Vertical);
        }


        void Move (float m_HorizontalMove, float m_VerticalMove)
        {
            movement.Set (m_HorizontalMove, 0f, m_VerticalMove); // get axes and set movement
            movement = movement.normalized * speed * Time.deltaTime; // normalize vector over time so we can't cheat by strafing
            playerRigidbody.MovePosition (transform.position + movement); // and then we move the player.
        }


        void Turning ()
        {
            Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition); // cast a ray from the cursor to the world

            RaycastHit m_floorHit; // store what we found out here

            // Perform the raycast and if it hits something on the floor layer...
            if(Physics.Raycast (camRay, out m_floorHit, camRayLength, floorMask))
            {
                Vector3 m_playerToMouse = m_floorHit.point - transform.position; // makes a vector from the player to where we hit the floor
                m_playerToMouse.y = 0f; // put it along the floor plane
                Quaternion m_newRotatation = Quaternion.LookRotation (m_playerToMouse); // create the quaternion... ew, quaternions.
                playerRigidbody.MoveRotation (m_newRotatation); // and then set the rotation
            }


            Vector3 m_turnDir = new Vector3(CrossPlatformInputManager.GetAxisRaw("Mouse X") , 0f , CrossPlatformInputManager.GetAxisRaw("Mouse Y"));

            if (m_turnDir != Vector3.zero)
            {
                //create a vector from the player to the point where mouse hit
                Vector3 m_playerToMouse = (transform.position + m_turnDir) - transform.position;
                m_playerToMouse.y = 0f; // mouse on the foor
                Quaternion newRotatation = Quaternion.LookRotation(m_playerToMouse); // make quaternion
                playerRigidbody.MoveRotation(newRotatation); // set player rotation
            }

        }


        void Animating (float h, float v)
        {
            // detects if the player's inputs are non-zero, then tells the animator to work
            bool m_walking = h != 0f || v != 0f;
            anim.SetBool ("IsWalking", m_walking);
        }
    }
