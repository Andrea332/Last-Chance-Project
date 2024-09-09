using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScript : MonoBehaviour
{
    public InGameMenu inGameMenu;
    public GameObject introText;
    public bool introOn;
    public bool skipintro;


    private void Update()
    {
        if (!skipintro)
        {
            if (introText.transform.localPosition.y <= 973 && introOn)
            {
                Cursor.lockState = CursorLockMode.Locked;
                inGameMenu.Pause();
                introText.transform.localPosition += Vector3.up * 100f * Time.unscaledDeltaTime;
            }
            else if (introOn)
            {
                inGameMenu.Resume();
                gameObject.GetComponent<Image>().enabled = false;
                introText.SetActive(false);
                introOn = false;
            }
        }
        else if(introOn)
        {
            inGameMenu.Resume();
            gameObject.GetComponent<Image>().enabled = false;
            introText.SetActive(false);
            introOn = false;
        }                    
    }
 
}
