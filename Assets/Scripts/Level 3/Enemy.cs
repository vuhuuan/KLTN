using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject GetHitEffect;

    [SerializeField] HealthBarController HealthBar;

    // Start is called before the first frame update
    //[HideInInspector]
    [SerializeField] protected int maxHealth;

    [HideInInspector]
    protected int currentHealth;

    public int Health
    {
        get { return currentHealth; }  
        set {  currentHealth = value;}
    }

    protected bool canTakeDame = true;


    private bool isDead = false;
    public bool Dead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    void Start()
    {
        currentHealth = maxHealth;
        HealthBar.setHealth(maxHealth);
    }
    void Update()
    {
        DestroyBelowTerrain();
    }
    public virtual void TakeDamage(int damage)
    {
        if (!canTakeDame)
        {
            return;
        }

        if (GetHitEffect)
        {
            GetHitEffect.SetActive(true);
        }

        StartCoroutine("TakeDameCoolDown");


        Debug.Log(this.name + " take damage");
        currentHealth -= damage;
        HealthBar.setHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {

        isDead = true;
        Debug.Log(this.name + " is dead");
    }

    protected void DestroyBelowTerrain()
    {
        if (gameObject.transform.position.y <= -10f)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator TakeDameCoolDown()
    {
        canTakeDame = false;
        yield return new WaitForSeconds(0.6f);
        canTakeDame = true;
        if (GetHitEffect)
        {
            GetHitEffect.SetActive(false);
        }
    }

    public void SetUpHealthBar()
    {
        currentHealth = maxHealth;
        HealthBar.setHealth(maxHealth);
    }
}
