using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpDrop : MonoBehaviour
{

    [SerializeField] private Transform objectGrabPointTransform;
    // Start is called before the first frame update
    private ObjectGrabbable objectGrabbable;
    private ObjectGrabbable grabbingObject;

    //private ObjectGrabbable isGrabbing;
    private bool isGrabbing;

    private bool canPick = true;
    private float coolDownPick = 0.5f;

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
        if (other.CompareTag("Item") && canPick)
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
            if (objectGrabbable != null && !isGrabbing && grabbingObject == null && canPick)
            {
                GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Item Pick");
                objectGrabbable.Grab(objectGrabPointTransform);
                objectGrabbable.transform.GetComponent<BoxCollider>().enabled = false;
                canPick = false;
                grabbingObject = objectGrabbable;
                isGrabbing = true;

                if (GameObject.Find("Item 1"))
                {
                    GameObject.Find("Item 1").GetComponent<Image>().enabled = true; ;
                }
            }
            else if (isGrabbing && grabbingObject != null)
            {
                grabbingObject.Drop();

                StartCoroutine("SetCoolDownPick");

                grabbingObject.transform.GetComponent<BoxCollider>().enabled = false;

                isGrabbing = false;
                grabbingObject = null;

                if (GameObject.Find("Item 1"))
                {
                    GameObject.Find("Item 1").GetComponent<Image>().enabled = false;
                }
            }
            
        }   
    }

    IEnumerator SetCoolDownPick()
    {
        yield return new WaitForSeconds(0.1f);

        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Item Drop");

        yield return new WaitForSeconds(coolDownPick);
        canPick = true;
    }
}
