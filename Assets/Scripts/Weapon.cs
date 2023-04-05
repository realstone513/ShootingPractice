using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject bulletPrefab;
    [Range (0f, 5f)]
    public float reloadDelay;
    public float damage;
    public float bulletSpeed;
    public Vector2 shootDirection;
    public bool isGuidedMissile = false;

    public void Init()
    {
        if (bulletPrefab == null)
            return;

        Bullet bullet = bulletPrefab.GetComponent<Bullet>();
        bullet.SetFromWeaponSetting(bulletSpeed, damage, shootDirection, isGuidedMissile);
    }
}