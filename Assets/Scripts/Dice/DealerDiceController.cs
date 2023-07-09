using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerDiceController : MonoBehaviour
{
    private static DealerDiceController instance;
    public static DealerDiceController Instance {
        get { return instance; }
    }
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
    }
    [SerializeField]
    private DiceController[] diceSet;
    [SerializeField]
    private int[] value;

    public void Roll() {
        for (int i = 0; i < diceSet.Length; i++) {
            diceSet[i].SetNumber(value[i]);
        }
    }
}
