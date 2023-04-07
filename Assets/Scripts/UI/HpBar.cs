using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Image fill;

    public void SetFill(float value)
    {
        fill.fillAmount = value;
    }
}