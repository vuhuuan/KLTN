using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damageAmount = 1;

    [SerializeField] private GameObject player;

    void Update()
    {
        CheckForEnemies();
    }

    void CheckForEnemies()
    {
        Vector3 boxSize = GetComponent<BoxCollider>().size;

        Vector3 localScale = transform.localScale;

        Vector3 scaledBoxSize = new Vector3(boxSize.x * localScale.x, boxSize.y * localScale.y, boxSize.z * localScale.z);

        Collider[] colliders = Physics.OverlapBox(transform.position, scaledBoxSize / 2f);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                //Debug.Log(boxSize);

                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damageAmount);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
