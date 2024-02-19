using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.DefalutCompany.PhotonTest
{
    public class PlayerUI : MonoBehaviour
    {
        #region Private Fields

        [SerializeField]
        private Text playerNameText;

        [SerializeField]
        private Slider playerHealthSlider;

        private PlayerManager target;

        [SerializeField]
        private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

        float characterControllerHeight = 0f;
        Transform targetTransform;
        Renderer targetRenderer;
        CanvasGroup _canvasGroup;
        Vector3 targetPosition;
        #endregion

        #region MonoBehaviour callbacks
        void Awake()
        {
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
            _canvasGroup = this.GetComponent<CanvasGroup>();
        }
        private void Update()
        {
            //reflect the player health
            if(playerHealthSlider != null)
            {
                playerHealthSlider.value = target.health;
            }
            // Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
            if (target == null)
            {
                Destroy(this.gameObject);
                return;
            }
        }
        void LateUpdate()
        {
            // Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
            if (targetRenderer != null)
            {
                this._canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
            }

            // #Critical
            // Follow the Target GameObject on screen.
            if (targetTransform != null)
            {
                targetPosition = targetTransform.position;
                targetPosition.y += characterControllerHeight;
                this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
            }
        }

        #endregion
        #region Public Methods

        public void SetTarget(PlayerManager _target)
        {
            if(_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayerManager target for Player.UI.SetTarget", this);
                return;
            }
            //Cache reference for efficiency
            target = _target;
            if(playerNameText != null)
            {
                playerNameText.text = target.photonView.Owner.NickName;
            }
            targetTransform = this.target.GetComponent<Transform>();
            targetRenderer = this.target.GetComponent<Renderer>();
            CharacterController characterController = _target.GetComponent<CharacterController>();
            // Get data from the Player that won't change during the lifetime of this Component
            if (characterController != null)
            {
                characterControllerHeight = characterController.height;
            }
        }
        #endregion
    }
}


