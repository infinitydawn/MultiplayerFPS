using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace fpsMultiplayer
{
    public class Sway : MonoBehaviourPunCallbacks
    {
        #region Variables
        public float intensity;
        public float smooth;
        private Quaternion rotOrigin;

        #endregion

        #region MonoBehavior callbacks

        private void Start()
        {
            rotOrigin = transform.localRotation;
        }
        private void Update()
        {
            //Check if this is the controlled Player
            
            //Further code works only for the controlled instance of the player

            updateSway();
        }
        #endregion

        #region private methods
        private void updateSway()
        {
            //controls

            float t_Xmouse = Input.GetAxisRaw("Mouse X");
            float t_Ymouse = Input.GetAxisRaw("Mouse Y");

            //Calculate Target Rotation

            Quaternion t_X_adj = Quaternion.AngleAxis(-intensity * t_Xmouse, Vector3.up);
            Quaternion t_Y_adj = Quaternion.AngleAxis(intensity * t_Ymouse, Vector3.right);

            Quaternion targetRotation = rotOrigin * t_X_adj * t_Y_adj;

            //Rotate towards target rot
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
        }
        #endregion
    }
}

