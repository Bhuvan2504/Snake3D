using System.Collections.Generic;
using UnityEngine;

public class SingleplayerController : PlayerManager
{
    SingleplayerManager singleplayerManager;

    GameObject TailParent;
    public GameObject SnakeTailPrefab;

    List<GameObject> SnakeTail = new List<GameObject>();
    List<Vector3> SnakeTailPosition = new List<Vector3>();

    int score;

    private void Start()
    {
        singleplayerManager = SingleplayerManager.Instance;
        TailParent = new GameObject();

    }
    void Update()
    {
        if (!singleplayerManager.isGameStarted)
            return;

        SnakeHeadMovement();
        UpdateSnakePosition(SnakeTailPosition, SnakeTail);
    }
    private void AddSnake()
    {
        GameObject body = Instantiate(SnakeTailPrefab, (transform.position-transform.forward * 3),Quaternion.identity);
        if(SnakeTail.Count > 2)
            body.tag = "Tail";
        body.transform.parent = TailParent.transform;
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

        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Tail")
        {
            singleplayerManager.ScoreTxt.text = "Game Over" + "\n Final Score: " + score;
            singleplayerManager.isGameStarted= false;
            singleplayerManager.ReloadLevelButton.gameObject.SetActive(true);
        }
    }
}