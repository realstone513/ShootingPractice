using UnityEngine;

public class PlayerWeapons : Aircraft
{
    public float DPS { get => mainWeapon.damage / mainWeapon.reloadDelay + subWeapon.damage / subWeapon.reloadDelay * subShootTransform.Length; }

    protected override void OnEnable()
    {
        base.OnEnable();
        subWeaponDelayTimer = 0f;
    }

    protected virtual void Update()
    {
        ShootMainWeapon();
        ShootSubWeapon();
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log($"DPS : {DPS}");
        }
    }

    public override void ShootMainWeapon()
    {
        if (mainWeapon == null)
            return;

        mainWeaponDelayTimer += Time.deltaTime;
        if (mainWeaponDelayTimer > mainWeapon.reloadDelay)
        {
            mainWeaponDelayTimer = 0f;
            string mainWeaponName = mainWeapon.bulletPrefab.name;
            GameObject bulletObj = gm.GetBullet(mainWeaponName);
            bulletObj.transform.position = mainShootTransform.position;
        }
    }

    public override void ShootSubWeapon()
    {
        if (subWeapon == null)
            return;

        subWeaponDelayTimer += Time.deltaTime;
        if (subWeaponDelayTimer > subWeapon.reloadDelay)
        {
            subWeaponDelayTimer = 0f;
            int count = subShootTransform.Length;
            for (int i = 0; i < count; i++)
            {
                string subWeaponName = subWeapon.bulletPrefab.name;
                GameObject bulletObj = gm.GetBullet(subWeaponName);
                bulletObj.transform.position = subShootTransform[i].position;
            }
        }
    }
}