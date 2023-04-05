using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Instruction;
    public GameObject BackButton;
    public GameObject Buttons;
    public GameObject Credits;

    private void Start()
    {
        Instruction.SetActive(false);
        BackButton.SetActive(false);
        Credits.SetActive(false);
        Buttons.SetActive(true);
    }

    public void Instructions()
    {
        Instruction.SetActive(true);
        Buttons.SetActive(false);
        BackButton.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene("Level");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Credit()
    {
        Credits.SetActive(true);
        Buttons.SetActive(false);
        BackButton.SetActive(true);
    }

    public void Back()
    {
        Credits.SetActive(false);
        Instruction.SetActive(false);
        BackButton.SetActive(false);
        Buttons.SetActive(true);
    }
}
