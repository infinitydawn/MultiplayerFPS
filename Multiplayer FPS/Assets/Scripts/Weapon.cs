using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fpsMultiplayer
{
    public class Weapon : MonoBehaviour
    {
        #region Variables
        public Gun[] loadout;
        public Transform weaponParent;

        private GameObject currentWeapon;

        private int currentIndex;

        #endregion

        #region MonoBehavior callbacks

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) Equip(0);

            if (currentWeapon != null)
            {
                Aim(Input.GetMouseButton(1));
            }
        }
        #endregion

        #region private methods
        void Equip(int p_ind)
        {
            if (currentWeapon != null) Destroy(currentWeapon);

            currentIndex = p_ind;

            GameObject t_newEquipment = Instantiate(loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
            t_newEquipment.transform.localPosition = Vector3.zero;
            t_newEquipment.transform.localEulerAngles = Vector3.zero;

            currentWeapon = t_newEquipment;

        }

        void Aim(bool p_isAiming)
        {
            Transform t_anchor = currentWeapon.transform.Find("Anchor");
            Transform t_state_ADS = currentWeapon.transform.Find("States/ADS");
            Transform t_state_HIP = currentWeapon.transform.Find("States/Hip");

            if (p_isAiming)
            {
                //aim 

                t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_ADS.position, Time.deltaTime * loadout[currentIndex].aimSpeed );

            }
            else
            {
                //hip
                t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_HIP.position, Time.deltaTime * loadout[currentIndex].aimSpeed);


            }
        }
        #endregion
    }
}

