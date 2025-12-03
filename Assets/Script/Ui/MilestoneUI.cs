using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MilestoneUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI cogsText;
    [SerializeField] private TextMeshProUGUI enemiesText;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI maxRankText;
    [SerializeField] private Button next;

    private void Awake()
    {
    }

    public void SetResultsUI(string time, int cogsCollected, int maxCogs,
        int enemiesKilled, int maxEnemies,
        string rank, string maxRank, Color rankColor, Color maxRankColor)
    {
        SetTimeText(time);
        SetCogsText(cogsCollected, maxCogs);
        SetEnemiesText(enemiesKilled, maxEnemies);
        SetRankText(rank, rankColor);
        SetMaxRankText(maxRank, maxRankColor);
    }

    private void SetTimeText(string time)
    {
        timeText.text = time;
    }
    private void SetCogsText(int cogsCollected, int maxCogs)
    {
        cogsText.text = $"{cogsCollected} / {maxCogs}";
    }
    private void SetEnemiesText(int enemiesKilled, int maxEnemies)
    {
        enemiesText.text = $"{enemiesKilled} / {maxEnemies}";
    }
    private void SetRankText(string rank, Color color)
    {
        rankText.text = rank;
        rankText.color = color;
    }
    private void SetMaxRankText(string maxRank, Color color)
    {
        maxRankText.text = maxRank;
        maxRankText.color = color;
    }

    private void OnEnable()
    {
        next.onClick.AddListener(() => GameManager.Instance.LoadNextScene());
        EventManager.SetResultScreen += SetResultsUI;
    }

    private void OnDisable()
    {
        next.onClick.RemoveAllListeners();
        EventManager.SetResultScreen -= SetResultsUI;
    }
}
