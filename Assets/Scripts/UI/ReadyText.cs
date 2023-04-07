using TMPro;
using UnityEngine;

public class ReadyText : MonoBehaviour
{
    private readonly float timer = 0.5f;
    private float duration = 0f;
    private bool isOff = false;
    public TextMeshProUGUI readyText;
    private GameManager gm;

    private void Awake()
    {
        readyText = GetComponent<TextMeshProUGUI>();
        gm = GameManager.Instance;
    }

    private void OnEnable()
    {
        duration = 0f;
        isOff = false;
        readyText.enabled = true;
    }

    private void Update()
    {
        if (gm.isClear)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                gm.StartGame();
        }
        else
        {
            if (Input.anyKeyDown)
                gm.StartGame();

            duration += Time.deltaTime;
            if (duration > timer)
            {
                isOff = !isOff;
                readyText.enabled = isOff;
                duration = 0f;
            }
        }
    }
}