using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private InputManager inputManager;
    private InputAction castAbility1;
    private InputAction castAbility2;
    private InputAction consumePotion1;
    private InputAction consumePotion2;

    private Animator animator;

    private PlayerController playerControllerScript;
    private Stats statsScript;

    [SerializeField] private Ability[] abilities = new Ability[3];
    [NonSerialized] public Ability[] spawnedAbilities = new Ability[3]; // The array length should match the abilities array's length

    private bool isCasting = false;
    public bool IsCasting
    {
        get { return isCasting; }
        set { isCasting = value; }
    }

    private int maxHealthPotionsAmount = 3;
    private int healthPotionsAmount;
    public int HealthPotionsAmount
    {
        get { return healthPotionsAmount; }
        set { healthPotionsAmount = value; }
    }
    private float healthPotionPoints = 20f;
    [NonSerialized] public float healthPotionRefillCooldown = 4f;
    [NonSerialized] public float healthPotionRefillTimer = 0f;


    private int maxManaPotionsAmount = 3;
    private int manaPotionsAmount;
    public int ManaPotionsAmount
    {
        get { return manaPotionsAmount; }
        set { manaPotionsAmount = value; }
    }

    private float manaPotionPoints = 10f;
    [NonSerialized] public float manaPotionRefillCooldown = 2f;
    [NonSerialized] public float manaPotionRefillTimer = 0f;

    private void Awake()
    {
        inputManager = new InputManager();
        playerControllerScript = GetComponent<PlayerController>();
        statsScript = GetComponent<Stats>();
        animator = GetComponent<Animator>();

        healthPotionsAmount = maxHealthPotionsAmount;
        manaPotionsAmount = maxManaPotionsAmount;
    }

    private void OnEnable()
    {
        castAbility1 = inputManager.Player.CastAbility1;
        castAbility1.Enable();
        castAbility1.performed += CastAbility1Performed;

        castAbility2 = inputManager.Player.CastAbility2;
        castAbility2.Enable();
        castAbility2.performed += CastAbility2Performed;

        consumePotion1 = inputManager.Player.ConsumePotion1;
        consumePotion1.Enable();
        consumePotion1.performed += ConsumePotion1;

        consumePotion2 = inputManager.Player.ConsumePotion2;
        consumePotion2.Enable();
        consumePotion2.performed += ConsumePotion2;
    }

    private void OnDisable()
    {
        castAbility1.Disable();
        castAbility1.performed -= CastAbility1Performed;

        castAbility2.Disable();
        castAbility2.performed -= CastAbility2Performed;

        consumePotion1.Disable();
        consumePotion1.performed -= ConsumePotion1;

        consumePotion2.Disable();
        consumePotion2.performed -= ConsumePotion2;
    }

    private void Update()
    {
        UpdateHealthPotions();
        UpdateManaPotions();
    }

    private void UpdateHealthPotions()
    {
        if (healthPotionsAmount < maxHealthPotionsAmount)
        {
            healthPotionRefillTimer += Time.deltaTime;

            if (healthPotionRefillTimer >= healthPotionRefillCooldown)
            {
                healthPotionsAmount++;
                healthPotionRefillTimer = 0;
            }
        }
    }

    private void UpdateManaPotions()
    {
        if (manaPotionsAmount < maxManaPotionsAmount)
        {
            manaPotionRefillTimer += Time.deltaTime;

            if (manaPotionRefillTimer >= manaPotionRefillCooldown)
            {
                manaPotionsAmount++;
                manaPotionRefillTimer = 0;
            }
        }
    }

    public void PrimaryAttack(GameObject intendedTarget)
    {
        CastAbility(0, intendedTarget, false);
    }

    private void CastAbility1Performed(InputAction.CallbackContext context)
    {
        CastAbility(1, gameObject, true);
    }

    private void CastAbility2Performed(InputAction.CallbackContext context)
    {
        CastAbility(2, gameObject, true);
    }

    private void CastAbility(int abilitySlotIndex, GameObject intendedTarget, bool stopMovement)
    {
        if (intendedTarget != null)
        {
            if (spawnedAbilities[abilitySlotIndex] == null && !isCasting)
            {
                float abiliyManaCost = abilities[abilitySlotIndex].GetManaCost();

                if (statsScript.CurrentMana >= abiliyManaCost)
                {
                    statsScript.ConsumeMana(abiliyManaCost);

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
    }

    private void ConsumePotion1(InputAction.CallbackContext context)
    {
        ConsumePotion("health");
    }

    private void ConsumePotion2(InputAction.CallbackContext context)
    {
        ConsumePotion("mana");
    }

    private void ConsumePotion(string potionType)
    {
        if (potionType == "health" && healthPotionsAmount > 0 && statsScript.CurrentHealth < statsScript.MaxHealth)
        {
            healthPotionsAmount--;
            healthPotionRefillTimer = 0;
            statsScript.RefillHealth(healthPotionPoints);
        }
        else if (potionType == "mana" && manaPotionsAmount > 0 && statsScript.CurrentMana < statsScript.MaxMana)
        {
            manaPotionsAmount--;
            manaPotionRefillTimer = 0;
            statsScript.RefillMana(manaPotionPoints);
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
