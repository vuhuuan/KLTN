using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpDrop2 : MonoBehaviour
{

    [SerializeField] private Transform objectGrabPointTransform;
    // Start is called before the first frame update
    private ObjectGrabbable objectGrabbable;
    [SerializeField] private ObjectGrabbable grabbingObject;

    //private ObjectGrabbable isGrabbing;
    private bool isGrabbing;

    public WolfBoss boss;

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem đối tượng va chạm có tag là "item" hay không
        if (other.CompareTag("Item"))
        {
            other.TryGetComponent(out objectGrabbable);
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            objectGrabbable = null;
        }
    }

    void Start()
    {
        isGrabbing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (objectGrabbable != null && !isGrabbing && grabbingObject == null)
            {
                PickUp();
            }
            else if (isGrabbing && grabbingObject != null)
            {
                Drop();
            }
            
        }   

        if (boss)
        {
            if (boss.Dead) Drop();
        }
    }

    public void PickUp()
    {
        Debug.Log("Pick");
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Item Pick");
        objectGrabbable.Grab(objectGrabPointTransform);
        objectGrabbable.transform.GetComponent<BoxCollider>().enabled = false;
        grabbingObject = objectGrabbable;
        isGrabbing = true;

        if (GameObject.Find("Item 1"))
        {
            GameObject.Find("Item 1").GetComponent<Image>().enabled = true;
        }
    }

    public void Drop()
    {
        StartCoroutine("FallingTime");
        grabbingObject.Drop();
        grabbingObject.transform.GetComponent<BoxCollider>().enabled = true;

        isGrabbing = false;
        grabbingObject = null;

        if (GameObject.Find("Item 1"))
        {
            GameObject.Find("Item 1").GetComponent<Image>().enabled = false;
        }
    }

    IEnumerator FallingTime()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Item Drop");
    }
}
