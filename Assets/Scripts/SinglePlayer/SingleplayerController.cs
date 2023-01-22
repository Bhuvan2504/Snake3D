using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleplayerController : MonoBehaviourPunCallbacks
{
    float SnakeMoveSpeed = 5;
    float SnakeTurnSpeed = 180;
    float TailFollowSpeed = 5;
    public int TailSpawnGap = 5;

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