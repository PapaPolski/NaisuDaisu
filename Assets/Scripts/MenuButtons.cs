using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject optionsMenu;

    private void Start()
    {
        optionsMenu.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            if(!optionsMenu.activeInHierarchy)
            {
                StartGame();
            }
            else
            {
                CloseOptions();
            }
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            OptionsMenu();
        }

        if(Input.GetKeyDown(KeyCode.Q) && !optionsMenu.activeInHierarchy)
        {
            Quit();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OptionsMenu()
    {
        optionsMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
    }
}
