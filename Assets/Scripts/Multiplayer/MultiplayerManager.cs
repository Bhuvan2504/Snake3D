using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    #region GameLogicVariables
    public static MultiplayerManager Instance;
    public GameObject FoodPrefab;
    public Vector3 foodSpawnArea;
    public bool isGameStart = false;
    bool isTimerStart = false;
    bool isGameOver = false;
    #endregion

    public List<Transform> playerSpawn = new List<Transform>();
    public List<TextMeshProUGUI> playerScore = new List<TextMeshProUGUI>();
    int playerCount = 0;

    #region TimerVariables
    public TextMeshProUGUI timerText;
    int finalTime;
    float totalTime = 90;
    float waitTime = 5;
    private int foodSpawnNumber;
    #endregion
    void Awake()
    {
        Instance= this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Update()
    {
        if(isGameOver)
            DecleareWinner();

        Timer();
    }
    void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        int i = PhotonNetwork.IsMasterClient? 0 : 1;
        GameObject g = PhotonNetwork.Instantiate("Multiplayer/Player", playerSpawn[i].position, playerSpawn[i].rotation);
        photonView.RPC("Attendance", RpcTarget.AllBuffered);

    }
  
    void Timer()
    {
        if (!isTimerStart)
            return;

        waitTime -= Time.deltaTime;
        finalTime = (int)waitTime;
        timerText.text = "Game Start In: " + (finalTime).ToString();

        if (waitTime < 0)
        {
            waitTime = 0;
            isGameStart = true;
            totalTime -= Time.deltaTime;
            finalTime = (int)totalTime;
            timerText.text = "Timer: " + finalTime.ToString();

            if(totalTime <= 0)
            {
                totalTime  = 0;
                isGameOver = true;
            }
        }
    }
    
    void DecleareWinner()
    {
        isGameStart = false;
        isTimerStart= false;
        int P1Score = int.Parse(playerScore[0].text.Split(':').Last());
        int P2Score = int.Parse(playerScore[1].text.Split(':').Last());

        if (P1Score > P2Score)
            timerText.text = "P1 Wins\nReturning To MainMenu...";
        else
            timerText.text = "P2 Wins\nReturning To MainMenu...";

        Invoke("ToMainMenu", 3);
    }

    public void SpawnFoodRPC()
    {
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("SpawnFood", RpcTarget.All, new Vector3(Random.Range(-foodSpawnArea.x / 2, foodSpawnArea.x / 2), 1f, Random.Range(-foodSpawnArea.z / 2, foodSpawnArea.z / 2)));
    }

    void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    [PunRPC]
    void Attendance()
    {
        playerCount++;
        if (playerCount == PhotonNetwork.PlayerList.Length)
            isTimerStart = true;
    }

    [PunRPC]
    void SpawnFood(Vector3 position)
    {
        foodSpawnNumber++;
        Vector3 foodPos = transform.position + position;
        GameObject food = Instantiate(FoodPrefab, foodPos, Quaternion.identity);

        if (foodSpawnNumber % 2 != 0)
            Destroy(food.GetComponent<FoodMovement>());

        food.name = "Food";
    }
}
