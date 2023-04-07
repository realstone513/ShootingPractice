using System.Collections.Generic;
using UnityEngine;

public class BossA : BossAI
{
    protected List<Vector2> customDirections = new ();

    private void Start()
    {
        customDirections.Add(Vector2.down);
        customDirections.Add(new Vector2(-1, -2).normalized);
        customDirections.Add(new Vector2(1, -2).normalized);
        customDirections.Add(new Vector2(-3, -2).normalized);
        customDirections.Add(new Vector2(3, -2).normalized);
    }

    protected override void ChangePhase(int nextPhase)
    {
        phase = nextPhase;
        mainWeapon = otherWeapons[0];
    }

    public override void ShootMainWeapon()
    {
        if (phase == 0)
            base.ShootMainWeapon();
        else if (phase == 1)
        {
            if (mainWeapon == null)
                return;

            mainWeaponDelayTimer += Time.deltaTime;
            if (mainWeaponDelayTimer > mainWeapon.reloadDelay)
            {
                mainWeaponDelayTimer = 0f;
                int count = customDirections.Count;
                GameObject target = GameManager.Instance.player;
                for (int i = 0; i < count; i++)
                {
                    Vector2 fixDir = target.transform.position - mainShootTransform.position;
                    MainSingleShot(customDirections[i] + fixDir, true, false);
                }
            }
        }
    }
}