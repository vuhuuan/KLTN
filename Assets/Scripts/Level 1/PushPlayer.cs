using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPlayer : MonoBehaviour
{
    public float pushForce = 5f;
    public GameObject player;

    private bool isPushing = false;

    public float pushDuration = 0.5f;
    private float pushTimer = 0f;
    private float initialPushForce;
    private Vector3 pushDirection;

    private void Awake()
    {
        player = GameObject.Find("Mick3 Player");

        initialPushForce = pushForce;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Push!!!");
            isPushing = true;
            player.GetComponent<Player>().TakeDamage(10);
        }
    }
    private void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Mick3 Player(Clone)");
        }
    }
    private void FixedUpdate()
    {
        if (isPushing)
        {
            pushDirection = (player.transform.position - transform.position).normalized;

            if (Mathf.Abs(pushDirection.y) > 0.5f)
            {
                pushDirection = player.transform.forward;
            }

            pushDirection.y = 0;

            player.GetComponent<CharacterController>().Move(pushDirection * pushForce * Time.deltaTime);

            pushForce = Mathf.Lerp(initialPushForce, 0f, pushTimer / pushDuration);

            pushTimer += Time.fixedDeltaTime;

            if (pushTimer > pushDuration)
            {
                isPushing = false;
                pushForce = initialPushForce;
                pushTimer = 0f;
            }
        }
    }

    public void EnemyAddPush()
    {
        pushForce += 2f;
    }

    public void EnemyResetPush()
    {
        pushForce = 4f;
    }
}
