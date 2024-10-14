using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Ability[] abilities = new Ability[1];
    [NonSerialized] public Ability[] spawnedAbilities = new Ability[1]; // The array length should match the abilities array's length

    private bool isCasting = false;
    public bool IsCasting
    {
        get { return isCasting; }
        set { isCasting = value; }
    }

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PrimaryAttack(GameObject intendedTarget)
    {
        CastAbility(0, intendedTarget);
    }

    private void CastAbility(int abilitySlotIndex, GameObject intendedTarget)
    {
        if (intendedTarget != null)
        {
            if (spawnedAbilities[abilitySlotIndex] == null)
            {
                isCasting = true;

                spawnedAbilities[abilitySlotIndex] = Instantiate(abilities[abilitySlotIndex], intendedTarget.transform.position, Quaternion.identity);
                spawnedAbilities[abilitySlotIndex].Caster = gameObject;
                spawnedAbilities[abilitySlotIndex].IntendedTarget = intendedTarget;

                animator.SetTrigger("attack");
            }
        }
    }

    public void GetAbilityIcon(int abilityIndex, out Sprite abilityIcon)
    {
        abilityIcon = null;

        if (abilityIndex < abilities.Length && abilities[abilityIndex] != null)
        {
            abilityIcon = abilities[abilityIndex].abilityIcon;
        }
    }

    public void GetAbilityCooldown(int abilityIndex, out float currentCooldown, out float maxCooldown)
    {
        currentCooldown = -1;
        maxCooldown = -1;

        if (abilityIndex < abilities.Length && spawnedAbilities[abilityIndex] != null)
        {
            currentCooldown = spawnedAbilities[abilityIndex].Cooldown;
            maxCooldown = spawnedAbilities[abilityIndex].MaxCooldown;
        }
    }
}
