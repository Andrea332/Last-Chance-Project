using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public bool GameInPause = false; //Dichiaro una variabile che definisce se il mio gioco è in pausa oppure no
    public GameObject MenuPauseUI;  //Vado a prendere i gameobject del Menu di Pausa e il Menu di sucita per poterci lavorare nello script
    public GameObject DeadUI;
    public GameObject CrossHair;
    public GameObject OptionsMenu;
    public IntroScript introScript;
    


    private void Start()    //Facciamo partire il gioco quando si carica la scena per evitare che ci siano rimasugli delle pause precedenti
    {
        if(!introScript.introOn)
        Resume();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))   //Controlliamo se il giocatore preme il tasto ESC 
        {
            if (!DeadUI.activeSelf && GameInPause && !introScript.introOn && MenuPauseUI.activeSelf )        //Se il gioco è in pausa fallo ripartire 
            {
                MenuPauseUI.SetActive(false);       //Chiudiamo il menu di pausa
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Resume();
            }
            else if(!DeadUI.activeSelf && GameInPause && !introScript.introOn && OptionsMenu.activeSelf)
            {
                OptionsMenu.SetActive(false);
                MenuPauseUI.SetActive(true);       //Chiudiamo il menu di pausa
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else if(!DeadUI.activeSelf && !GameInPause && !introScript.introOn)                    //Se il gioco non è in pausa mettilo in pausa
            {
                MenuPauseUI.SetActive(true);    //Attiviamo il MenuDiPausa
                Cursor.visible = true;
                Pause();
            }
        }
    }

    public void Resume()            //Funzione che farà partire il gioco
    {
        Time.timeScale = 1f;                //Facciamo partire il tempo
        GameInPause = false;                //Impostiamo la variabile che tiene conto se il gioco è in pausa a false
        MenuPauseUI.SetActive(false);
        CrossHair.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void Pause()         //funzione per mettere in pausa il gioco
    {
        Time.timeScale = 0f;            //Fermiamo il tempo del gioco        
        GameInPause = true;             //Impostiamo la variabile gioco in pausa su vero
        CrossHair.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
    }


    public void ReloadLivello1()
    {
        SceneManager.LoadScene("Level 1");      
    }
    public void Options()       //Funzione che andrà abbinata al tasto opzioni
    {
        OptionsMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }


    public void Morte()
    {
        Time.timeScale = 0f;
        GameInPause = true;
        DeadUI.SetActive(true);
    }

    public void Back()                //Impostiamo la funzione per il tasto annulla
    {
        OptionsMenu.SetActive(false);
    }

    public void ToMainMenu()          //Impostiamo la funzione per il tasto esci al menu principale
    {
        SceneManager.LoadScene("Main Menu");       //Carichiamo la scena "MenuPrincipale"
    }
    public void QuitToDesktop()          //Impostiamo la funzione per il tasto esci al desktop
    {
        Debug.Log("Premuto il tasto uscita gioco");
        Application.Quit();         //Chiudiamo il gioco
    }
}
