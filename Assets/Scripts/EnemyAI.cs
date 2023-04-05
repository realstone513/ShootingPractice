using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Vector2 direction = Vector2.down;
    public float speed = 3f;
    public List<GameObject> weapons;

    private void Update()
    {
        if (gameObject.CompareTag("Enemy"))
            gameObject.transform.Translate(speed * Time.deltaTime * direction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border") && collision.name.Equals("Bottom"))
        {
            Debug.Log(collision.name);
            GameManager.Instance.DestroyAircraft(gameObject);
        }
    }
}