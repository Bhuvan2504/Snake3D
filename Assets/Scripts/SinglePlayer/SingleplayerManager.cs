using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleplayerManager : MonoBehaviour
{
    public static SingleplayerManager Instance;
    public GameObject FoodPrefab;
    public GameObject PlayerPrefab;
    public List<Transform> playerSpawn = new List<Transform>();
    public Vector3 foodSpawnArea;
    public bool isGameStarted = false;
    public TMPro.TextMeshProUGUI ScoreTxt;
    public bool isMulti = false;


    public int playerCount;
    public Button ReloadLevelButton;
    private void Awake()
    {
        Instance= this;
        SceneManager.sceneLoaded += OnSceneLoaded;
        ReloadLevelButton.onClick.AddListener(() => ReloadLevel());
    }

    private void Start()
    {
        SpawnFood();
    }

   
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        GameObject player = Instantiate(PlayerPrefab, playerSpawn[0].position, playerSpawn[0].rotation);
    }
    public void StartGame()
    {
        if (!isGameStarted && !isMulti)
        {
            isGameStarted = true;
        }
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(1);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SpawnFood()
    {
        Vector3 foodPos = transform.position + new Vector3(Random.Range(-foodSpawnArea.x / 2, foodSpawnArea.x / 2), 1f, Random.Range(-foodSpawnArea.z / 2, foodSpawnArea.z / 2));
        GameObject food = Instantiate(FoodPrefab, foodPos, Quaternion.identity);
        food.name = "Food";
    }
}
