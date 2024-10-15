using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityRage : Ability
{
    private void Awake()
    {
        abilityName = "Rage";
        castDuration = animationClip.length;
        buffDuration = 10f;
        maxCooldown = 20f;
        cooldown = maxCooldown;
    }

    private void Start()
    {
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
        casterStats.CurrentMana = casterStats.MaxMana;
    }

    private void DebuffPlayer()
    {
        caster.TryGetComponent<Stats>(out Stats casterStats);
        casterStats.Strength /= 2;
    }
}
