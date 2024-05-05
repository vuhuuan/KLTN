using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeToBreak : MonoBehaviour
{
    public float shakeAmount = 0.02f;
    public float shakeDuration = 0.2f;
    public int shakeCountThreshold = 10;

    private Vector3 originalPosition;
    private float shakeTimer = 0;
    public int shakeCount = 0;
    private Rigidbody rb;
    private Transform player;

    [SerializeField] private Transform item;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = transform.Find("Mick3 Player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            gameObject.GetComponent<Enemy>().TakeDamage(1);
            shakeCount++;
            if (shakeCount == shakeCountThreshold)
            {
                ActivateComponent();
            }
            else
            {
                ShakeObject();
            }
        }

        if (shakeTimer > 0)
        {
            float perlinX = Mathf.PerlinNoise(Time.time * 10f, 0f) - 0.5f;
            float perlinY = Mathf.PerlinNoise(0f, Time.time * 10f) - 0.5f;
            Vector3 offset = new Vector3(perlinX, perlinY, 0f) * shakeAmount;

            transform.position += offset;
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            shakeTimer = 0f;
        }
    }

    void ShakeObject()
    {
        shakeTimer = shakeDuration;
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Shake");
    }

    void ActivateComponent()
    {
        rb.useGravity = true;
        transform.SetParent(null, true);
        StartCoroutine("AfterBreak");
    }


    IEnumerator AfterBreak()
    {
        item.gameObject.SetActive(true);
        item.SetParent(null, true);
        item.localScale = new Vector3(0.04f, 0.04f, 0.04f);

        yield return new WaitForSeconds(0.5f);
        player.SetParent(null, true);
        player.localScale = Vector3.one;
        player.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

