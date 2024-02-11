using UnityEngine.EventSystems;
using UnityEngine;
using Photon.Pun;
using System.Collections;

namespace Com.DefalutCompany.PhotonTest
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("The current health of our player")]
        public float health = 1.0f;

        #region Private Fields
        [SerializeField]
        GameObject beams;

        bool isFiring;
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if(beams == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
            }
            else
            {
                beams.SetActive(false);
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine)
            {
                ProcessInputs();
                if (health <= 0f)
                {
                    GameManager.Instance.LeaveRoom();
                }
            }
            // ProcessInputs();
            //trigger Beams active state
            if(beams != null && isFiring != beams.activeInHierarchy) 
            {
                beams.SetActive(isFiring);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!photonView.IsMine)
            {
                return;
            }

            if(!other.name.Contains("Beams"))
            {
                return;
            }
            health -= 0.1f;
        }
        private void OnTriggerStay(Collider other)
        {
            if(!photonView.IsMine)
            {
                return;
            }
            if (!other.name.Contains("Beams"))
            {
                return;
            }
            health -= 0.1f * Time.deltaTime;
        }

        #endregion
        #region Custom
        void ProcessInputs()
        {
            if(Input.GetButtonDown("Fire1"))
            {
                if(!isFiring)
                {
                    isFiring = true;
                }

            }
            if(Input.GetButtonUp("Fire1"))
            {
                if(isFiring)
                {
                    isFiring = false;
                }

            }
        }
        #endregion
    }
}

