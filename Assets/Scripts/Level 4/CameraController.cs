using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject topDownCamera;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            topDownCamera.SetActive(!topDownCamera.activeSelf);
        }
    }
}
