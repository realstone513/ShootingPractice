using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public float bulletSpeed;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        gameObject.transform.Translate(bulletSpeed * Time.deltaTime * Vector2.up);
    }
}