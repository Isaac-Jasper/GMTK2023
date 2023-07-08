using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DiceController : MonoBehaviour
{
    private TMP_Text number; //change to an array of images later
    [SerializeField]
    private int currentNumber;
    private Image buttonImage;

    bool isLocked = false;
    private void Awake() {
        buttonImage = GetComponent<Image>();
        number = transform.GetComponentInChildren<TMP_Text>();
    }
    public void RollAndSet(int num) {
        SetNumber(num);
        PlayRollAnimation();
    }
    public void RollBackwardsAndSet(int num) {
        SetNumber(num);
        PlayRollBackwardsAnimation();
    }
    public void SetNumber(int num) {
        if (num == 0) number.text = num.ToString(); //debuging purposes
        else if (num > 6 || num < 1) {
            Debug.LogError("number must be between 1 and 6");
            return;
        }
        number.text = num.ToString();
        currentNumber = num;
    }
        private void PlayRollBackwardsAnimation() {
        Debug.Log("Rolled backwards");
        //play roll backwards animation, probably animate with code
    }
    private void PlayRollAnimation() {
        Debug.Log("Roll");
        //play roll animation, probably animate with code
    }
    public void ToggleLock() {
        SetIsLocked(!GetIsLocked());
        if (GetIsLocked()) buttonImage.color = new Color(0.6f, 0.6f, 0.6f, 1f);
        else buttonImage.color = Color.white;
        //lock animation
    }
    public void SetIsLocked(bool isLocked) {
        this.isLocked = isLocked;
    }

    public bool GetIsLocked() {
        return isLocked;
    }
    public int GetCurrentNumber() {
        return currentNumber;
    }
}
