using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityRage : Ability
{
    protected override void Setup()
    {
        abilityName = "Rage";
        castDuration = animationClip.length;
        buffDuration = 10f;
        maxCooldown = 20f;

        base.Setup();
    }

    protected override void ActivateAbility()
    {
        base.ActivateAbility();

        transform.SetParent(caster.transform);
        caster.GetComponent<Animator>().SetTrigger("rage");
        BuffPlayer();
    }

    protected override void Debuff()
    {
        base.Debuff();
        DebuffPlayer();
    }

    private void BuffPlayer()
    {
        caster.TryGetComponent<Stats>(out Stats casterStats);
        casterStats.Strength *= 2;
        casterStats.CurrentHealth = casterStats.MaxHealth;
    }

    private void DebuffPlayer()
    {
        caster.TryGetComponent<Stats>(out Stats casterStats);
        casterStats.Strength /= 2;
    }
}
