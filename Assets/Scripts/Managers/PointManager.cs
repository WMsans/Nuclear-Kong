using TMPro;
using UnityEngine;

public class PointManager : MonoSingleton<PointManager>
{
    public int points;
    public TextMeshProUGUI text;

    public void AddPoints(int num)
    {
        points += num;
        UpdateUI();
    }

    public void LosePoints(int num)
    {
        points -= num;
        UpdateUI();
    }

    public void UpdateUI()
    {
        text.text = points.ToString("000000");
    }
}
