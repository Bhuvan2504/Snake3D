using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    int lobbyNumber = 0;
    RoomOptions roomOptions = new RoomOptions()
    {
        MaxPlayers = 2,
    };

    void Start()
    {
        PhotonNetwork.Disconnect();

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonCreateRoom();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == 2)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(3);
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == 2)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(3);
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        PhotonNetwork.JoinRoom("Snake Lobby" + lobbyNumber);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        lobbyNumber++;
        PhotonCreateRoom();
    }

    void PhotonCreateRoom()
    {
        PhotonNetwork.CreateRoom("Snake Lobby" + lobbyNumber, roomOptions, TypedLobby.Default);

    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   