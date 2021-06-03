using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonSettings : MonoBehaviour
{
    private bool soundOn;
    // Start is called before the first frame update
    void Start()
    {
        soundOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame() {
        Time.timeScale = 0;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void MusicOn() {
        //set music to off
        if (soundOn == true)
        {
            transform.GetChild(0).GetChild(4).GetChild(0).gameObject.SetActive(true);
            soundOn = false;
        }
        else {
            transform.GetChild(0).GetChild(4).GetChild(0).gameObject.SetActive(false);
            soundOn = true;
        }
        
    }

    public void WatchVideo() { 
        
    }

    public void GoHome() {
        transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("Levels");
        GameManager.totalKill = 0;
    }

    public void RestartLevel() {
        transform.GetChild(0).gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        GameManager.totalKill = 0;
    }

}
