using UnityEngine;

public class Aircraft : MonoBehaviour
{
    public float healthPoint;
    public float curHp;
    private Animator anim;

    private void Awake()
    {
        curHp = healthPoint;
        anim = GetComponent<Animator>();
    }

    public void GetDamage(float Damage)
    {
        curHp -= Damage;
        anim.SetTrigger("OnHit");
        if (curHp < 0f)
        {
            Debug.Log($"{name} destroy");
            Destroy(gameObject);
        }
    }
}