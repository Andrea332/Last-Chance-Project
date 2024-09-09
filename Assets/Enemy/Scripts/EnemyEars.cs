using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEars : MonoBehaviour
{
    public InGameMenu inGameMenu;
    public Gun PlayerGun;
    private void Start()
    {
        inGameMenu = GameObject.Find("UI").GetComponent<InGameMenu>();
        PlayerGun = GameObject.Find("Rifle_06").GetComponent<Gun>();
    }
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(other.gameObject.name);

        if (!inGameMenu.GameInPause)
        {
            if(other.gameObject.GetComponentInParent<EnemyController>() != null)
            {
                if (other.gameObject.GetComponentInParent<EnemyController>().Fight)
                {
                    GetComponentInParent<EnemyController>().Fight = true;
                }
            }
            if(other.gameObject.name == "Flare(Clone)")
                GetComponentInParent<EnemyController>().Fight = true;

        }      
    }
}
