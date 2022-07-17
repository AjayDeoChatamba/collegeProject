using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    [SerializeField] GameObject lvlEndMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        lvlEndMenu.SetActive(false);
    }
  

    public void NextLvl()
    {
        AudioManager.instance.Play("button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void Restart()
    {
        AudioManager.instance.Play("button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


   public void  TurnOnPanel()   //lvl end panel
    {
        lvlEndMenu.SetActive(true);
    }

    public void ToMainMenu()
    {
        AudioManager.instance.Play("button");
        SceneManager.LoadScene("MainMenu");  
    }

   
}
