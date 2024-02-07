using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

namespace Com.DefalutCompany.PhotonTest
{
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants
        //Store PlayerPrefs Key to avoid typos
        const string playerNamePrefKey = "PlayerName";
        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            string defaultName = string.Empty;
            InputField _inputField = this.GetComponent<InputField>();
            if(_inputField != null )
            {
                if(PlayerPrefs.HasKey(playerNamePrefKey) )
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.name = defaultName;
                }
            }
            PhotonNetwork.NickName = defaultName;
        }
        #endregion
        /// <summary>
        /// Sets the name of the player and saves it to PlayerPrefs
        /// </summary>
        /// <param name="value"></param>The name of the player</param>
        #region Public Methods
        public void SetPlayername(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                Debug.Log("Player name is null or empty");
                return;
            }
            PhotonNetwork.NickName=value;
            PlayerPrefs.SetString(playerNamePrefKey, value);
        }
        #endregion
    }
}

