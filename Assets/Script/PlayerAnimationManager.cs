using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.DefalutCompany.PhotonTest
{
    public class PlayerAnimationManager : MonoBehaviourPun
    {
        private Animator animator;
        #region MonoBehaviour Callbacks

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            if(!animator)
            {
                Debug.LogError("PlayerAnimatorManager is missing Animator", this);
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            if(!animator)
            {
                return;
            }

            //deal with jumping
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            //only allow jumping if we are running
            if (stateInfo.IsName("Base Layer.Run"))
            {
                //when using trigger parameter
                if (Input.GetButtonDown("Fire2"))
                {
                    animator.SetTrigger("Jump");
                }
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if(v<0)
            {
                v = 0;
            }
            animator.SetFloat("Speed", h*h+v*v);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);


        }
        #endregion

        #region Private Fields
        [SerializeField]
        float directionDampTime = 0.25f;

        #endregion

    }
}
