using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
{

    //Inizializzo una serie di variabili e liste che mi serviranno per il controllo del nemico
    public List<Rigidbody> RagDolls;    //Creo una lista di rigidbody del nemico
    public List<Collider> GunCollider;  //Creo una lista di GunCollider che saranno i collider dell'arma del nemico 
    public float health = 15f;          //Creo una variabile che definisce la vita del nemico con un tot di vita predefinita
    Animator Anim;                      //Creo una variabile per inserire l'animator dello script   
    NavMeshAgent Agent;                 //Creo una variabile per inserire l'agent del navmesh 
    
    public GameObject Player;           //Creo delle variabili di tipo GameObject per inserire quello che mi serve   
    public GameObject EnemyGun;
    public GameObject Eyes;
    public Transform Position1;         //Creo due variabili di tipo transform che mi serviranno per spostare l'arma in base all'animazione che sta facendo
    public Transform Position2;                                                                          
    public VisionCone visionCone;       //Creo una variabile di tipo VisionCone che ne assume i valori
    public GameControllerScript GCS;

    public bool EnemySawPlayer;         //Creo dei valori booleani che mi serviranno per definire in che stato è il nemico
    public bool Fight;
    public bool Searching;
    public bool Shoot;
    public bool Dead;
    public float TimerChase;            //Questo timer viene usato per definire quanto tempo il nemico mi insegue quando mi perde di vista 
    public float TimerIdle;             //Questo Timer viene usato per definire quanto tempo il nemico rimane fermo ad un Waypoint
    //public float VisibilityTimer;
    public LayerMask EnemyLayer;        //Definiamo questo LayerMask per definire se il raycast deve ignorarlo oppure no
    
    public int wpCounter;               //Questo sarà il valore che indica la posizone nella lista sottostante(watPoint)
    public List<GameObject> wayPoint = new List<GameObject>();

   
    void Start()
    {   
        //Test
        /*VisibilityIndicator = GameObject.Find("VisibilityIndicator");
        VisibilityTimer = Mathf.Clamp(VisibilityTimer,0,5);*/

        /*usiamo il void start per modificare il valore "IsKinematic" di tutti i rigidbody del nemico in modo da non intralciare le animazioni*/
        for (int i = 0; i < 13; i++)
        {
            RagDolls[i].isKinematic = true;
        }

        EnemyGun.GetComponent<Rigidbody>().isKinematic = true; //Attivo il Kinematic dell'arma sempre per evitare problemi con le animazioni
        
        //Inserisco i valori presenti negli script e cerco il player poichè in questo modo posso creare altri nemici senza dover inserire manualmente il player
        visionCone.GetComponent<VisionCone>(); 
        Anim = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("First Person Player");
        GCS = GameObject.Find("GameController").GetComponent<GameControllerScript>();
    }
 
    void Update()
    {
        //Test
        //VisibilityIndicator.GetComponent<Image>().fillAmount += VisibilityTimer / 5;

        //Richiamo la funzione per verificare continuamente la vista del nemico
        RayForEnemyWatch();

        //In base all'animazione che il nemico sta eseguendo cambio la posizione dell'arma per farla combaciare con le mani
        if (Anim.GetBool("IsIdle") || Anim.GetBool("IsRunning"))
        {
            EnemyGun.transform.SetParent(Position1, false);
        }
        if (Anim.GetBool("IsWalking") || Anim.GetBool("IsFiring") || Anim.GetBool("IsReloading"))
        {
            EnemyGun.transform.SetParent(Position2, false);
        }
        
        //Se la vita del nemico è maggiore di zero allora è vivo
        if (health > 0)
        {
            //Se il player è nel suo cono di visione e lo vede direttamente gli spara o lo insegue, se gli spara va in combattimento quindi non serve che ci veda in zona
            if (visionCone.PlayerInZone && EnemySawPlayer || Fight)
            {
                //Se il nemico mi perde di vista lo faccio cercare per 5 secondi e poi lo rimando in ronda
                if (TimerChase < Time.time && !EnemySawPlayer)
                {
                    Searching = false;
                    Fight = false;
                    EnemySawPlayer = false;
                    visionCone.PlayerInZone = false;
                }
                //Se il Player è più vicino di 30 metri e il nemico non è più in ricerca allora sparagli
                if (Distance(transform, Player.transform) < 30 && !Searching)
                {
                    Agent.enabled = false;
                    Vector3 LookAt = Player.transform.position - transform.position;
                    LookAt.y = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(LookAt), 10 * Time.deltaTime);
                    if (!Anim.GetBool("IsReloading"))
                        IsFiring();
                }
                //Se il player è più distante di 30 metri e il nemico mi sta cercando 
                else if (Distance(transform, Player.transform) >= 30f || Searching)
                {
                    if (!Anim.GetBool("IsReloading"))
                    {
                        Agent.enabled = true;
                        IsRunning();
                        Agent.SetDestination(Player.transform.position);
                        Agent.speed = 8;
                    }                     
                }                 
            }
            //Se il nemico non è in combattimento e di conseguenza non mi sta neanche cercando gli torno impostare la ronda tra i waypoint
            if (!Fight)
            {
                //Imposto un Timer per stare fermo per 3 secondi quando arriva al waypoint
                if (Time.time > TimerIdle)
                {

                    if (Distance(wayPoint[wpCounter].transform, transform) < 3f)
                    {
                        wpCounter++; //incremento counter per il waypoint successivo.
                        wpCounter = wpCounter % wayPoint.Count; //quanfo il counter arriva al numero massimo di elementi all'interno della lista, riparte da zero.
                        TimerIdle = Time.time + 3;
                    }
                    else
                    {
                        Agent.enabled = true;
                        IsWalking();
                        Agent.SetDestination(wayPoint[wpCounter].transform.position);
                        Agent.speed = 3;
                    }
                }
                else
                {
                    IsIdle();
                    Agent.enabled = false;
                }
            }
        }           
    }

    //Questo Raycast verifica che il nemico mi veda in linea d'aria
    void RayForEnemyWatch()
    {
        //Raggio di controllo con il player
        Ray EnemyWatch = new Ray(Eyes.transform.position, (Player.transform.position - Eyes.transform.position));

        //Creo Le RaycastHIt
        RaycastHit hitPlayer;

        //Disegno i raycast di debug
        Debug.DrawRay(Eyes.transform.position, (Player.transform.position - Eyes.transform.position), Color.blue);

        //Attivo il Raggio che mi controlla se il player è visto in linea d'aria ed escludo l'EnemyLayer per evitare falsi contatti
        if (Physics.Raycast(EnemyWatch, out hitPlayer, Distance(transform, Player.transform), ~EnemyLayer))
        {
            //Debug.Log(hitPlayer.collider.name);
            //Se collido con il player
            if (hitPlayer.collider.tag == "Player")
            {
                //Allora lo vedo
                EnemySawPlayer = true;
                //Se sono in combattimento
                if (Fight)
                {
                    //E lo sto cercando
                    if (Searching)
                    {
                        //Imposto il timer della ricerca a 0 e non lo cerco più visto che l'ho trovato
                        TimerChase = Time.time;
                        Searching = false;
                    }
                }
                else //se non sono in combattimento
                {
                    //Se il player è nel cono di visione del nemico
                    if (visionCone.PlayerInZone)
                    {
                        //Allora è in combattimento
                        Fight = true;
                    }
                }               
            }
            else if (Fight) //Se non collido con il player ma sono in combattimento
            {
                //Non vedo il player
                EnemySawPlayer = false;
                //Se non l osto cercando allora lo metto alla ricerca per 5 secondi
                if (!Searching)
                {
                    TimerChase = Time.time + 5;
                    Searching = true;
                }                            
            }      
            //Debug.Log("RayCast enemy update" + hitPlayer.collider.name);
        }
    }

    //Funzione semplificata del calcolo della distanza, utilizzabile più volte
    public float Distance(Transform a, Transform b)
    {
        return Vector3.Distance(a.position, b.position);
    } 

    //Cambio l'animazione in corso sul nemico attivando e disattivando le condizioni per le transizioni
    public void IsIdle()
    {
        Anim.SetBool("IsWalking", false);
        Anim.SetBool("IsRunning", false);
        Anim.SetBool("IsFiring", false);
        Anim.SetBool("IsReloading", false);
        Anim.SetBool("IsIdle", true);
    }
    public void IsWalking()
    {       
        Anim.SetBool("IsRunning", false);
        Anim.SetBool("IsFiring", false);
        Anim.SetBool("IsReloading", false);
        Anim.SetBool("IsIdle", false);
        Anim.SetBool("IsWalking", true);
    }
    public void IsRunning()
    {
        Anim.SetBool("IsWalking", false);
        Anim.SetBool("IsFiring", false);
        Anim.SetBool("IsReloading", false);
        Anim.SetBool("IsIdle", false);
        Anim.SetBool("IsRunning", true);
    }
    public void IsFiring()
    {
        Anim.SetBool("IsWalking", false);
        Anim.SetBool("IsRunning", false);
        Anim.SetBool("IsReloading", false);
        Anim.SetBool("IsIdle", false);
        Anim.SetBool("IsFiring", true);
    }
    public void IsReloading()
    {
        Anim.SetBool("IsWalking", false);
        Anim.SetBool("IsRunning", false);
        Anim.SetBool("IsFiring", false);
        Anim.SetBool("IsIdle", false);
        Anim.SetBool("IsReloading", true);
    }
    public void IsNotReloading()
    {
        Anim.SetBool("IsWalking", false);
        Anim.SetBool("IsRunning", false);
        Anim.SetBool("IsFiring", false);
        Anim.SetBool("IsIdle", false);
        Anim.SetBool("IsReloading", false);
        EnemyGun.GetComponent<EnemyGun>().currentAmmo = EnemyGun.GetComponent<EnemyGun>().maxAmmo;
    }

    //Funzione che si occupa di far prendere danno al nemico a sesconda del valore float che inseriamo quando richiamiamo la funzione
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }          
    }

    //Funzione che si richiama alla morte del personaggio
    void Die()
    {
        //Disattiviamo l'animator per non dare più animazioni al nemico e l'agent per non far muovere il nemico
        Anim.enabled = false;
        Agent.enabled = false;
        //Con un for che passa tutti i rigidbody disattiviamo i loro kinematic per attivare i ragdoll
        for (int i = 0; i < 13; i++)
        {
            RagDolls[i].isKinematic = false;          
        }
        //Disattiviamo l'arma del nemico
        EnemyGun.SetActive(false);
        if (!Dead)
        {
            Dead = true;
            GCS.EnemiesDeadCounter += 1;
        } 
        /*Test di attivazione collider dell'arma al posto di disattivarla, 
        problema: l'arma compenetradosi con il nemico schizza alle stelle e causa la disattivazione del rigisbody del personaggio per un errore*/

        /*
        for (int i = 0; i < GunCollider.Count; i++)
        {
            GunCollider[i].GetComponent<Collider>().enabled = true; 
        }
        EnemyGun.transform.SetParent(null);
        EnemyGun.GetComponent<Rigidbody>().isKinematic = false;  
        */
    }

    //Funzione richiamata nell'animazione dello sparo per specificare che l'animazione è paretita e si pu1o sparare
    public void OkShoot()
    {
        Shoot = true;
    }
}
