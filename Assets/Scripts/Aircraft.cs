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
        curHp = healthPoint;
        anim = GetComponent<Animator>();
        inverseHP = 1f / healthPoint;
    }

    public void GetDamage(float Damage)
    {
        curHp -= Damage;
        anim.SetTrigger("OnHit");
        if (hpBar != null)
            hpBar.SetFill(curHp * inverseHP);
        if (curHp < 0f)
        {
            Debug.Log($"{name} destroy");
            Destroy(gameObject);
        }
    }
}