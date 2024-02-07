using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace Com.DefalutCompany.PhotonTest
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Photon Callbacks

        //called when the local player left the room. We need to load the launcher scene
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion
    }
}

