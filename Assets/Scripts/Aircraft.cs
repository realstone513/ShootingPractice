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

    protected float HPRatio { get => curHp * inverseHP; }

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

    public virtual void GetDamage(float damage)
    {
        curHp -= damage;
        anim.SetTrigger("OnHit");
        if (hpBar != null)
            hpBar.SetFill(HPRatio);
    }

    protected void SetFullHP()
    {
        curHp = healthPoint;
    }

    public abstract void ShootMainWeapon();
    public abstract void ShootSubWeapon();
}