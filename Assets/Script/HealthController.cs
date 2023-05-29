using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum HealthType
{
    Player,
    NPC,
    Enemy
}
public class HealthController : MonoBehaviour
{

    public float Health = 100;
     float initHealth;
    public HealthType CharacterType;
    private Animator anim;
    public Image HealthBar;
    public int healthfactor = 1;
    private void Awake()
    {
        initHealth = Health;
    }
    public void ReduceHealth(float reduction,GameObject Originiated)
    {
        if(CharacterType==HealthType.Player)
        {
            int Diff=PlayerPrefs.GetInt("DIFFICULTY", 1);
            if(Diff==0)
            {
                reduction *=0.85f;
            }
            else if(Diff==1)
            {
                reduction *= 3.2f;
            }
            else if(Diff==2)
            {
                reduction *= 5.2f;
            }
        }
        Health -= reduction;
        
        
        Health = Mathf.Clamp(Health,0, initHealth);

        if (HealthBar == null)
            Debug.LogError(name);

        HealthBar.fillAmount = Health / initHealth;

        if(CharacterType==HealthType.Enemy)
        {
            GameManager.instance.SplashText.text = "Enemy Has Been Hit";
        }
        if (CharacterType == HealthType.NPC)
        {
            GameManager.instance.SplashText.text = "NPC Has Been Hit";
                        
        }


        if (Health <= 0)
        {
            this.GetComponent<BoxCollider>().enabled = false;
            if (CharacterType == HealthType.NPC || CharacterType == HealthType.Enemy)
            {
                if (CharacterType == HealthType.Enemy && this.GetComponent<Enemy>().isAlreadyDead==false)
                {
                    this.GetComponent<Enemy>().isAlreadyDead = true;
                    this.GetComponent<Enemy>().enabled = false;
                    this.GetComponent<WanderingAI>().enabled = false;
                    this.GetComponent<NavMeshAgent>().isStopped = true;
                    this.GetComponent<NavMeshAgent>().speed = 0;
                    Destroy(this.GetComponent<Enemy>().BulletInstantiatePosition.transform.GetChild(0).gameObject);
                    GameManager.instance.EnemyDied(this.gameObject);
                    anim.SetTrigger("DeathTrigger");
                   
                    this.gameObject.tag ="Untagged";
                    GameManager.instance.SplashText.text = "Enemy has Died";
                }
               
                if (CharacterType == HealthType.NPC)
                {
                    this.GetComponent<AIMovement>().enabled = false;
                    anim.SetTrigger("DeathTrigger");
                    GameManager.instance.NPCDied(Originiated);
                    this.gameObject.tag = "Untagged";
                    GameManager.instance.SplashText.text = "NPC has died";
                    Destroy(this.GetComponent<AIMovement>().BulletInstantiatePosition.transform.GetChild(0).gameObject);
                }
            }
            if (CharacterType == HealthType.Player)
            {
                GameManager.instance.PlayerDied();
            }
        }
    }
    void Start()
    {
        if(this.GetComponent<Animator>()!=null)
        {
            anim = this.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
