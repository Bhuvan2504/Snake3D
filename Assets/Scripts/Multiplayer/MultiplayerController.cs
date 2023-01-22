using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerController : PlayerManager
{
    MultiplayerManager multiplayerManager;

    public GameObject SnakeTailPrefab;

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
    
     void Start()
    {
        multiplayerManager = MultiplayerManager.Instance;
    }
    void Update()
    {
        if (!multiplayerManager.isGameStart || !photonView.IsMine)
            return;

        SnakeHeadMovement();
        UpdateSnakePosition(SnakeTailPosition,SnakeTail);
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
            multiplayerManager.SpawnFoodRPC();

        }

        if (other.gameObject.tag == "Wall")
            multiplayerManager.isGameOver = true;
    }


    [PunRPC]
    void UpdateScore(int actornumber, int value)
    {
        multiplayerManager.playerScore[actornumber %2].text = "P"+actornumber+":"+ value.ToString();
    }
}
