using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenoOpening : MonoBehaviour
{
    public static bool GameIsPaused = false;

    //public GameObject pauseMenuUIPrefab;
    public GameObject pauseMenuUI;
    //public GameObject encyclopediaMenuUIPrefab;
    public GameObject encyclopediaMenuUI;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        PlayerAbilityManager.instance.EnableAll();
        pauseMenuUI.SetActive(false);
        
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PlayerAbilityManager.instance.DisableAll();
        //pauseMenuUI.SetActive(true);
        
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Loading Menu");
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void PauseMenuInput(InputAction.CallbackContext ctx)
    {
        if (PlayerAbilityManager.instance.inDialogue) { return; }
        if (ctx.started)
        {
                Debug.Log("Turning off pause menu, right?");
            if (pauseMenuUI.activeSelf)
            {
                //Destroy(pauseMenuUIInstance);
                pauseMenuUI.SetActive(false);
                Resume();
            }
            else
            {
                if (!encyclopediaMenuUI.activeSelf)
                {
                    //pauseMenuUIInstance = Instantiate(pauseMenuUIPrefab, Vector3.zero, Quaternion.identity);
                    pauseMenuUI.SetActive(true);
                    Pause();
                }
            }
        }
    }
    public void EncyclopediaMenuInput(InputAction.CallbackContext ctx)
    {
        if(PlayerAbilityManager.instance.inDialogue) { return; }
        if (ctx.started)
        {
            if (encyclopediaMenuUI.activeSelf)
            {
                encyclopediaMenuUI.SetActive(false);
                Resume();
            }
            else
            {
                if (!pauseMenuUI.activeSelf)
                {
                    encyclopediaMenuUI.SetActive(true);
                    Pause();
                }
            }
        }
    }
}