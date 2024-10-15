using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AbilityCharge : Ability
{
    private void Awake()
    {
        abilityName = "Charge";
        baseDamage = 20f;
        castDuration = 1f;
        maxCooldown = 8f;
        cooldown = maxCooldown;
    }

    private void Start()
    {
        transform.SetParent(caster.transform);
        caster.GetComponent<Animator>().SetBool("isCharging", true);
        caster.GetComponent<NavMeshAgent>().enabled = false;

        StartCoroutine(ChargeForward());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<IDamageable>() != null && isActive)
        {
            collision.gameObject.TryGetComponent<Stats>(out Stats targetStats);
            caster.TryGetComponent<Stats>(out Stats casterStats);

            targetStats.TakeDamage(baseDamage * casterStats.Strength);
        }
    }

    private IEnumerator ChargeForward()
    {
        yield return new WaitForSeconds(0.2f);

        while (true)
        {
            caster.transform.position += 20 * Time.deltaTime * caster.transform.forward;
            yield return null;
        }
    }

    protected override void DisableAbility()
    {
        base.DisableAbility();
        StopAllCoroutines();
        caster.GetComponent<Animator>().SetBool("isCharging", false);
        caster.GetComponent<NavMeshAgent>().enabled = true;
    }
}
