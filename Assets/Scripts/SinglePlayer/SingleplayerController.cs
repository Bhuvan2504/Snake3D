using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleplayerController : MonoBehaviourPunCallbacks
{
    public PlayerManager playerManager;
    public GameObject SnakeTailPrefab;
    int score;
    private List<GameObject> SnakeTail = new List<GameObject>();
    private List<Vector3> SnakeTailPosition = new List<Vector3>();
    SingleplayerManager singleplayerManager;
    private void Start()
    {
        singleplayerManager = SingleplayerManager.Instance;   
    }
    void Update()
    {
        if (!singleplayerManager.isGameStarted)
            return;

        playerManager.SnakeHeadMovement();
        playerManager.UpdateSnakePosition(SnakeTailPosition, SnakeTail);
    }
    private void AddSnake()
    {
        GameObject body = Instantiate(SnakeTailPrefab);
        SnakeTail.Add(body);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Food")
        {
            Destroy(other.gameObject);
            score++;
            AddSnake();
            singleplayerManager.SpawnFood();
            singleplayerManager.ScoreTxt.text = "Score: " + score.ToString();
        }

        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Player")
        {
            singleplayerManager.ScoreTxt.text = "Game Over" + "\n Final Score: " + score;
            singleplayerManager.isGameStarted= false;
            singleplayerManager.ReloadLevelButton.gameObject.SetActive(true);
        }
    }
}