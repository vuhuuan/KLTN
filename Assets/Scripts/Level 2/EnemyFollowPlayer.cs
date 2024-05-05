using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyFollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject player;
    [SerializeField] private float chaseSpeed = 0f;
    [SerializeField] private float moveSpeed = 8f;

    [SerializeField] private float accelarate = 0.01f;
    [SerializeField] private float stopAccelarate = 0.01f;

    [SerializeField] private GameObject target;

    [SerializeField] private ShakeToBreak breakCrate;


    [SerializeField] private CinemachineFreeLook ThirdPersonCamera;

    private bool chaseMode = false;
    private bool chaseModeSet = false;
    private bool delayed = false;
    private float turnSpeed = 100f;

    private bool stopped = true;

    [SerializeField] private float canNotChangeDirectionDistance = 3f;

    private bool canTakeDamage = true;
    private float damageCooldown = 1.2f;

    [SerializeField] private TMP_Text notifyText;

    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (canTakeDamage)
            {
                Debug.Log("Crash player");
                
                Player playerScript = player.gameObject.GetComponent<Player>();

                Transform item = playerScript.transform.Find("LocketNecklace");
                if (item)
                {
                    Transform dropItem = player.gameObject.transform.Find("Pickup Range");
                    dropItem.GetComponent<PickUpDrop2>().Drop();
                    playerScript.TakeDamage(1);
                    StartCoroutine("NotifyWhenDrop");
                }
                else
                {
                    playerScript.TakeDamage(2);
                }

                canTakeDamage = false;

                StartCoroutine(ResetDamageCooldown());
            }
        }
        
        if (other.gameObject.name == "Trigger1" && breakCrate.shakeCount <= 9)
        {
            breakCrate.enabled = false;
            Player playerScript = player.gameObject.GetComponent<Player>();
            playerScript.TakeDamage(5);

            GameObject.Find("Overlay2").GetComponent<Animation>().Play("overlay-end");
            StartCoroutine("WaitForReload");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.activeSelf)
        {
            CheckBound();
            chasePlayer();
        } else
        {
            MoveToTarget();
        }

    }

    void MoveToTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0f;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Driving");

    }

    void chasePlayer()
    {
        if (!delayed)
        {
            StartCoroutine("DelayDetectPlayer");
            delayed = true;
        }

        if (chaseModeSet)
        {
            Vector3 direction = (player.transform.position - transform.position);

            float distanceToPlayer = direction.magnitude;

            if (chaseSpeed <= 1)
            {
                stopped = true;
                chaseSpeed = 5f;
            }

            ;

            if ((distanceToPlayer > canNotChangeDirectionDistance) && stopped)
            {
                canNotChangeDirectionDistance = 2f;
                stopAccelarate = 0.001f;
                direction.y = 0f;
                direction = direction.normalized;

                chaseSpeed += chaseSpeed * accelarate * accelarate;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                transform.position += direction * chaseSpeed * Time.deltaTime;
                GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Chase");
            }
            else if (chaseSpeed >= 0)
            {
                stopped = false;
                if (distanceToPlayer > canNotChangeDirectionDistance)
                {
                    stopAccelarate += 0.00015f;
                    chaseSpeed -= chaseSpeed * stopAccelarate;
                }
                else
                {
                    chaseSpeed += chaseSpeed * accelarate;
                    canNotChangeDirectionDistance -= 0.3f;
                }
                transform.Translate(Vector3.forward * chaseSpeed * Time.deltaTime);
            }
        }
    }
    
    void CheckBound()
    {
        float lRange = 40;
        float rRange = 52;

        if (transform.position.x < lRange)
        {
            transform.position = new Vector3(lRange, transform.position.y, transform.position.z);
            chaseSpeed = 1;
        }

        if (transform.position.x > rRange)
        {
            transform.position = new Vector3(rRange, transform.position.y, transform.position.z);
            chaseSpeed = 1;
        }
    }
    IEnumerator DelayDetectPlayer()
    {
        ThirdPersonCamera.LookAt = player.transform;

        yield return new WaitForSeconds(2f);

        chaseMode = true;
        if (chaseMode && !chaseModeSet)
        {
            chaseModeSet = true;
        }
    }

    private IEnumerator ResetDamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    private IEnumerator NotifyWhenDrop()
    {
        yield return new WaitForSeconds(1f);
        notifyText.GetComponent<Animation>().Play("Float text");
    }

    private IEnumerator WaitForReload()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
