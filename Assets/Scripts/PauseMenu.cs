using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Level");
        Time.timeScale = 1;
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
