using UnityEngine;

public interface ISubWeapon
{
    public Weapon SubWeapon { get; set; }
    public Transform[] SubShootTransform { get; set; }
    public float SubWeaponDelayTimer { get; set; }
    public abstract void ShootSubWeapon();
}