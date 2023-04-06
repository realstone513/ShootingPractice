using System.Collections;
using UnityEngine;

public class EnemyAI : Aircraft
{
    public Vector2 moveDirection = Vector2.down;
    public float moveSpeed = 3f;

    protected virtual void Update()
    {
        ShootMainWeapon();
        ShootSubWeapon();
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

    public override void GetDamage(float Damage)
    {
        base.GetDamage(Damage);
        if (curHp < 0f)
        {
            if (gameObject.CompareTag("Enemy"))
            {
                gm.TranslateScore(value);
                gm.UseEffect("Explosion", gameObject.transform.position, 0.5f);
                gm.DestroyEnemyAircraft(gameObject, true);
            }
            StopAllCoroutines();
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
            if (mainWeapon.magazineSize == 1)
                MainSingleShot();
            else
                StartCoroutine(CoMainOpenFire());
        }
    }

    protected virtual void MainSingleShot(Vector2? direction = null, bool customDirection = false, bool isGuided = false)
    {
        GameObject bulletObj = gm.GetBullet(mainWeapon.bulletPrefab.name);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        mainWeapon.SetBulletData(bullet);
        if (mainWeapon.isGuidedMissile || isGuided)
        {
            GameObject target = GameManager.Instance.player;
            Vector2 fixDir = customDirection ? (Vector2)direction : target.transform.position - mainShootTransform.position;
            bullet.SetDirection(fixDir);
            bullet.transform.Rotate(fixDir);
        }
        bulletObj.transform.position = mainShootTransform.position;
    }

    protected virtual IEnumerator CoMainOpenFire()
    {
        int count = mainWeapon.magazineSize;
        WaitForSeconds wfs = new(mainWeapon.fireRate);
        for (int i = 0; i < count; i++)
        {
            MainSingleShot();
            yield return mainWeapon.fireRate == 0 ? null : wfs;
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
                if (subWeapon.magazineSize == 1)
                    SubSingleShot(i);
                else
                    StartCoroutine(CoSubOpenFire(i));
            }
        }
    }

    protected virtual void SubSingleShot(int index, Vector2? direction = null, bool customDirection = false, bool isGuided = false)
    {
        GameObject bulletObj = gm.GetBullet(subWeapon.bulletPrefab.name);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        subWeapon.SetBulletData(bullet);
        if (subWeapon.isGuidedMissile)
        {
            GameObject target = GameManager.Instance.player;
            Vector2 fixDir = customDirection ? (Vector2)direction : target.transform.position - subShootTransform[index].position;
            bullet.SetDirection(fixDir);
            bullet.transform.Rotate(fixDir);
        }
        bulletObj.transform.position = subShootTransform[index].position;
    }

    protected virtual IEnumerator CoSubOpenFire(int index)
    {
        int count = subWeapon.magazineSize;
        WaitForSeconds wfs = new(subWeapon.fireRate);
        for (int i = 0; i < count; i++)
        {
            SubSingleShot(index);
            yield return subWeapon.fireRate == 0 ? null : wfs;
        }
    }
}