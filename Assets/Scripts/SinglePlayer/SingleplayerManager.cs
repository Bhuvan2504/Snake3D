using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleplayerManager : MonoBehaviour
{
    public static SingleplayerManager Instance;
    public GameObject FoodPrefab;
    public GameObject PlayerPrefab;
    public Transform playerSpawn;
    public Vector3 foodSpawnArea;
    public bool isGameStarted = false;
    public TMPro.TextMeshProUGUI ScoreTxt;

    public Button ReloadLevelButton;
    int foodSpawnNumber = 0;

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
        GameObject player = Instantiate(PlayerPrefab, playerSpawn.position, playerSpawn.rotation);
    }
    public void StartGame()
    {
        if (!isGameStarted)
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
        foodSpawnNumber++;
        Vector3 foodPos = transform.position + new Vector3(Random.Range(-foodSpawnArea.x / 2, foodSpawnArea.x / 2), 1f, Random.Range(-foodSpawnArea.z / 2, foodSpawnArea.z / 2));
        GameObject food = Instantiate(FoodPrefab, foodPos, Quaternion.identity);

        if (foodSpawnNumber % 2 != 0)
            Destroy(food.GetComponent<FoodMovement>());

        food.name = "Food";
    }
}
