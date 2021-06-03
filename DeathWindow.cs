using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathWindow : MonoBehaviour
{
    Canvas myCanvas;
    // Start is called before the first frame update
    void Start()
    {
        myCanvas = GetComponent<Canvas>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (myCanvas.worldCamera != Camera.main) {
            myCanvas.worldCamera = Camera.main;
        }
    }

    public void RetryLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1;
        GameManager.totalKill = 0;

    }

    public void Home() {
        SceneManager.LoadScene("Levels");
        transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1;
        GameManager.totalKill = 0;
    }

    public void WatchAdd() { 
        
    }

    

}
