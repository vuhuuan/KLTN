using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform cam;
    void Start()
    {
        if (GameObject.Find("Main Camera"))
        {
            cam = GameObject.Find("Main Camera").gameObject.transform;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
