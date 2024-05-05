using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRing : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Wolf;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) {
            if (Vector3.Distance(other.transform.position, transform.position) >= 11 )
            {
                other.GetComponent<Player>().TakeDamage(10);
                Wolf.GetComponent<WolfBoss>().SetUpHealthBar();
            }
        }
    }

}
