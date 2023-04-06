using UnityEngine;

public abstract class Aircraft : MonoBehaviour
{
    public float healthPoint;
    public float curHp;
    public HpBar hpBar;
    public int value;
    private Animator anim;
    private float inverseHP;

    [SerializeField]
    protected Weapon mainWeapon;
    [SerializeField]
    protected Transform mainShootTransform;
    protected float mainWeaponDelayTimer;

    [SerializeField]
    protected Weapon subWeapon;
    [SerializeField]
    protected Transform[] subShootTransform;
    protected float subWeaponDelayTimer;

    protected GameManager gm;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        inverseHP = 1f / healthPoint;
        gm = GameManager.Instance;
    }

    protected virtual void OnEnable()
    {
        SetFullHP();
        mainWeaponDelayTimer = mainWeapon.reloadDelay;
    }

    public void GetDamage(float Damage)
    {
        curHp -= Damage;
        anim.SetTrigger("OnHit");
        if (hpBar != null)
            hpBar.SetFill(curHp * inverseHP);
        if (curHp < 0f)
        {
            if (gameObject.CompareTag("Enemy"))
            {
                gm.TranslateScore(value);
                gm.DestroyEnemyAircraft(gameObject, true);
            }
            else if (gameObject.CompareTag("Player"))
            {
                gm.EndGame();
                Debug.Log("Player Die");
            }
            else if (gameObject.CompareTag("Boss"))
            {
                gm.TranslateScore(value);
                gm.DestroyEnemyAircraft(gameObject, true);
                gm.ClearGame();
            }
        }
    }

    protected void SetFullHP()
    {
        curHp = healthPoint;
    }

    public abstract void ShootMainWeapon();
    public abstract void ShootSubWeapon();
}