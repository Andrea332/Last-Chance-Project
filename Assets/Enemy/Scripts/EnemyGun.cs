using UnityEngine;

public class EnemyGun : MonoBehaviour
{

    public float Damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public int maxAmmo = 10;
    public int currentAmmo;
    public float reloadTime = 1f;
    public bool isReloading = false;
    public GameObject Player;
    public GameObject GameController;

    public Transform Bullet;
    public AudioSource GunSound;
    public ParticleSystem GunFlash;
    public GameObject ImpactEffect;
    public GameObject ImpactEffectEnemy;

    private float nextTimeToFire = 0f;

    public Animator EnemyAnim;
    public GameControllerScript GCS;

    public EnemyController EC;

    private void Start()
    {
        currentAmmo = maxAmmo;
        Player = GameObject.Find("First Person Player");
        GameController = GameObject.Find("GameController");
        GCS = GameController.GetComponent<GameControllerScript>();
    }


    // Update is called once per frame
    void Update()
    {
        if (currentAmmo <= 0)
        {
            EC.IsReloading();
            return;
        }

        if (EC.Shoot && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            EnemyShoot();
            EC.Shoot = false;
        }
    }
    void EnemyShoot()
    {
        GunFlash.Play();
        GunSound.Play();

        Ray BulletTarget = new Ray(Bullet.position, (Player.transform.position - Bullet.position));

        RaycastHit Enemyhit;

        Debug.DrawRay(Bullet.position, (Player.transform.position - Bullet.position), Color.red);

        currentAmmo--;


        if (Physics.Raycast(BulletTarget, out Enemyhit))
        {

            //Debug.Log(Enemyhit.transform.name);

            /*EnemyController enemy = hit.transform.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
            }*/

            if (Enemyhit.transform.tag == "Player")
            {

                if (GCS.health > 0)
                {
                    GCS.TakeDamage(Damage);
                }
                GameObject impactGo = Instantiate(ImpactEffectEnemy, Enemyhit.point, Quaternion.LookRotation(Enemyhit.normal));
                Destroy(impactGo, 0.5f);
            }
            else
            {
                GameObject impactGo = Instantiate(ImpactEffect, Enemyhit.point, Quaternion.LookRotation(Enemyhit.normal));
                Destroy(impactGo, 0.5f);
            }

        }

    }
}
