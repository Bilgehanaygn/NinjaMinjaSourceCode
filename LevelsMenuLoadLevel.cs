﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelsMenuLoadLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(string levelName) {
        SceneManager.LoadScene(levelName);
    }
    public void BackButton() {
        SceneManager.LoadScene("MainMenu");
    }

}
