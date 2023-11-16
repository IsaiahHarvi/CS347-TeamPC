using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Function to load a scene by its number
    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    // Function to exit the game
    public void ExitGame()
    {
        // If we are running in a standalone build of the game
        #if UNITY_STANDALONE
            // Quit the application
            Application.Quit();
        #endif

        // If we are running in the editor
        #if UNITY_EDITOR
            // Stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
