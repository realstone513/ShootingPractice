using UnityEngine;

public class EnemyAI : Aircraft
{
    public Vector2 moveDirection = Vector2.down;
    public float moveSpeed = 3f;

    protected virtual void Update()
    {
        ShootMainWeapon();
        if (gameObject.CompareTag("Enemy"))
            gameObject.transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Border") && collision.name.Equals("Bottom"))
        {
            gm.DestroyEnemyAircraft(gameObject);
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
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            mainWeapon.SetBulletData(bullet);
            if (mainWeapon.isGuidedMissile)
            {
                GameObject target = GameManager.Instance.player;
                bullet.SetDirection(target.transform.position - gameObject.transform.position);
            }
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
                Bullet bullet = bulletObj.GetComponent<Bullet>();
                subWeapon.SetBulletData(bullet);
                if (subWeapon.isGuidedMissile)
                {
                    GameObject target = GameManager.Instance.player;
                    bullet.SetDirection(target.transform.position - gameObject.transform.position);
                }
                bulletObj.transform.position = subShootTransform[i].position;
            }
        }
    }
}