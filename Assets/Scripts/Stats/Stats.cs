using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
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
        Destroy(gameObject);
    }

    private void ShowFloatingText(float damage)
    {
        GameObject text = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform);
        text.GetComponent<TextMesh>().text = damage.ToString();
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
