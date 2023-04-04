using TMPro;
using UnityEngine;

public class ReadyText : MonoBehaviour
{
    private readonly float timer = 0.5f;
    private float duration = 0f;
    private bool isOff = false;
    public TextMeshProUGUI readyText;

    private void Awake()
    {
        readyText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        duration = 0f;
        isOff = false;
        readyText.enabled = true;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
            GameManager.Instance.StartGame();

        duration += Time.deltaTime;
        if (duration > timer)
        {
            isOff = !isOff;
            readyText.enabled = isOff;
            duration = 0f;
        }
    }
}