using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OddsCalculator : MonoBehaviour
{
    private static OddsCalculator instance;
    public static OddsCalculator Instance {
        get { return instance; }
    }

    [SerializeField]
    DiceSetController diceSetController;
    [SerializeField]
    TMP_Text oddsText;
    LinkedList<float> oddsList;
    LinkedList<int> priorScore;
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        oddsList = new LinkedList<float>();
        priorScore = new LinkedList<int>();
        oddsList.AddLast(1);
        priorScore.AddLast(0);
    }
    public void CalculateOdds() {
        int[] currentScore = diceSetController.GetScore();
        if (priorScore.Last.Value >= currentScore[0]) {
            oddsList.AddLast(oddsList.Last.Value);
            priorScore.AddLast(priorScore.Last.Value);
            return;
        }

        DiceController[] diceSet = diceSetController.GetDiceSet();
        int lockCount = 0;
        for (int i = 0; i < diceSet.Length; i++) {
            if (diceSet[i].GetIsLocked()) lockCount++;
        }

        float odds = 0;
        switch (lockCount) {
            case 0:
                odds = OddsOfAtleastForFive(currentScore[0]);
                break;
            case 1:
                if (currentScore[0] == 6)
                    odds = 1f / 6 / 6 / 6 / 6;
                else if (currentScore[0] == 5)
                    odds = 1f / 6 / 6 / 3;
                else if (currentScore[0] == 4)
                    odds = 1f / 6 / 6;
                else
                    odds = OddsOfAtleastForFour(currentScore[0]);
                break;
            case 2:
                if (currentScore[0] == 6)
                    odds = 1f / 6 / 6 / 6;
                else if (currentScore[0] == 5)
                    odds = 1f / 6 / 3;
                else
                    odds = OddsOfAtleastForThree(currentScore[0]);
                break;
            case 3:
                if (currentScore[0] == 6)
                    odds = 1f / 6 / 6;
                else odds = 1f / 6;
                break;
            case 4:
                odds = 1f / 6;
                break;
            case 5:
                break;
            default:
                odds = 1f / 6;
                break;
        }
        oddsList.AddLast(odds*oddsList.Last.Value);
        oddsText.text = string.Format("1\n{0:#,0.0}", 1f / oddsList.Last.Value);
        priorScore.AddLast(currentScore[0]);
    }
    public void ReverseOdds() {
        oddsList.RemoveLast();
        priorScore.RemoveLast();
        Debug.Log(priorScore.Last.Value);
        oddsText.text = string.Format("1\n{0:#,0.0}", 1f / oddsList.Last.Value);
    }
    private float OddsOfAtleastForFive(int x) {
        switch (x) {
            case 0:
                return 1f;
            case 1:
                return 3600f / 7776 + OddsOfAtleastForFive(2);
            case 2:
                return 1800f / 7776 + OddsOfAtleastForFive(3);
            case 3:
                return 1200f / 7776 + OddsOfAtleastForFive(4);
            case 4:
                return 300f / 7776 + OddsOfAtleastForFive(5);
            case 5:
                return 150f / 7776 + OddsOfAtleastForFive(6);
            case 6:
                return 6f / 7776;
        }
        return 1f/6;
    }
    private float OddsOfAtleastForFour(int x) {
        switch (x) {
            case 0:
                return 1f/6;
            case 1:
                return 720f / 1296 + OddsOfAtleastForFour(2);
            case 2:
                return 90f / 1296 + OddsOfAtleastForFour(3);
            case 3:
                return 120f / 1296 + OddsOfAtleastForFour(5);
            case 5:
                return 6f / 1296;
        }
        return 1f / 6;
    }
    private float OddsOfAtleastForThree(int x) {
        switch (x) {
            case 0:
                return 1f/6;
            case 1:
                return 90f / 216 + OddsOfAtleastForThree(3);
            case 3:
                return 6f / 216;
        }
        return 1f / 6;
    }
    private float OddsOfAtleastForTwo(int x) {
        switch (x) {
            case 0:
                return 1f/6;
            case 1:
                return 6f / 36;
        }
        return 0;
    }
}
