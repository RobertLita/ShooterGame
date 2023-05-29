using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public enum EnemyStatus
    {
        Idle,
        Attack

    }
    public enum GameStatus
    {
      Running,
      Paused,
      Failed,
      Completed


    }



    public static GameManager instance;
    public EnemyStatus EnemiesStatus=EnemyStatus.Idle;
    public GameStatus GameState=GameStatus.Running;
    public List<GameObject> Enemies;
    public List<GameObject> NPC;
    public GameObject Player;
    public int EnemiesKilled;
    public int Cash;
    public Text EnemiesKilledText;
    public Text SplashText;
    public int TotalCash;
    public int TotalEnemies;
    public GameObject LevelCompletedPanel;
    public GameObject LevelFailedPanel;
    public GameObject LevelPausedPanel;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
   
    public void EnemyDied(GameObject obj)
    {
        Enemies.Remove(obj);
        SETENEMIES();
        SETNPC();
        EnemiesKilled++;

        if (SceneManager.GetActiveScene().ToString().Equals("Attacker"))
            EnemiesKilledText.text = "ATTACKERS KILLED: " + EnemiesKilled + "/" + TotalEnemies.ToString();
        else
            EnemiesKilledText.text = "DEFENDERS KILLED: " + EnemiesKilled + "/" + TotalEnemies.ToString();
        if (EnemiesKilled == TotalEnemies && Cash == TotalCash)
            if (EnemiesKilled == TotalEnemies && Cash == TotalCash)
        {
            LevelCompleted();
        }
    }
    public void PlayerDied()
    {
        LevelFailed();
    }
    public void NPCDied(GameObject obj)
    {
        NPC.Remove(obj);
        SETENEMIES();
        SETNPC();
    }
    public void OnCollectionCash()
    {
    
        
        if(EnemiesKilled==TotalEnemies && Cash==TotalCash)
        {
            LevelCompleted();
        }

    }
    public void LevelCompleted()

    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameState = GameStatus.Completed;
        Time.timeScale = 0;
        LevelCompletedPanel.gameObject.SetActive(true);
    }
    public void LevelFailed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameState = GameStatus.Failed;
        Time.timeScale = 0;
        LevelFailedPanel.gameObject.SetActive(true);
    }
    public void GamePause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameState = GameStatus.Paused;
        Time.timeScale = 0;
        LevelPausedPanel.gameObject.SetActive(true);
    }
    public void Retry()
    {
        Time.timeScale = 1;

     

        StartCoroutine(LoadScene("Defense"));
      
    }
    public void Home()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1;



        StartCoroutine(LoadScene("MainMenu"));
    }
    public IEnumerator LoadScene(string name)
    {
    

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void Next()
    {
        Time.timeScale = 1;
        
            SceneManager.LoadScene("Defense");
      
    }
    public void UnPause()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameState = GameStatus.Running;
        Time.timeScale = 1;
        LevelPausedPanel.gameObject.SetActive(false);
    }
    void Start()
    {
        GameObject[] Enem = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i=0;i< Enem.Length;i++)
        {
            Enemies.Add(Enem[i]);
        }
        TotalEnemies = Enemies.Count;

        GameObject[] NPs = GameObject.FindGameObjectsWithTag("NPC");
        for (int i = 0; i < NPs.Length; i++)
        {
            NPC.Add(NPs[i]);
        }


        Player = GameObject.FindGameObjectWithTag("Player");

 

    }

    public int GetNPC()
    {
        int r = Random.RandomRange(0, NPC.Count);
        return r;
    }
    public int GetEnemy()
    {
        int r = Random.RandomRange(0, Enemies.Count);
        return r;
    }
    public void SETENEMIES()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {

            int r = UnityEngine.Random.RandomRange(0, 100);
            if (r % 2 == 0)
            {
                Enemies[i].GetComponent<Enemy>().TargetTag = "Player";
                Enemies[i].GetComponent<Enemy>().CurrentTarget = Player;
            }
              
            else if (r % 3 == 0)
            {
                   Enemies[i].GetComponent<Enemy>().TargetTag = "NPC";
                if (NPC.Count != 0)
                    Enemies[i].GetComponent<Enemy>().CurrentTarget = NPC[GetNPC()];
                else
                    Enemies[i].GetComponent<Enemy>().CurrentTarget = Player;
            }
            Enemies[i].GetComponent<Enemy>().ConvertToAttackState();
        }
    }
    public void SETNPC()
    {
        for (int i = 0; i < NPC.Count; i++)
        {
            
            if(Enemies.Count!=0)
            {
                if (NPC[i].GetComponent<AIMovement>().CurrentTarget != null && NPC[i].GetComponent<HealthController>().Health > 0)
                    continue;
                else
                {
                    NPC[i].GetComponent<AIMovement>().CurrentTarget = Enemies[GetEnemy()];

                    NPC[i].GetComponent<AIMovement>().ConvertToAttackState();
                }
                   
            }

        }
    }
    public void SETSPECIFICNPC(GameObject t)
    {
        for (int i = 0; i < NPC.Count; i++)
        {

            if (Enemies.Count != 0)
            {
                NPC[i].GetComponent<AIMovement>().CurrentTarget = Enemies[GetEnemy()];

                NPC[i].GetComponent<AIMovement>().ConvertToAttackState();
            }

        }
    }
    public void RESETNPC()
    {
        for (int i = 0; i < NPC.Count; i++)
        {

            NPC[i].GetComponent<AIMovement>().CurrentTarget = Enemies[GetEnemy()];
            NPC[i].GetComponent<AIMovement>().ConvertToAttackState();
        }
    }
    // Update is called once per frame

 
void Update()
    {

        if((Input.GetKey(KeyCode.H)))
            {
            SETNPC();
         }
        if((Input.GetKey(KeyCode.H) || Input.GetMouseButtonDown(0)) && EnemiesStatus == EnemyStatus.Idle)
        {
            SoundController.instance.StartAttack.Play();
            SETENEMIES();
            SETNPC();
          
            EnemiesStatus = EnemyStatus.Attack;
        }
        if ((Input.GetKey(KeyCode.H)))
        {
            SETNPC();
        }
        if ((Input.GetKey(KeyCode.R)) && EnemiesStatus == EnemyStatus.Attack)
        {
            SoundController.instance.Retreat.Play();
            RESETNPC();
        }
        if (Input.GetKey(KeyCode.Escape) && GameState != GameStatus.Paused)
        {
          GamePause();
        }
    }
}
