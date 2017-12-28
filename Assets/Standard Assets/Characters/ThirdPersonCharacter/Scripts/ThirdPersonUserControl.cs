using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
	{
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
		private const float maxHealth = 10f;
		private float health = maxHealth;
		[SerializeField]
		private float healthPerSecond = 0.5f;
		[SerializeField]
		private Light light;
		private float healHealth = 5f;
		[SerializeField]
		private Image healthBar;
		[SerializeField]
		private GameObject tryAgain;
		[SerializeField]
		private AudioClip pickupSound;
		private AudioSource audioSource;
        
        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
			audioSource = GetComponent<AudioSource> ();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

			DecreaseHealth ();
        }

		// Decrease health of character per second
		private void DecreaseHealth() {
			if (health - healthPerSecond * Time.deltaTime > 0) {
				health -= healthPerSecond * Time.deltaTime;
			} else {
				health = 0;
				tryAgain.SetActive (true);
				gameObject.SetActive (false);
			}
			light.range = health;
			healthBar.fillAmount = health / 10f;
		}

		// Heal character if he picks up the bulb
		private void Heal() {
			health = maxHealth;
			light.range = health;
			healthBar.fillAmount = health / 10f;
			audioSource.PlayOneShot (pickupSound);
		}

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v*m_CamForward + h*m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v*Vector3.forward + h*Vector3.right;
            }
#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }

		void OnTriggerEnter(Collider other) {
			if (other.CompareTag ("Bulb")) {
				Heal ();
			} else if(other.CompareTag ("Exit")){
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			}
		}
    }
}
