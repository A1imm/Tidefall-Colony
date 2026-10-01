using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenarioCompleteUI : MonoBehaviour
{
    public void RestartScenario()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}