using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;
    public GameObject FoodPrefab;
    public Vector3 foodSpawnArea;
    public bool isGameStart = false;
    public bool isTimerStart = false;
    public bool isGameOver = false;
    public List<Transform> playerSpawn = new List<Transform>();
    public List<TextMeshProUGUI> playerScore = new List<TextMeshProUGUI>();
    int playerCount = 0;
    float totalTime = 90;
    int finalTime;
    float waitTime = 5;
    public TextMeshProUGUI timerText;
    void Awake()
    {
        Instance= this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Update()
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
        int P2Score = int.Parse(playerScore[0].text.Split(':').Last());

        if (P1Score > P2Score)
            timerText.text = "P1 Wins";
        else
            timerText.text = "P2 Wins";


    }

    public void SpawnFoodRPC()
    {
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("SpawnFood", RpcTarget.All, new Vector3(Random.Range(-foodSpawnArea.x / 2, foodSpawnArea.x / 2), 1f, Random.Range(-foodSpawnArea.z / 2, foodSpawnArea.z / 2)));
    }

    [PunRPC]
    void Attendance()
    {
        playerCount++;
        if (playerCount == PhotonNetwork.PlayerList.Length)
            isTimerStart = true;
    }

    [PunRPC]
    public void SpawnFood(Vector3 position)
    {
        Vector3 foodPos = transform.position + position;
        GameObject food = Instantiate(FoodPrefab, foodPos, Quaternion.identity);
        food.name = "Food";
    }
}
