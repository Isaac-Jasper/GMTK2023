using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    private static GameController instance;
    public static GameController Instance {
        get { return instance; }
    }

    [SerializeField]
    DialogueManager dealerDialogue, friendDialogue;
    [SerializeField]
    private int[] talkOrder; //0 for friend 1 for dealer
    private int talkPointer = 0;
    bool canClickDealerDialogue;
    bool canClickFriendDialogue;
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
    }
    private void Start() {
        InputManager.Instance.OnClickStarted += ClickStart;
        dealerDialogue.OnDialogueFinished += DealerDialogueFinished;
        friendDialogue.OnDialogueFinished += FriendDialogueFinished;

        InputManager.Instance.lockReverseRoll = true;
        InputManager.Instance.lockRoll = true;
        dealerDialogue.StartDialogue();
        friendDialogue.StartDialogue();

        StartDialogue();
    }
    private void DealerDialogueFinished() {
        EndDialogue();
    }
    private void FriendDialogueFinished() {
        EndDialogue();
    }
    private void ClickStart(Vector2 pos, float time) {
        StartDialogue();
    }
    private void StartDialogue() {

        if (!(canClickFriendDialogue || canClickDealerDialogue)) return;
        if (talkPointer == talkOrder.Length) return;

        InputManager.Instance.lockAll(true);
        if (talkOrder[talkPointer++] == 1)
            dealerDialogue.DisplayNext();
        else
            friendDialogue.DisplayNext();
    }
    private void EndDialogue() {
        InputManager.Instance.lockAll(false);
    }
    public void EnterDealer(bool isEnter) {
        canClickDealerDialogue = isEnter;
    }
    public void EnterFriend(bool isEnter) {
        canClickFriendDialogue = isEnter;
    }
}
