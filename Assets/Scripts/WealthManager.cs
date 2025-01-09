using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WealthManager : MonoBehaviour
{
    public static WealthManager Instance;

    [SerializeField] int coinCount = 0;
    [SerializeField] int fishCount = 0;
    [SerializeField] int starCount = 0;
    [Space]
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] TextMeshProUGUI fishesText;
    [SerializeField] TextMeshProUGUI starsText;

    private void Awake()
    {
        Instance = this;

        if (!PlayerPrefs.HasKey("coin-count")) PlayerPrefs.SetInt("coin-count", coinCount);
        else coinCount = PlayerPrefs.GetInt("coin-count");

        if (!PlayerPrefs.HasKey("fish-count")) PlayerPrefs.SetInt("fish-count", fishCount);
        else fishCount = PlayerPrefs.GetInt("fish-count");

        if (!PlayerPrefs.HasKey("star-count")) PlayerPrefs.SetInt("star-count", starCount);
        else starCount = PlayerPrefs.GetInt("star-count");
    }

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        coinsText.text = coinCount.ToString();
        fishesText.text = fishCount.ToString();
        starsText.text = starCount.ToString();
    }

    public void AddCoin(int count)
    {
        coinCount += count;

        PlayerPrefs.SetInt("coin-count", coinCount);

        UpdateUI();

        GlobalEvent.InvokeGlobal("on-coin-add");
    }

    public void AddFish(int count)
    {
        fishCount += count;

        PlayerPrefs.SetInt("fish-count", fishCount);

        UpdateUI();

        GlobalEvent.InvokeGlobal("on-fish-add");
    }

    public void AddStar(int count)
    {
        starCount += count;

        PlayerPrefs.SetInt("star-count", starCount);

        UpdateUI();

        GlobalEvent.InvokeGlobal("on-star-add");
    }
}
