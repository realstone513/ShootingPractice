using System.Collections;
using UnityEngine;

public class BossAI : EnemyAI
{
    private void Start()
    {
        direction = Vector2.down;
        speed = 1f;
    }

    private void OnEnable()
    {
        StartCoroutine(CoArrangeBoss());
    }

    private IEnumerator CoArrangeBoss()
    {
        Vector2 initPos = GameManager.Instance.bossInitPos;
        while (gameObject.transform.position.y > initPos.y)
        {
            gameObject.transform.Translate(speed * Time.deltaTime * direction);
            yield return null;
        }
    }
}