using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace fpsMultiplayer
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        //Connect to server and run settings
        public void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            connect();
        }


        //As soon as connected join a random room
        public override void OnConnectedToMaster()
        {
            Debug.Log("CONNECTED");

            join();
            
            base.OnConnectedToMaster();
        }


        //As soon as joined a room run a start game function
        public override void OnJoinedRoom()
        {
            startGame();
            base.OnJoinedRoom();
        }


        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            create();
            base.OnJoinRandomFailed(returnCode, message);
        }

        //Create a room
        public void create()
        {
            PhotonNetwork.CreateRoom("");
        }


        //Connect to photon network and apply settings
        public void connect()
        {
            Debug.Log("Trying to connect...");
            PhotonNetwork.GameVersion = "0.0.0";
            PhotonNetwork.ConnectUsingSettings();
        }


        // Join random room
        public void join()
        {
            PhotonNetwork.JoinRandomRoom();
        }


        // Load scene if there is only one player
        public void startGame()
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel(1);
            }
        }
    }
}
