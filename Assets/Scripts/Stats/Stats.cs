using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    [SerializeField] private float maxMana;
    private float currentMana;
    public float CurrentMana
    {
        get { return currentMana; }
        set { currentMana = value; }
    }

    [SerializeField] private float strength;
    public float Strength
    {
        get { return strength; }
        set { strength = value; }
    }

    [SerializeField] private GameObject floatingTextPrefab;

    private Animator animator;
    [SerializeField] private AnimationClip deathAnimation;
    private float deathAnimationLength;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        deathAnimationLength = deathAnimation.length;

        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name}'s current health: {currentHealth}");
        
        if (floatingTextPrefab != null)
        {
            ShowFloatingText(damage);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        DisableObject();
        animator.SetTrigger("death");
        Destroy(gameObject, deathAnimationLength + 2f);
    }

    private void ShowFloatingText(float damage)
    {
        GameObject text = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform);
        text.GetComponent<TextMesh>().text = damage.ToString();
    }

    private void DisableObject()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        if (GetComponent<Enemy>() != null)
        {
            GetComponent<Enemy>().enabled = false;
        }

        if (GetComponent<PlayerController>() != null)
        {
            GetComponent<PlayerController>().StopAllCoroutines();
            GetComponent<PlayerController>().enabled = false;
            GetComponent<PlayerCombat>().enabled = false;
        }

        GetComponent<NavMeshAgent>().ResetPath();
        GetComponent<NavMeshAgent>().enabled = false;

        if (animator.GetBool("isWalking"))
        {
            animator.SetBool("isWalking", false);
        }   
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
