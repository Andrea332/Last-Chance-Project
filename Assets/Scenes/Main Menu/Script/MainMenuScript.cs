using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject MissionAndControlsMenu;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void Playgame()
    {
        SceneManager.LoadScene("Loading Screen");
    }
    public void Quitgame()
    {
        Application.Quit();
    }
    public void missionAndControls()       //Funzione che andrà abbinata al tasto opzioni
    {
        MissionAndControlsMenu.SetActive(true);
    }
    public void Back()                //Impostiamo la funzione per il tasto annulla
    {
        MissionAndControlsMenu.SetActive(false);
    }

}
