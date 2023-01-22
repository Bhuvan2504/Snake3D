using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerController : MonoBehaviourPunCallbacks
{
    float SnakeMoveSpeed = 5;
    float SnakeTurnSpeed = 180;
    float TailFollowSpeed = 5;
    public int TailSpawnGap = 5;

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
    NetworkManager networkManager;
    private void Start()
    {
        networkManager = NetworkManager.Instance;
    }
    void Update()
    {
        if (!networkManager.isGameStart || !photonView.IsMine)
            return;

        SnakeHeadMovement();
        UpdateSnakePosition();
    }
    Touch touch;
    void SnakeHeadMovement()
    {
        transform.position += transform.forward * SnakeMoveSpeed * Time.deltaTime;

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                transform.Rotate(Vector3.up * touch.deltaPosition.x * SnakeTurnSpeed * Time.deltaTime);
            }
        }
        float snakeDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * snakeDirection * SnakeTurnSpeed * Time.deltaTime);

    }

    void UpdateSnakePosition()
    {
        SnakeTailPosition.Insert(0, transform.position);

        int index = 0;
        foreach (var body in SnakeTail)
        {
            Vector3 point = SnakeTailPosition[Mathf.Clamp(index * TailSpawnGap, 0, SnakeTailPosition.Count - 1)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * TailFollowSpeed * Time.deltaTime;
            body.transform.LookAt(point);

            index++;
        }
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
