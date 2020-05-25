using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace fpsMultiplayer
{
    public class Weapon : MonoBehaviourPunCallbacks
    {
        #region Variables
        public Gun[] loadout;
        private float currentCooldown;
        public Transform weaponParent;

        private GameObject currentWeapon;

        private int currentIndex;

        //Bullet holes
        public GameObject bulletHolePrefab;
        public LayerMask canBeShot;

        #endregion

        #region MonoBehavior callbacks

        // Update is called once per frame
        void Update()
        {
            //Check if this is the controlled Player
            if (!photonView.IsMine)
            {
                return;
            }

            //Further code works only for the controlled instance of the player




            //current weapon error maybe here
            if (Input.GetKeyDown("q") && currentWeapon == null) Equip(0);

            if (currentWeapon != null)
            {
                Aim(Input.GetMouseButton(1));
                if (Input.GetMouseButtonDown(0) && currentCooldown <= 0)
                {
                    shoot();
                }

                //WEAPON POSITION ELASTICITY
                currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);


                //COOLDOWN
                if (currentCooldown > 0) currentCooldown -= Time.deltaTime;

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

            //SWAY script will only load if this weapon script is accessing the controlled player
            t_newEquipment.GetComponent<Sway>().enabled = photonView.IsMine;

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

        void shoot()
        {
            Transform t_spawn = transform.Find("Cameras/Camera");

            //Bloom

            Vector3 t_bloom = t_spawn.position + t_spawn.forward * 1000f;
            t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * t_spawn.up;
            t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * t_spawn.right;
            t_bloom -= t_spawn.position;
            t_bloom.Normalize();


            //Raycast

            RaycastHit t_hit = new RaycastHit();
            //t_bloom can be replace by t_spawn.forawrd
           if( Physics.Raycast(t_spawn.position,t_bloom, out t_hit,1000f, canBeShot))
            {

                GameObject t_newHole = Instantiate(bulletHolePrefab, t_hit.point + t_hit.normal * 0.001f, Quaternion.identity) as GameObject;
                t_newHole.transform.LookAt(t_hit.point + t_hit.normal);
                Destroy(t_newHole, 15f);
            }



            //Gun FX
            currentWeapon.transform.Rotate(-loadout[currentIndex].recoil, 0, 0);
            currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[currentIndex].kickBack;

            //cooldown

            currentCooldown = loadout[currentIndex].fireRate;
                
        }
        #endregion
    }
}

