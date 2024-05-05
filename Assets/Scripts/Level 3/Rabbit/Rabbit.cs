using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Enemy
{
    private Animator animator;
    void Start()
    {
        maxHealth = 2;
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DestroyBelowTerrain();
    }

    protected override void Die()
    {
        gameObject.GetComponent<RabbitAI>().enabled = false;
        base.Die();
        animator.SetTrigger("dead");
        gameObject.transform.Find("Canvas").gameObject.SetActive(false);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Rabbit Get Hurt");
    }
}
