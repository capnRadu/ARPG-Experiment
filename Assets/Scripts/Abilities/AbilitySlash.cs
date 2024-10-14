using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySlash : Ability
{
    private float impactDelay = 0.35f;

    protected override void Awake()
    {
        base.Awake();

        abilityName = "Slash";
        baseDamage = 10f;
        maxCooldown = castDuration;
        cooldown = maxCooldown;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == intendedTarget)
        {
            collision.gameObject.TryGetComponent<Stats>(out Stats targetStats);
            caster.TryGetComponent<Stats>(out Stats casterStats);

            StartCoroutine(DealDamage(targetStats, casterStats));
        }
    }

    // Deal damage when the attack animation's sword hits the target
    private IEnumerator DealDamage(Stats targetStats, Stats casterStats)
    {
        yield return new WaitForSeconds(impactDelay);

        targetStats.TakeDamage(baseDamage * casterStats.Strength);
    }
}
