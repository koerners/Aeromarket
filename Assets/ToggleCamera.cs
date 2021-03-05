using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCamera : MonoBehaviour
{

    //This is Main Camera in the Scene
    Camera m_MainCamera;
    //This is the second Camera and is assigned in inspector
    public Camera m_CameraTwo;

    void Start()
    {
        //This gets the Main Camera from the Scene
        m_MainCamera = Camera.main;
        //This enables Main Camera
        m_MainCamera.enabled = true;
        //Use this to disable secondary Camera
        m_CameraTwo.enabled = false;
    }

    public void toggleCamera()
    {
        if (m_MainCamera.enabled)
        {
            //Enable the second Camera
            m_CameraTwo.enabled = true;

            //The Main first Camera is disabled
            m_MainCamera.enabled = false;
        }
        //Otherwise, if the Main Camera is not enabled, switch back to the Main Camera on a key press
        else if (!m_MainCamera.enabled)
        {
            //Disable the second camera
            m_CameraTwo.enabled = false;

            //Enable the Main Camera
            m_MainCamera.enabled = true;
        }
    }


}

