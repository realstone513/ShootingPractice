using UnityEngine;

public class Aircraft : MonoBehaviour
{
    public float healthPoint;
    public float curHp;
    public HpBar hpBar;
    private Animator anim;
    private float inverseHP;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        inverseHP = 1f / healthPoint;
    }

    private void OnEnable()
    {
        SetFullHP();
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
                GameManager.Instance.DestroyAircraft(gameObject);
            else if (gameObject.CompareTag("Player"))
                Debug.Log("Player Die");
        }
    }

    private void SetFullHP()
    {
        curHp = healthPoint;
    }
}