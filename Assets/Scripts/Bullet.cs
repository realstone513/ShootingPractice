using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float damage;
    public Vector2 direction;
    private IObjectPool<GameObject> bulletPool;

    private void Start()
    {
        direction = direction.normalized;
    }

    private void Update()
    {
        gameObject.transform.Translate(bulletSpeed * Time.deltaTime * direction);
    }

    public void SetPool(IObjectPool<GameObject> pool)
    {
        bulletPool = pool;
    }

    private void Release()
    {
        bulletPool.Release(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isPlayerBullet = true;
        if (CompareTag("EnemyBullet"))
            isPlayerBullet = false;

        if (isPlayerBullet)
        {
            if (collision.CompareTag("Enemy"))
            {
                Debug.Log($"{collision.name} hit {damage}");
                Release();
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                Debug.Log($"{collision.name} hit {damage}");
                Release();
            }
        }
    }
}