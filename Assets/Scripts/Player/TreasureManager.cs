using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreasureManager : MonoBehaviour
{
    [SerializeField] private int coinCount = 0;
    [SerializeField] private TMP_Text coinText;

    private void Update()
    {
        coinText.text = coinCount.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int treasureValue = other.gameObject.GetComponent<Coin>().CoinCount();
        Animator treasureAnim = other.gameObject.GetComponent<Animator>();

        if (other.gameObject.CompareTag("Coin"))
        {
            treasureAnim.SetTrigger("TreasureTacken");
            coinCount += treasureValue;
        }
    }


}
