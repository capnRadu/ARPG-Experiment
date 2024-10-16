using UnityEngine;
using UnityEngine.UI;

public class PlayerResource : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Stats playerStats;

    [SerializeField] private string resourceType;
    [SerializeField] private Image fillImage;

    private void Start()
    {
        playerStats = player.GetComponent<Stats>();
    }

    private void Update()
    {
        if (playerStats != null)
        {
            UpdateResource();
        }
    }

    private void UpdateResource()
    {
        switch (resourceType)
        {
            case "health":
                fillImage.fillAmount = playerStats.GetHealthPercentage();
                break;
            case "mana":
                fillImage.fillAmount = playerStats.GetManaPercentage();
                break;
            default:
                break;
        }
    }
}
