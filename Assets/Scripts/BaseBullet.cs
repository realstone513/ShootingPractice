using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public float bulletSpeed;
    public float damage;
    public float holdingTime = 5f;

    private void Start()
    {
        Destroy(gameObject, holdingTime);
    }

    private void Update()
    {
        gameObject.transform.Translate(bulletSpeed * Time.deltaTime * Vector2.up);
    }
}