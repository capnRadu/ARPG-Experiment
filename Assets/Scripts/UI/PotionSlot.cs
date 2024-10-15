using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionSlot : MonoBehaviour
{
    [SerializeField] private string potionType;
    [SerializeField] private TextMeshProUGUI potionAmountText;
    [SerializeField] private Image cooldownBar;

    [SerializeField] private Sprite healthPotionIcon;
    [SerializeField] private Sprite manaPotionIcon;

    private PlayerCombat playerCombatScript;
    [SerializeField] private GameObject player;

    private void Start()
    {
        playerCombatScript = player.GetComponent<PlayerCombat>();

        if (potionType == "health")
        {
            GetComponent<Image>().sprite = healthPotionIcon;
        }
        else if (potionType == "mana")
        {
            GetComponent<Image>().sprite = manaPotionIcon;
        }
    }

    private void Update()
    {
        if (potionType == "health")
        {
            int healthPotionsAmount = playerCombatScript.HealthPotionsAmount;
            float healthPotionRefillTimer = playerCombatScript.healthPotionRefillTimer;

            potionAmountText.text = healthPotionsAmount.ToString();

            if (healthPotionRefillTimer > 0)
            {
                cooldownBar.fillAmount = healthPotionRefillTimer / playerCombatScript.healthPotionRefillCooldown;
            }
            else
            {
                cooldownBar.fillAmount = 0;
            }
        }
        else if (potionType == "mana")
        {
            int manaPotionsAmount = playerCombatScript.ManaPotionsAmount;
            float manaPotionRefillTimer = playerCombatScript.manaPotionRefillTimer;

            potionAmountText.text = manaPotionsAmount.ToString();

            if (manaPotionRefillTimer > 0)
            {
                cooldownBar.fillAmount = manaPotionRefillTimer / playerCombatScript.manaPotionRefillCooldown;
            }
            else
            {
                cooldownBar.fillAmount = 0;
            }
        }
    }
}
