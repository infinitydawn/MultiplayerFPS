using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 

namespace fpsMultiplayer
{
    public class Manager : MonoBehaviourPunCallbacks
    {
        public string playerPrefab;
        public Transform spawnPoint;



        void Start()
        {
            spawn();
        }

       public void spawn()
        {
            PhotonNetwork.Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
