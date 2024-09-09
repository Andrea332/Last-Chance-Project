using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEnemyController : MonoBehaviour
{
    public List<Rigidbody> RagDolls;
    public List<Collider> GunCollider;
    Animator Anim;
    public GameObject EnemyGun;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 13; i++)
        {
            RagDolls[i].isKinematic = true;
        }
        EnemyGun.GetComponent<Rigidbody>().isKinematic = true;
        Anim = GetComponent<Animator>();
    }
}
