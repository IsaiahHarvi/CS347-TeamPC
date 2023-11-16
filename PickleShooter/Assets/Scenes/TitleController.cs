using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    void Update()
    {
        // Check for a specific key press to load the scene
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene("ShootingRange");
        }
    }
}
