using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AbilityCharge : Ability
{
    protected override void Setup()
    {
        abilityName = "Charge";
        baseDamage = 20f;
        castDuration = 1f;
        maxCooldown = 8f;

        base.Setup();
    }

    protected override void ActivateAbility()
    {
        base.ActivateAbility();

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
