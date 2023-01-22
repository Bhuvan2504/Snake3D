using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void LoadSinglePlayer()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMultiplayer()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
