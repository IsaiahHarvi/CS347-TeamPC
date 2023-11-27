using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    public SceneLoader sceneLoader;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (SceneManager.GetActiveScene().buildIndex == 1) 
            {
                // Load the next level based on current level index
                sceneLoader.LoadScene(SceneLoader.currentLevelIndex + 1); 
                sceneLoader.IncrementLevel();
                Debug.Log("Current Level Index + 1" + SceneLoader.currentLevelIndex + 1);
            }
            else
            {
                // Go to garage
                sceneLoader.LoadScene(1); 
                Debug.Log("going to garage");
            }
        }
    }
}
