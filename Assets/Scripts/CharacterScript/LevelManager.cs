using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject CoinText;
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CoinIncrease()
    {
        score += 1;
        CoinText.GetComponent<TextMeshProUGUI>().text = "Coins: " + (score).ToString();
    }
}
