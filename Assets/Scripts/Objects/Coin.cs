using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinCount = 0;

    public int CoinCount()
    {
        return coinCount;
    }

    public void TreasureDestroy() // вызывается в анимации
    {
        Destroy(gameObject);
    }
} 
