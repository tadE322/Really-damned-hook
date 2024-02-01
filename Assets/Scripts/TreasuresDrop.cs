using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TreasuresDrop : MonoBehaviour
{
    [SerializeField] private GameObject silverCoin;
    [SerializeField] private GameObject goldCoin;
    [SerializeField] private GameObject diamond;
    [SerializeField] private GameObject izumrud;
    [SerializeField] private GameObject rubin;
    [SerializeField] private GameObject goldenSkull;
    
    
    private int maxTreasuresToDrop = 5;
    private int minTreasuresToDrop = 0;

    public void DropTreasures(int numberOfTreasure, Vector3 positionToDrop, int valueToMinCount, int valueToMaxCount)
    {
        int coinToDrop = Random.Range(minTreasuresToDrop + valueToMinCount, maxTreasuresToDrop + valueToMaxCount);

        for (int i = 0; i < coinToDrop; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));

            switch (numberOfTreasure)
            {
                case 1:
                    Instantiate(silverCoin, positionToDrop + randomOffset, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(goldCoin, positionToDrop + randomOffset, Quaternion.identity);
                    break;
                case 3:
                    Instantiate(diamond, positionToDrop + randomOffset, Quaternion.identity);
                    break;
                case 4:
                    Instantiate(izumrud, positionToDrop + randomOffset, Quaternion.identity);
                    break;
                case 5:
                    Instantiate(rubin, positionToDrop + randomOffset, Quaternion.identity);
                    break;
                case 6:
                    Instantiate(goldenSkull, positionToDrop + randomOffset, Quaternion.identity);
                    break;
            }

        }
    }
}
