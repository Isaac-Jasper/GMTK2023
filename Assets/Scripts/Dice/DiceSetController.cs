using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSetController : MonoBehaviour
{
    [SerializeField]
    private DiceController[] diceSet;
    private DiceController[] nextRollSet;
    [SerializeField]
    private GameObject[] nextRollSetObjects;
    [SerializeField]
    private int[] diceWeights = { 1, 1, 1, 1, 1, 1 }; //6 nums for 6 sides of a die
    private int[] listPosition = { 0, 0, 0, 0, 0 }; //5 0's for 5 dice
    private List<int>[] knownRolls = new List<int>[] {
    new List<int>(),
    new List<int>(),
    new List<int>(),
    new List<int>(),
    new List<int>()
    };
    [SerializeField]
    private int globalRollNum = 0;
    private bool initialRoll = true,
        canRoll = true;
    private void Awake() {
        InputManager.Instance.OnRollPerformed += Roll;
        InputManager.Instance.OnReverseRollPerformed += ReverseRoll;
        foreach (List<int> c in knownRolls) {
            c.Add(0);
        }
        nextRollSet = new DiceController[nextRollSetObjects.Length];
        for (int i = 0; i < nextRollSetObjects.Length; i++) {
            nextRollSet[i] = nextRollSetObjects[i].GetComponent<DiceController>();
            nextRollSetObjects[i].SetActive(false);
        }
    }
    public void ReverseRoll() {
        if (globalRollNum == 0) return;
        globalRollNum--;

        for (int j = 0; j < diceSet.Length; j++) {
            if (listPosition[j] - 1 < globalRollNum) continue;

            listPosition[j]--;
            nextRollSetObjects[j].SetActive(true);
            nextRollSet[j].SetNumber(diceSet[j].GetCurrentNumber());
            if (listPosition[j] == 0) {
                diceSet[j].SetNumber(0);
            } else
                diceSet[j].SetNumber(knownRolls[j][listPosition[j]]);
        }
        OddsCalculator.Instance.ReverseOdds();
    }
    public void Roll() {
        for (int j = 0; j < diceSet.Length; j++) {
            if (!diceSet[j].GetIsLocked()) {
                globalRollNum++;
                break;
            }
        }

        for (int j = 0; j < diceSet.Length; j++) {
            DiceController currentDie = diceSet[j];
            if (currentDie.GetIsLocked()) continue;

            if (!initialRoll && listPosition[j] + 1 != knownRolls[j].Count) {
                listPosition[j]++;
                currentDie.SetNumber(knownRolls[j][listPosition[j]]);

                if (listPosition[j] + 1 == knownRolls[j].Count) 
                    nextRollSetObjects[j].SetActive(false);
                else
                    nextRollSet[j].SetNumber(knownRolls[j][listPosition[j] + 1]);

                continue;
            }
            int sum = 0;
            foreach (int num in diceWeights) sum += num;
            
            int rand = Random.Range(0, sum);
            sum--;
            for (int i = diceWeights.Length - 1; i >= 0; i--) {
                sum -= diceWeights[i];
                if (rand > sum) {
                    currentDie.SetNumber(i + 1);
                    knownRolls[j].Add(i + 1);
                    listPosition[j]++;
                    break;
                }
            }
        }
        initialRoll = false;
        OddsCalculator.Instance.CalculateOdds();
    }
    /// <returns>value of set in the form { rank of hand, sum of nums used in hand }</returns>
    public int[] GetScore() {
        int[] nums = new int[5];
        for (int i = 0; i < nums.Length; i++) {
            nums[i] = diceSet[i].GetCurrentNumber();
        }

        int numForSet = 0;
        bool pair = false;
        bool three = false;
        for (int i = 1; i < 7; i++) {
            int count = 0;
            for (int j = 0; j < nums.Length; j++) {
                if (nums[j] == i) count++;
            }
            switch (count) {
                case 2:
                    if (pair) return new int[] { 2, i + numForSet };
                    if (three) return new int[] { 4, i + numForSet };
                    pair = true;
                    numForSet += i;
                    break;
                case 3:
                    if (pair) return new int[] { 4, i + numForSet };
                    three = true;
                    numForSet += i;
                    break;
                case 4:
                    return new int[] { 5, i };
                case 5:
                    return new int[] { 6, i };
            }
        }
        if (pair) return new int[] { 1, numForSet };
        if (three) return new int[] { 3, numForSet };
        return new int[] { 0, 0 };
    }

    //setters
    public void SetCanRoll(bool canRoll) {
        this.canRoll = canRoll;
    }
    public void SetDiceWeights(int[] diceWeights) {
        this.diceWeights = diceWeights;
    }
    //getters
    public DiceController[] GetDiceSet() {
        return diceSet;
    }
    public bool GetCanRoll() {
        return canRoll;
    }
    public int[] GetDiceWeights() {
        return diceWeights;
    }
}
