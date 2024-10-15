using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    protected string abilityName;
    protected float baseDamage;
    protected float castDuration; // How long the ability lasts

    protected float maxCooldown; // Time it takes to be able to cast the ability again
    public float MaxCooldown
    {
        get { return maxCooldown; }
        set { maxCooldown = value; }
    }
    protected float cooldown;
    public float Cooldown
    {
        get { return cooldown; }
        set { cooldown = value; }
    }

    [SerializeField] protected AnimationClip animationClip;
    public Sprite abilityIcon;

    protected bool isActive = true;

    protected GameObject caster;
    public GameObject Caster
    {
        get { return caster; }
        set { caster = value; }
    }

    protected GameObject intendedTarget;
    public GameObject IntendedTarget
    {
        get { return intendedTarget; }
        set { intendedTarget = value; }
    }

    protected virtual void Update()
    {
        UpdateCooldown();
        UpdateCastDuration();
    }

    private void UpdateCooldown()
    {
        cooldown -= Time.deltaTime;

        if (cooldown <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateCastDuration()
    {
        castDuration -= Time.deltaTime;

        if (castDuration <= 0)
        {
            DisableAbility();
        }
    }

    protected virtual void DisableAbility()
    {
        caster.TryGetComponent<PlayerCombat>(out PlayerCombat playerCombat);
        playerCombat.IsCasting = false;
        isActive = false;
    }
}