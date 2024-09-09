using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    public float health = 100f;
    public PostProcessVolume postProcesVolume;
    private ColorGrading colorGrading;
    public GameObject Morte;
    public InGameMenu inGameMenu;
    public List<GameObject> Enemies;
    public Text EnemiesDead;
    public int EnemiesDeadCounter;

    private void Update()
    {
        postProcesVolume.profile.TryGetSettings(out colorGrading);

        if (Win())
        {
            inGameMenu.ToMainMenu();
        }

        EnemiesDead.text = "Enemies Dead: " + EnemiesDeadCounter + "/6";
    }
    
    public void TakeDamage(float amount)
    {
        health -= amount;
        colorGrading.colorFilter.value.g -= amount / 100;
        colorGrading.colorFilter.value.b -= amount / 100;

        if (health <= 0f)
        {
            Die();
        }          
    }

    bool Win()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (!Enemies[i].GetComponentInChildren<EnemyController>().Dead)
                return false;
        }
        return true;
    }
    void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = enabled;
        Morte.SetActive(true);
        inGameMenu.Pause();
    }
}

