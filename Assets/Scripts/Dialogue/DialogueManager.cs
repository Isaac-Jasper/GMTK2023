using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour {
    
    private TMP_Text textSentences;
    [SerializeField]
    private Dialogue dialogue;
    [SerializeField]
    private Animator TextBoxAnimation;
    [SerializeField]
    private int typeSoundSpeed
              , basicDialogueSoundCount;
    [SerializeField]
    private float typeSpeed;

    private Queue<string> Sentences;

    public delegate void DialogueFinishedEvent();
    public event DialogueFinishedEvent OnDialogueFinished;

    protected virtual void DialogueFinished() {
        OnDialogueFinished?.Invoke();
    }
    private void Awake() {
        Sentences = new Queue<string>();
    }
    public void StartDialogue() { //applies the dialogue to the textbox and loads the dialogue into the sentences queue
        textSentences = dialogue.textBox;
        Sentences.Clear();

        foreach (string c in dialogue.sentences) {
            Sentences.Enqueue(c);
        }
        StartCoroutine(StartAnimation());
    }
    public void DisplayNext() { //after the dialogue is started use display next to continue the conversation, will also auto end if there are no more sentences loaded
        if (Sentences.Count == 0) {
            EndDialogue();
            StartDialogue();
            return;
        }

        string sentence = Sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TextAnimation(sentence));
    }
    IEnumerator StartAnimation() {
        //TextBoxAnimation.SetBool("IsEnter", true);
        //yield return new WaitForSeconds(TextBoxAnimation.GetCurrentAnimatorStateInfo(0).length);
        yield return null;
    }
    IEnumerator TextAnimation(string sentence) {
        textSentences.text = "";

        for (int i = 0; i < sentence.Length; i++) {
            textSentences.text += sentence[i];
            if (i % typeSoundSpeed == 0) {
                int rand = Random.Range(0, basicDialogueSoundCount);

                //SoundManager.PlaySound(SoundManager.Sound.Dialogue_Basic01 + rand);
            }
            yield return new WaitForSeconds(typeSpeed);
        }
        DialogueFinished();
    }

    private void EndDialogue() { //clears sentences queue
        textSentences.text = "";
        //TextBoxAnimation.SetBool("IsEnter", false);
    }
}
