using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyState
{
    Idle,
    Walk,
    Attack,
    Dead

}
public class Enemy : MonoBehaviour
{
    public GameObject RayCastPos;

    public int health;
    public int BulletPower;
    public string TargetTag;
    public GameObject Bullet;
    public bool isAlreadyDead = false;
    public EnemyState CurrentStateOfEnemy;
    public GameObject CurrentTarget;

    public float TargetFollowSpeed;
    public float shootingdistance;
    public float CoolDownBullet;
    private float ShootColldown;
    private Animator anim;
    public ParticleSystem MuzzleParticle;
    private NavMeshAgent nav;


    private float dis = 2f;
    public GameObject BulletInstantiatePosition;
    public void ConvertToAttackState()
    {
        CurrentStateOfEnemy = EnemyState.Attack;
        this.GetComponent<Animator>().SetInteger("STATE", 1);
        this.GetComponent<WanderingAI>().enabled = false;
      
    }
    void Start()
    {
        nav = this.GetComponent<NavMeshAgent>();
        CurrentTarget = GameObject.FindGameObjectWithTag("Player");
        anim = this.GetComponent<Animator>();
        nav.speed = TargetFollowSpeed;
    }
    public void RotateTowardsPlayer()
    {
        var lookPos = CurrentTarget.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
    }
    public bool CheckEnemyInView()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(BulletInstantiatePosition.transform.position, BulletInstantiatePosition.transform.TransformDirection(Vector3.forward), out hit, 500f))
        {
         
            if (hit.collider.gameObject==CurrentTarget)
            {
                Debug.DrawRay(BulletInstantiatePosition.transform.position, BulletInstantiatePosition.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                return true;
            }
            else
            {
                Debug.DrawRay(BulletInstantiatePosition.transform.position, BulletInstantiatePosition.transform.TransformDirection(Vector3.forward) * 1000, Color.white);

                return false;
            }

        return true;
        }
        else
        {
            Debug.DrawRay(BulletInstantiatePosition.transform.position, BulletInstantiatePosition.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            return false;
        }
    }

    public void ReduceHealth(float health = 30)
    {
        Debug.Log("ENEMY HEALTH DOWN");
        health--;
    }
    // Update is called once per frame
    void Update()
    {

        ShootColldown -= Time.deltaTime;
      

        if (CurrentStateOfEnemy==EnemyState.Attack)
        {
            BulletInstantiatePosition.transform.GetChild(0).gameObject.SetActive(true);
           // RotateTowardsTarget();
            float d = Vector3.Distance(this.transform.position, CurrentTarget.transform.position);
       
            if ( CheckEnemyInView()==false && d>4f)
            {

                nav.speed =3f;

                nav.destination=CurrentTarget.transform.position;

                BulletInstantiatePosition.transform.GetChild(0).gameObject.SetActive(false);
                anim.SetInteger("STATE", 2);
                RotateTowardsPlayer();
                
            }
            else 
            {

                BulletInstantiatePosition.transform.GetChild(0).gameObject.SetActive(true);
                anim.SetInteger("STATE", 1);
                 nav.speed = 0;

                dis = 24f;
          
                if (ShootColldown <= 0 )
                {
                    MuzzleParticle.Play();
                    RotateTowardsPlayer();

                    SoundController.instance.FireSoundPlay();
                    GameObject IstantiatedBullet = Instantiate(Bullet, this.transform);
                    IstantiatedBullet.transform.localPosition = BulletInstantiatePosition.transform.localPosition;
                    IstantiatedBullet.GetComponent<Bullet>().Tag = TargetTag;
                    IstantiatedBullet.GetComponent<Bullet>().Originiated = this.gameObject;
                    IstantiatedBullet.GetComponent<Bullet>().reduction = BulletPower;
                    IstantiatedBullet.SetActive(true);
                    ShootColldown = CoolDownBullet;
                }
                   


            }
        }
        else
        {
            BulletInstantiatePosition.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        nav.speed = 0;
        BulletInstantiatePosition.transform.GetChild(0).gameObject.SetActive(false);
    }
}
