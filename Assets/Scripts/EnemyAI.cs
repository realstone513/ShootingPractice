using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Vector2 direction = Vector2.down;
    public float speed = 3f;

    private void Update()
    {
        if (gameObject.CompareTag("Enemy"))
            gameObject.transform.Translate(speed * Time.deltaTime * direction);
    }
}