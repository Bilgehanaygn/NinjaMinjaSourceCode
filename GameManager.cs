using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static int health = 4;
    public static int totalKill;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this) {
            Debug.Log("garip");
            Destroy(gameObject);
        }
        transform.GetChild(0).gameObject.SetActive(false);
    }

    

    // Update is called once per frame
    void Update()
    {
        NextLevelLoad();
    }

    public void GameOver() {
        health--;
        transform.GetChild(0).gameObject.SetActive(true);
        Time.timeScale = 0;
    }


    public void NextLevelLoad() {
        if (SceneManager.GetActiveScene().name == "Level1") {
            if (totalKill >= 1)
            {
                totalKill = 0;
                StartCoroutine(WaitBeforeLoad(3.0f, "Level2"));
            }
        }
        else if (SceneManager.GetActiveScene().name == "Level2") {
            if (totalKill >= 3) {
                totalKill = 0;
                StartCoroutine(WaitBeforeLoad(3.0f, "Level3"));
            }
        }
    }

    IEnumerator WaitBeforeLoad(float givenTime, string nextLevelName) {
        yield return new WaitForSeconds(givenTime);
        SceneManager.LoadScene(nextLevelName);
        
    }

    
}
