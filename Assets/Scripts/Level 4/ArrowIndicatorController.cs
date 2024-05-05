using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicatorController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject arrowIndicator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        arrowIndicator.SetActive(true);
    }

    public void Hide()
    {
        arrowIndicator.SetActive(false);
    }
}
