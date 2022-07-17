using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject PostCredit;

    private void Start()
    {
        PostCredit.SetActive(false);
    }
    public void PlayGame()
    {
        AudioManager.instance.Play("button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void Quit()
    {
        AudioManager.instance.Play("button");
        Application.Quit();
    }

    public void OpenPostCreditPanel()
    {
        AudioManager.instance.Play("button");
        PostCredit.SetActive(true);
    }
    public void ClosePostCredit()
    {
        AudioManager.instance.Play("button");
        PostCredit.SetActive(false);
    }
}
