using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    // Start is called before the first frame update
    public float pushForce = 5f;
    public ParticleSystem particleEffect;

    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>().isDead) { return; }
            Instantiate(particleEffect, other.transform.position, Quaternion.identity);
            Debug.Log("Push!!!");
            isPushing = true;
            initialPushForce = pushForce;
            gameObject.GetComponent<WolfStateManager>().SwitchState(gameObject.GetComponent<WolfStateManager>().IdleState);

            if (other.GetComponent<PlayerMovement2>().isRolling) 
            {
                return;
            }

            other.GetComponent<Player>().TakeDamage(2);
            if (gameObject.GetComponent<WolfChaseState>().chaseSpeed < 12f)
            {
                gameObject.GetComponent<WolfChaseState>().chaseSpeed += 0.5f;
            }

            StartCoroutine(DamageCoolDown());
        }
    }
    void Start()
    {
        
    }
    private bool isPushing = false;
    private float pushDuration = 0.5f; 
    private float pushTimer = 0f;
    private float initialPushForce;
    private Vector3 pushDirection;

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

            if (pushTimer >= pushDuration)
            {
                isPushing = false;
                pushForce = 20f;
                pushTimer = 0f;
            }
        }

        if (canIncreaseSpeed)
        {
            canIncreaseSpeed = false;
            StartCoroutine(IncreaseSpeedCoolDown());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 5f, transform.position.z);
        }
    }

    bool canIncreaseSpeed = true;

    IEnumerator DamageCoolDown()
    {
        yield return new WaitForSeconds(4f);
    }

    IEnumerator IncreaseSpeedCoolDown()
    {
        yield return new WaitForSeconds(3f);

        if (gameObject.GetComponent<WolfChaseState>().chaseSpeed < 7f)
        {
            gameObject.GetComponent<WolfChaseState>().chaseSpeed += 0.2f;
        }
        canIncreaseSpeed = true;
    }
}
