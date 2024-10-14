using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    [SerializeField] private int abilitySlotNumber;
    [SerializeField] private Image cooldownBar;

    private PlayerCombat playerCombatScript;
    [SerializeField] private GameObject player;

    private void Start()
    {
        playerCombatScript = player.GetComponent<PlayerCombat>();
        playerCombatScript.GetAbilityIcon(abilitySlotNumber, out Sprite abilityIcon);

        if (abilityIcon != null)
        {
            GetComponent<Image>().sprite = abilityIcon;
        }
    }

    private void Update()
    {
        playerCombatScript.GetAbilityCooldown(abilitySlotNumber, out float currentCooldown, out float maxCooldown);

        if (currentCooldown > 0)
        {
            cooldownBar.fillAmount = currentCooldown / maxCooldown;
        }
        else
        {
            cooldownBar.fillAmount = 0;
        }
    }
}
