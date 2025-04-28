using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject img;

    //Game cena 0, Menu cena 1

    void Update()
    {
        ReturnMenu();
        CallPauseResume();
        Restart();
    }

    public void ReturnMenu()
    {      
               
        SceneManager.LoadScene(0);
        
    }

    public void CallPauseResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
            
        }
        //tem que botar o controle
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        img.SetActive(true);

    }

    public void Resume ()
    {
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        img.SetActive(false);
    }

    public void Restart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   
}
