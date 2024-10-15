using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject barContainer;

    private void Update()
    {
        barContainer.transform.localScale = new Vector3(GetComponentInParent<Stats>().GetHealthPercentage(), 1, 1);
        transform.LookAt(Camera.main.transform);

        if (GetComponentInParent<Stats>().CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
