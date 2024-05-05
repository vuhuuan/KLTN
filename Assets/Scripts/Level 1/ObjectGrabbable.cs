using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField] private float groundBound;

    private Rigidbody rb;
    private Transform objectGrabPointTransform;
    [SerializeField] private GameObject item_indicator;

    private bool isGrabbing;
    private BoxCollider boxCollider;

    [SerializeField] private GameObject player;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Mick3 Player(Clone)");
        }

        if (transform.position.y <  groundBound)
        {
            transform.position = new Vector3(transform.position.x, groundBound, transform.position.z);
        }
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            //float lerpSpeed = 5f;

            //Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, lerpSpeed);

            //rb.MovePosition(objectGrabPointTransform.position);

            gameObject.transform.position = objectGrabPointTransform.position;
            isGrabbing = true;
        }
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        gameObject.transform.position = objectGrabPointTransform.position;
        isGrabbing = true;

        transform.SetParent(player.transform);
        this.objectGrabPointTransform = objectGrabPointTransform;
        rb.useGravity = false;
        rb.isKinematic = true;
        item_indicator.SetActive(false);
        boxCollider.enabled = false;
        isGrabbing = true;

    }

    public void Drop()
    {
        transform.SetParent(null, true);
        objectGrabPointTransform = null;
        rb.useGravity = true;
        rb.isKinematic = false;
        StartCoroutine(show_indicator());
        boxCollider.enabled = true;
        isGrabbing = false;
    }

    IEnumerator show_indicator()
    {
        yield return new WaitForSeconds(2);
        if (isGrabbing == false) {
            item_indicator.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Arrow Indicator")
        {
            other.gameObject.SetActive(false);
        }
    }
}
