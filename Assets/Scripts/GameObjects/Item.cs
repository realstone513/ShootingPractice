using UnityEngine;

public class Item : MonoBehaviour
{
    public float speed;
    public Vector2 moveDirection;
    private GameManager gm;

    private void OnEnable()
    {
        moveDirection = moveDirection.normalized;
        gm = GameManager.Instance;
    }

    private void Update()
    {
        gameObject.transform.Translate(speed * Time.deltaTime * moveDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            Destroy(gameObject);
            gm.UseItem(name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Border") && collision.name.Equals("Bottom"))
        {
            Destroy(gameObject);
        }
    }
}