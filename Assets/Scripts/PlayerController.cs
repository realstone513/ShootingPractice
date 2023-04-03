using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Direction
{
    Left, Right, Top, Bottom, Count
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Dictionary<Direction, bool> borderFlags = new();

    private void Start()
    {
        Direction count = Direction.Count;
        for (Direction i = 0; i < count; i++)
            borderFlags[i] = false;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        h = borderFlags[Direction.Left] && h < 0 || borderFlags[Direction.Right] && h > 0 ? 0 : h;

        float v = Input.GetAxisRaw("Vertical");
        v = borderFlags[Direction.Bottom] && v < 0 || borderFlags[Direction.Top] && v > 0 ? 0 : v;

        Vector2 nextPos = speed * Time.deltaTime * new Vector2(h, v);
        transform.Translate(nextPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            switch (collision.name)
            {
                case "Left":
                    borderFlags[Direction.Left] = true;
                    break;

                case "Right":
                    borderFlags[Direction.Right] = true;
                    break;

                case "Top":
                    borderFlags[Direction.Top] = true;
                    break;

                case "Bottom":
                    borderFlags[Direction.Bottom] = true;
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            switch (collision.name)
            {
                case "Left":
                    borderFlags[Direction.Left] = false;
                    break;

                case "Right":
                    borderFlags[Direction.Right] = false;
                    break;

                case "Top":
                    borderFlags[Direction.Top] = false;
                    break;

                case "Bottom":
                    borderFlags[Direction.Bottom] = false;
                    break;
            }
        }
    }
}