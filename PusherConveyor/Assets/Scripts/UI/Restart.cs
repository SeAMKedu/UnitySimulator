using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour 
{
    public void RestartSimulation()
    {
        // loads current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}
