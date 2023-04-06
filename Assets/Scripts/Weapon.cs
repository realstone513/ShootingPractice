using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject bulletPrefab;
    [Range (0f, 10f)]
    public float reloadDelay;
    public float damage;
    public float bulletSpeed;
    [Range (1, 100)]
    public int magazineSize = 1;
    public Vector2 shootDirection;
    public bool isGuidedMissile = false;

    public void SetBulletData(Bullet bullet)
    {
        bullet.SetFromWeaponSetting(bulletSpeed, damage, shootDirection);
    }
}