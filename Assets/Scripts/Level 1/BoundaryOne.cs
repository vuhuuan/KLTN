using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoundaryOne : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float x1Range = -320.3f;
    [SerializeField] private float x2Range = -258f;

    [SerializeField] private float z1Range = -42f;
    [SerializeField] private float z2Range = 4.7f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        stayInMap();
    }

    void stayInMap()
    {
        Vector3 currentPosition = transform.position;

        currentPosition.x = Mathf.Clamp(currentPosition.x, x1Range, x2Range);

        currentPosition.z = Mathf.Clamp(currentPosition.z, z1Range, z2Range);

        transform.position = currentPosition;
    }

    public void setBoundX(float x1, float x2)
    {
        this.x1Range = x1;
        this.x2Range = x2;

    }
    public void setBoundZ(float z1, float z2)
    {
        this.z1Range = z1;
        this.z2Range = z2;
    }

}

