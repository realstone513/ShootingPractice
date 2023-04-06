using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject bulletPrefab;
    [Range (0f, 10f)]
    public float reloadDelay;
    public float damage;
    public float bulletSpeed;
    [Header("When 1 for single shot, more than 1 for continuous fire"), Range (1, 100)]
    public int magazineSize = 1;
    [Header("When 0, Fire a bullet every frame"), Range (0f, 1f)]
    public float fireRate = 0.2f;
    public Vector2 shootDirection;
    public bool isGuidedMissile = false;

    public void SetBulletData(Bullet bullet)
    {
        bullet.SetFromWeaponSetting(bulletSpeed, damage, shootDirection);
    }
}