using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour 
{
    public void RestartSimulation()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }
}
