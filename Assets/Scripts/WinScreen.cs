using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public GameObject quitButton;

    private void Start()
    {
        quitButton.SetActive(false);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void fakeQuit()
    {
        quitButton.SetActive(true);
    }
}
