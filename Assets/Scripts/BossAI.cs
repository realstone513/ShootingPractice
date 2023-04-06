using System.Collections;
using UnityEngine;

public class BossAI : EnemyAI
{
    protected override void OnEnable()
    {
        base.OnEnable();
        subWeaponDelayTimer = 0f;
        StartCoroutine(CoArrangeBoss());
    }

    protected override void Update()
    {
        base.Update();
        ShootSubWeapon();
    }

    private IEnumerator CoArrangeBoss()
    {
        Vector2 initPos = GameManager.Instance.bossInitPos;
        while (gameObject.transform.position.y > initPos.y)
        {
            gameObject.transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
            yield return null;
        }
    }
}