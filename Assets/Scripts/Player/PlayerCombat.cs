using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    private InputManager inputManager;
    private InputAction castAbility1;

    private PlayerController playerControllerScript;

    [SerializeField] private Ability[] abilities = new Ability[2];
    [NonSerialized] public Ability[] spawnedAbilities = new Ability[2]; // The array length should match the abilities array's length

    private bool isCasting = false;
    public bool IsCasting
    {
        get { return isCasting; }
        set { isCasting = value; }
    }

    private Animator animator;

    private void Awake()
    {
        inputManager = new InputManager();
        playerControllerScript = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        castAbility1 = inputManager.Player.CastAbility1;
        castAbility1.Enable();
        castAbility1.performed += CastAbility1Performed;
    }

    private void OnDisable()
    {
        castAbility1.Disable();
        castAbility1.performed -= CastAbility1Performed;
    }

    public void PrimaryAttack(GameObject intendedTarget)
    {
        CastAbility(0, intendedTarget, false);
    }

    private void CastAbility1Performed(InputAction.CallbackContext context)
    {
        CastAbility(1, gameObject, true);
    }

    private void CastAbility(int abilitySlotIndex, GameObject intendedTarget, bool stopMovement)
    {
        if (intendedTarget != null)
        {
            if (spawnedAbilities[abilitySlotIndex] == null && !isCasting)
            {
                if (stopMovement)
                {
                    playerControllerScript.StopRunningCoroutine(0, true);
                }

                isCasting = true;

                spawnedAbilities[abilitySlotIndex] = Instantiate(abilities[abilitySlotIndex], intendedTarget.transform.position, Quaternion.identity);
                spawnedAbilities[abilitySlotIndex].Caster = gameObject;
                spawnedAbilities[abilitySlotIndex].IntendedTarget = intendedTarget;
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
