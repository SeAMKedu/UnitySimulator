using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour 
{
    // source: https://answers.unity.com/questions/1230216/a-proper-way-to-pause-a-game.html
    //[SerializeField]
    private GameObject menuPanel;
    private GameObject helpPanel;

    void Start () 
	{
        menuPanel = GameObject.FindGameObjectWithTag("Menu");
        menuPanel.SetActive(false);

        helpPanel = GameObject.FindGameObjectWithTag("HelpMenu");
        helpPanel.SetActive(false);
    }
	
	void Update () 
	{
        if (Input.GetButtonDown("Menu"))
        {
            if (!helpPanel.activeInHierarchy)
            {
                if (menuPanel.activeInHierarchy)
                    ContinueSimulation();

                else if (!menuPanel.activeInHierarchy)
                    PauseSimulation();
            }
            else if (helpPanel.activeInHierarchy)
            {
                helpPanel.SetActive(false);
                menuPanel.SetActive(true);
            }

            
           

        }
	}

    private void PauseSimulation()
    {
        Time.timeScale = 0;
        menuPanel.SetActive(true);
    }

    private void ContinueSimulation()
    {
        Time.timeScale = 1;
        menuPanel.SetActive(false);
    }

}
