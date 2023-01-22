using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerController : MonoBehaviourPunCallbacks
{
    public GameObject SnakeTailPrefab;
    public PlayerManager playerManager;
    int myScore;
    int score
    {
        get { return myScore; }
        set
        {
            if (photonView.IsMine)
            {
                myScore = value;
                photonView.RPC("UpdateScore", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, value);
            }
        }
    }
    List<GameObject> SnakeTail = new List<GameObject>();
    List<Vector3> SnakeTailPosition = new List<Vector3>();
    public TextMeshProUGUI myScoreText;
    NetworkManager networkManager;
    private void Start()
    {
        networkManager = NetworkManager.Instance;
    }
    void Update()
    {
        if (!networkManager.isGameStart || !photonView.IsMine)
            return;

        playerManager.SnakeHeadMovement();
        playerManager.UpdateSnakePosition(SnakeTailPosition,SnakeTail);
    }

    private void AddSnake()
    {
        GameObject body = PhotonNetwork.Instantiate("Multiplayer/SnakeTail", Vector3.zero,Quaternion.identity);
        SnakeTail.Add(body);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Food")
        {
            Destroy(other.gameObject);
            score++;
            AddSnake();
            networkManager.SpawnFoodRPC();

        }
    }


    [PunRPC]
    void UpdateScore(int actornumber, int value)
    {
        networkManager.playerScore[actornumber %2].text = "P"+actornumber+": "+ value.ToString();
    }
}
