using UnityEngine;

public class PauseMenu : MonoBehaviour 
{
    // source: https://answers.unity.com/questions/1230216/a-proper-way-to-pause-a-game.html
    //[SerializeField]
    private GameObject menuPanel;
    private GameObject helpPanel;
    private GameObject aboutPanel;

    void Start () 
	{
        menuPanel = GameObject.FindGameObjectWithTag("Menu");
        menuPanel.SetActive(false);

        helpPanel = GameObject.FindGameObjectWithTag("HelpMenu");
        helpPanel.SetActive(false);

        aboutPanel = GameObject.FindGameObjectWithTag("AboutMenu");
        aboutPanel.SetActive(false);
    }
	
	void Update () 
	{
        if (Input.GetButtonDown("Menu"))
        {
            // Exit from Help panel
            if (helpPanel.activeInHierarchy)
            {
                helpPanel.SetActive(false);
                menuPanel.SetActive(true);
            }
            // Exit from About panel
            else if (aboutPanel.activeInHierarchy)
            {
                aboutPanel.SetActive(false);
                menuPanel.SetActive(true);
            }
            // Menu-button was pressed without other panels being active. 
            else
            {
                SetMenuPanelState();
            }
        }
	}

    private void SetMenuPanelState()
    {
        // Esc was pressed while menu was active => exit menu.
        if (menuPanel.activeInHierarchy)
            ContinueSimulation();

        // Esc was pressed while menu was not active => activate menu.
        else if (!menuPanel.activeInHierarchy)
            PauseSimulation();
    }

    /// <summary>
    /// Pause simulation and set menu as active.
    /// </summary>
    public void PauseSimulation()
    {
        Time.timeScale = 0;
        menuPanel.SetActive(true);
    }

    /// <summary>
    /// Continue simulation and deactivate menu.
    /// </summary>
    public void ContinueSimulation()
    {
        Time.timeScale = 1;
        menuPanel.SetActive(false);
    }

}
