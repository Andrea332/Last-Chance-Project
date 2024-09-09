using System.Collections;
using UnityEngine;
using UnityEngine.UI;


//Script che si occupa di gestire le armi del player
public class Gun : MonoBehaviour
{

    public float Damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public Text Ammo;
    public int maxAmmo = 10;
    public int currentAmmo;
    public float reloadTime = 1f;
    public bool isReloading = false;

    private InGameMenu inGameMenu;
    public Camera fpsCam;
    public AudioSource GunSound;
    public ParticleSystem GunFlash;
    public GameObject ImpactEffect;
    public GameObject ImpactEffectEnemy;
    public LayerMask Enemy;

    private float nextTimeToFire = 0f;

    public Animator Anim;
    public RaycastHit hit;
    public EnemyController EC;
    public GameControllerScript GCS;

    private void Start()
    {
        inGameMenu = GameObject.Find("UI").GetComponent<InGameMenu>();
        currentAmmo = maxAmmo;
        Ammo = GameObject.Find("Ammo").GetComponent<Text>();
        fpsCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Anim = GameObject.Find("WeaponSwitch").GetComponent<Animator>();
    }

    private void OnEnable()
    {
        isReloading = false;
        Anim.SetBool("IsReloading", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inGameMenu.GameInPause)
        {
            if (isReloading)
                return;

            if (currentAmmo <= 0 || (Input.GetKeyDown(KeyCode.R) && currentAmmo != maxAmmo))
            {
                StartCoroutine(Reload());
                return;
            }

            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }

            Ammo.text = "Ammo: " + currentAmmo;
        }      
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        Anim.SetBool("IsReloading", true);

        yield return new WaitForSeconds(reloadTime - 0.25f);

        Anim.SetBool("IsReloading", false);

        yield return new WaitForSeconds(0.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void Shoot()
    {
        GunFlash.Play();
        GunSound.Play();

        currentAmmo--;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, Enemy, QueryTriggerInteraction.Ignore))
        {

            //Debug.Log(hit.transform.name);

            /*EnemyController enemy = hit.transform.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
            }*/

            switch (hit.transform.name)
            {
                case "mixamorig:Hips":
                    BulletOnEnemy(); EC.TakeDamage(Damage); 
                    break;
                case "mixamorig:LeftUpLeg":
                    BulletOnEnemy(); EC.TakeDamage(Damage / 2); 
                    break;
                case "mixamorig:LeftLeg":
                    BulletOnEnemy(); EC.TakeDamage(Damage / 2);
                    break;
                case "mixamorig:RightUpLeg":
                    BulletOnEnemy(); EC.TakeDamage(Damage / 2); 
                    break;
                case "mixamorig:RightLeg":
                    BulletOnEnemy(); EC.TakeDamage(Damage / 2);
                    break;
                case "mixamorig:Spine":
                    BulletOnEnemy(); EC.TakeDamage(Damage);
                    break;
                case "mixamorig:LeftArm":
                    BulletOnEnemy(); EC.TakeDamage(Damage / 2);
                    break;
                case "mixamorig:LeftForeArm":
                    BulletOnEnemy(); EC.TakeDamage(Damage / 2);
                    break;
                case "mixamorig:LeftHand":
                    BulletOnEnemy(); EC.TakeDamage(Damage / 2);
                    break;
                case "mixamorig:Head":
                    BulletOnEnemy(); EC.TakeDamage(Damage * 4);
                    break;
                case "mixamorig:RightArm":
                    BulletOnEnemy(); EC.TakeDamage(Damage / 2);
                    break;
                case "mixamorig:RightForeArm":
                    BulletOnEnemy(); EC.TakeDamage(Damage / 2);
                    break;
                case "mixamorig:RightHand":
                    BulletOnEnemy(); EC.TakeDamage(Damage / 2);
                    break;
            }
        }
        else if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, ~Enemy, QueryTriggerInteraction.Ignore))
        {
                    BulletOnSomething();                   
        }
    }

    public void BulletOnEnemy()
    {
        EC = hit.transform.GetComponentInParent<EnemyController>();
        EC.Fight = true;
        GameObject impactGo = Instantiate(ImpactEffectEnemy, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impactGo, 0.5f);
    }
    public void BulletOnSomething()
    {
        if (hit.rigidbody)
        {
            hit.rigidbody.AddForce(-hit.normal * impactForce);
        }
        GameObject impactGo = Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impactGo, 0.5f);
    }

}
