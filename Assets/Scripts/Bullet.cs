using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float damage;
    public Vector2 direction;
    private float lifeTime = 5f;
    private float duration = 0f;
    public int id = 0;

    private void Start()
    {
        direction = direction.normalized;
    }

    private void OnEnable()
    {
        duration = 0f;
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