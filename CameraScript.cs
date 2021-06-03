using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FixCameraSize();
    }

    public void FixCameraSize() {
            
        Camera.main.orthographicSize = (((float)Screen.height) / 1080.0f) * (10800 / ((float)Screen.width));
        
    }

}
