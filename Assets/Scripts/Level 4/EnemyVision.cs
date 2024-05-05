using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyVision : MonoBehaviour
{
    // Start is called before the first frame update

    [Range(1f, 100f)]
    public float distance;

    [Range(1f, 180f)]
    public float angle;

    public Transform player;

    [SerializeField] LayerMask target;

    [SerializeField] LayerMask obstacle;

    private float delayTime = 0.2f;

    public bool playerInVision;

    private void Start()
    {
        StartCoroutine(VisionDelay());
    }

    IEnumerator VisionDelay ()
    {
        WaitForSeconds delay = new WaitForSeconds(delayTime);

        while (true)
        {
            yield return delay;
            VisionScan();
        }
    }

    void VisionScan()
    {
        Collider[] targetsInViewDistance = Physics.OverlapSphere(transform.position, distance, target);

        if (targetsInViewDistance.Length > 0)
        {
            foreach (Collider target in targetsInViewDistance)
            {
                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle/2)
                {
                    float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacle))
                    {
                        playerInVision = true;
                    } else
                    {
                        playerInVision = false;
                    }
                }
                else
                {
                    playerInVision = false;
                }
            }
        }
    }
}
