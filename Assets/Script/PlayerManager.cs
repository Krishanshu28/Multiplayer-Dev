using UnityEngine.EventSystems;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Com.DefalutCompany.PhotonTest
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {

        [Tooltip("The current health of our player")]
        public float health = 1.0f;
        public static GameObject LocalPlayerInstance;

        #region Private Fields
        [SerializeField]
        GameObject beams;

        bool isFiring;

        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
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
            //used in GameManager.cs we keep track of local Player to prevent instantiation when levels are synchronized
            if(photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }
            //we flag as don't destroy on load so that instance survives level synchronization
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
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
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

        void CalledOnLevelWasLoaded(int level)
        {
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
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

        public override void OnDisable()
        {
            base.OnDisable();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        #endregion
    }
}

