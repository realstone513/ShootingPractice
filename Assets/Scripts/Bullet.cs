using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float damage;
    public Vector2 direction;
    private readonly float lifeTime = 5f;
    private float duration = 0f;

    public void SetFromWeaponSetting(float bulletSpeed, float damage, Vector2 direction)
    {
        this.bulletSpeed = bulletSpeed;
        this.damage = damage;
        this.direction = direction;
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
        gameObject.transform.Rotate(this.direction);
    }

    private void OnEnable()
    {
        duration = 0f;
        SetDirection(direction);
    }

    private void Update()
    {
        gameObject.transform.Translate(bulletSpeed * Time.deltaTime * direction);
        duration += Time.deltaTime;
        if (duration > lifeTime)
        {
            Release();
        }
    }

    private void Release()
    {
        GameManager.Instance.ReleaseBullet(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isPlayerBullet = true;
        if (CompareTag("Enemy"))
            isPlayerBullet = false;

        Aircraft aircraft = collision.GetComponent<Aircraft>();
        if (isPlayerBullet)
        {
            if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
            {
                aircraft.GetDamage(damage);
                Release();
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                aircraft.GetDamage(damage);
                Release();
            }
        }
    }
}