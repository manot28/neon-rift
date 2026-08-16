using UnityEngine;
using UnityEngine.SceneManagement;
public class WaitLoadScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(Load), 6f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Load()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
