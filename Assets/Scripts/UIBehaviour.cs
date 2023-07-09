using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class UIBehaviour : MonoBehaviour
{
    public GameObject startMenu;
    public AudioMixer mixer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseStart()
    {
        startMenu.SetActive(false);
    }

    public void Restart()
    {
        AudioManager.StartFade(mixer, "MasterVolume", 0.0f, 0);
        GameObject.Find("GlobalController").GetComponent<GlobalController>().Clear();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Debug.Log("Restart");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
