using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum AIState
{
    Idle,
    Walk,
    Attack,
    Dead

}
public class AIMovement : MonoBehaviour
{

    // Member variables
    public int health;
    //Bullet Power
    public int BulletPower;
    //Bullet
    public GameObject Bullet;

    public AIState CurrentStateOfAI;
    public GameObject CurrentTarget;

    public float TargetFollowSpeed;
    public float shootingdistance;
    public float CoolDownBullet;
    private float ShootColldown;
    private Animator anim;

    private NavMeshAgent nav;
    public GameObject Player;
    public GameObject BulletInstantiatePosition;
    public ParticleSystem MuzzleParticle;


    private float LimitDistanceFromPlayer = 10f;
    //change state to attack
    public void ConvertToAttackState()
    {
        CurrentStateOfAI = AIState.Attack;
        this.GetComponent<Animator>().SetInteger("STATE", 1);
        
    
    }
    //reset the states
    public void RESETATTACKSTATE()
    {
        CurrentStateOfAI = AIState.Walk;
        this.GetComponent<Animator>().SetInteger("STATE", 1);
    }
    //reset the state
    public void ConvertToNormalState()
    {
        CurrentStateOfAI = AIState.Walk;
        this.GetComponent<Animator>().SetInteger("STATE", 1);


    }
    void Start()
    {
        nav = this.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        anim = this.GetComponent<Animator>();
        CurrentStateOfAI = AIState.Walk;
    }
    //Rotate The enemy or NPc towards player
    public void RotateTowardsPlayer()
    {
        var lookPos = CurrentTarget.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
    }
    //Check if path is reachable
    public bool isPathAttainable()
    {
            bool pathAvailable;
            NavMeshPath navMeshPath;
            navMeshPath = new NavMeshPath();
            pathAvailable = nav.CalculatePath(CurrentTarget.transform.position, navMeshPath);
        return pathAvailable;
    }
    //Check if is enemy is in view
    public bool CheckEnemyInView()
    {
     

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(BulletInstantiatePosition.transform.position, BulletInstantiatePosition.transform.TransformDirection(Vector3.forward), out hit, 100f))
        {

            if (hit.collider.gameObject == CurrentTarget)
            {
            
                return true;
            }
                else
            {
                return false;
            }
           

            Debug.DrawRay(BulletInstantiatePosition.transform.position, BulletInstantiatePosition.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            return true;
        }
        else
        {
     
            Debug.DrawRay(BulletInstantiatePosition.transform.position, BulletInstantiatePosition.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            return false;
            //Debug.Log("Did not Hit");
        }
    }
    
    public string TargetTag;
    // Update is called once per frame
    void Update()
    {

        ShootColldown -= Time.deltaTime;


        if (CurrentStateOfAI == AIState.Attack)
        {
            BulletInstantiatePosition.transform.GetChild(0).gameObject.SetActive(true);
            // RotateTowardsTarget();
            float d = Vector3.Distance(this.transform.position, CurrentTarget.transform.position);
            
            if (d > shootingdistance && CheckEnemyInView()==false )
            {
                nav.destination = CurrentTarget.transform.position;

                nav.speed = 4.5f;

                BulletInstantiatePosition.transform.GetChild(0).gameObject.SetActive(false);
                anim.SetInteger("STATE", 2);
                RotateTowardsPlayer();
            }
            else
            {

                BulletInstantiatePosition.transform.GetChild(0).gameObject.SetActive(true);
                anim.SetInteger("STATE", 1);
                nav.speed = 0;
                RotateTowardsPlayer();
                if (ShootColldown <= 0)
                {

                    Debug.Log("NPC HAS FIRED");
                    MuzzleParticle.Play();
                    GameObject IstantiatedBullet = Instantiate(Bullet, this.transform);
                    IstantiatedBullet.name = "NPCFIRE";
                    SoundController.instance.FireSoundPlay();

                    IstantiatedBullet.transform.localPosition = BulletInstantiatePosition.transform.localPosition;
                    IstantiatedBullet.GetComponent<Bullet>().Tag = TargetTag;
                    IstantiatedBullet.GetComponent<Bullet>().reduction = BulletPower;

                  
                    IstantiatedBullet.GetComponent<Bullet>().Originiated = this.gameObject;
                    IstantiatedBullet.SetActive(true);
                    ShootColldown = CoolDownBullet;
                }



            }
        }
        else
        {
            BulletInstantiatePosition.transform.GetChild(0).gameObject.SetActive(false);
        }
        if(CurrentStateOfAI==AIState.Walk)
        {
            float d = Vector3.Distance(this.transform.position, Player.transform.position);
            if(d>LimitDistanceFromPlayer)
            {
                nav.SetDestination(Player.transform.position);
                nav.speed = TargetFollowSpeed;
                anim.SetInteger("STATE", 0);
            

            }
            else
            {
                LimitDistanceFromPlayer += 5;
                nav.speed =0;
                anim.SetInteger("STATE", -1);
            }

        }
    }
   
    private void OnDisable()
    {
        BulletInstantiatePosition.transform.GetChild(0).gameObject.SetActive(false);
    }
}
