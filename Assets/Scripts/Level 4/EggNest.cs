using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggNest : Enemy
{
    // Start is called before the first frame update

    [SerializeField] private GameObject eggPrefab;
    [SerializeField] private Transform parentTransform; 


    void Start()
    {
        eggPrefab.transform.localScale = new Vector3(3f, 3f, 3f);
        base.SetUpHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Die()
    {
        base.Die();

        int numEggs = Random.Range(0, 3); // Random number of eggs to create

        for (int i = 0; i < numEggs; i++)
        {
            Vector3 randomPosition = GetRandomPosition(); // Get random position around the current game object
            GameObject newEgg = Instantiate(eggPrefab, randomPosition, Quaternion.identity);
            // Set up newEgg if needed
        }

        Destroy(gameObject);
    }

    private Vector3 GetRandomPosition()
    {
        // Get a random direction and distance from the current game object
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(0.1f, 0.3f);

        Vector3 randomPosition = transform.position + new Vector3(randomDirection.x, 4f, randomDirection.y) * randomDistance;
        return randomPosition;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Nest Get Hit");

    }
}
