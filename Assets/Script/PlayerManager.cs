using UnityEngine.EventSystems;
using UnityEngine;
using Photon.Pun;
using System.Collections;

namespace Com.DefalutCompany.PhotonTest
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {

        [Tooltip("The current health of our player")]
        public float health = 1.0f;

        #region Private Fields
        [SerializeField]
        GameObject beams;

        bool isFiring;
        #endregion
        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(isFiring);
                stream.SendNext(health);
            }
            else
            {
                // Network player, receive data
                this.isFiring = (bool)stream.ReceiveNext();
                this.health = (float)stream.ReceiveNext();
            }

        }

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
        void Start()
        {
            CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();

            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
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
            if (photonView.IsMine)
            {
                ProcessInputs();
            }
            //trigger Beams active state
            if (beams != null && isFiring != beams.activeInHierarchy) 
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

