using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_End_Script : MonoBehaviour
{
    Door_Animation _doorAnimation;
    bool TriggerEnter;

    void Start()
    {
        _doorAnimation = GetComponent<Door_Animation>();
    }

    void Update()
    {
        if (TriggerEnter && _doorAnimation.DoorIsOpen)
        {
            Scene s = SceneManager.GetActiveScene();
            if (s.buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadSceneAsync(s.buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) TriggerEnter = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) TriggerEnter = false;
    }
}
