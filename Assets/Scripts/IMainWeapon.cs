using UnityEngine;

public interface IMainWeapon
{
    public Weapon MainWeapon { get; set; }
    public Transform MainShootTransform { get; set; }
    public float MainWeaponDelayTimer { get; set; }
    public abstract void ShootMainWeapon();
}