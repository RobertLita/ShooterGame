using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class UICONTROLLER : MonoBehaviour
{
    // Start is called before the first frame update
    public int difficulty;
    public AudioSource BtnSound;
    void Start()
    {
        
    }
    public void SelectLevel(int Difficulty)
    {
        difficulty = Difficulty;
    }
    public void BUTTONSOUNDPLAY()
    {
        BtnSound.Play();
    }
    public void LoadLevel()
    {
        PlayerPrefs.SetInt("DIFFICULTY", difficulty);
        SceneManager.LoadScene("Defense");
    }
    public void Q()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
