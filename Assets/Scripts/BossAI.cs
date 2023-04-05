using System.Collections;
using UnityEngine;

public class BossAI : EnemyAI, ISubWeapon
{
    [SerializeField]
    private Weapon subWeapon;
    [SerializeField]
    private Transform[] subShootTransform;
    private float subWeaponDelayTimer;

    public Weapon SubWeapon { get => subWeapon; set => subWeapon = value; }
    public Transform[] SubShootTransform { get => subShootTransform; set => subShootTransform = value; }
    public float SubWeaponDelayTimer { get => subWeaponDelayTimer; set => subWeaponDelayTimer = value; }

    protected override void Start()
    {
        moveDirection = Vector2.down;
        moveSpeed = 1f;
        base.Start();
        if (subWeapon != null)
            subWeapon.Init();
        subWeaponDelayTimer = 0f;
    }

    private void OnEnable()
    {
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

    public void ShootSubWeapon()
    {
        if (subWeapon == null)
            return;

        subWeaponDelayTimer += Time.deltaTime;
        if (subWeaponDelayTimer > subWeapon.reloadDelay)
        {
            subWeaponDelayTimer = 0f;
            int count = subShootTransform.Length;
            for (int i = 0; i < count; i++)
            {
                string subWeaponName = subWeapon.bulletPrefab.name;
                GameObject bulletObj = gm.GetBullet(subWeaponName);
                bulletObj.transform.position = subShootTransform[i].position;
            }
        }
    }
}