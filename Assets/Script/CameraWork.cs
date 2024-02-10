using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.DefalutCompany.PhotonTest
{
    public class CameraWork : MonoBehaviour
    {
        #region Private Fields
        [Tooltip("The distance in the local x-z plane to the target")]
        [SerializeField]
        float distance = 7.0f;

        [SerializeField]
        float height = 3.0f, smoothSpeed = 0.125f;

        [SerializeField]
        Vector3 cameraOffset = Vector3.zero, centreOffset = Vector3.zero;

        [SerializeField]
        bool followOnStart = false, isFollowing;

        Transform cameraTransform;
        #endregion

        #region MonoBehaviour Callbacks
        // Start is called before the first frame update
        void Start()
        {
            if(followOnStart)
            {
                OnStartFollowing();
            }
        }

        void LateUpdate()
        {
            if (cameraTransform == null && isFollowing)
            {
                OnStartFollowing();
            }
            if(isFollowing)
            {
                Follow();
            }
        }
        #endregion
        #region Public Methods
        public void OnStartFollowing()
        {
            cameraTransform = Camera.main.transform;
            isFollowing = true;
            // we don't smooth anything, we go straight to the right camera shot
            Cut();
        }
        #endregion
        #region Private Methods

        /// <summary>
        /// Follow the target smoothly
        /// </summary>
        void Follow()
        {
            cameraOffset.z = -distance;
            cameraOffset.y = height;

            cameraTransform.position = Vector3.Lerp(cameraTransform.position, this.transform.position + this.transform.TransformVector(cameraOffset), smoothSpeed * Time.deltaTime);

            cameraTransform.LookAt(this.transform.position + centreOffset);
        }


        void Cut()
        {
            cameraOffset.z = -distance;
            cameraOffset.y = height;

            cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);

            cameraTransform.LookAt(this.transform.position + centreOffset);
        }
        #endregion
    }
}


   

