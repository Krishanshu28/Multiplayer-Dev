using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

namespace Com.DefalutCompany.PhotonTest
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [Tooltip("The UI Panel to let user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;

        [Tooltip("The UI label to inform user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        #endregion

        #region Private Fields
        string gameVersion = "1";
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene  = true;
        }

        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            
        }
        #endregion

        #region Public Methods
        public void Connect()
        {
            controlPanel.SetActive(false);
            progressLabel.SetActive(true);
            if(PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
        #endregion

        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            print("Connected to master");
            PhotonNetwork.JoinRandomRoom();
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("OnDisconnected() called {0}", cause);
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed()");
            PhotonNetwork.CreateRoom(null,new RoomOptions {MaxPlayers = maxPlayersPerRoom });
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom() called");
        }


        #endregion
    }
}

