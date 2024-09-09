using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaves : MonoBehaviour
{
    [SerializeField]
    float mouseSensivity=100;
    // Start is called before the first frame update
    public void MouseSensitivity(float ms)
    {
        PlayerPrefs.SetFloat("Sensibilita", ms);
    }


    // Update is called once per frame
    void Update()
    {
        print("Sensibilita: " + PlayerPrefs.GetFloat("Sensibilita"));
    }
}
