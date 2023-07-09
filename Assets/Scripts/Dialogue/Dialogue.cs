using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[System.Serializable]
public class Dialogue{
    //when attached to a gameobject will conatin the sentences used in that conversation
    public TMP_Text textBox;
    [TextArea(3, 10)]
    public string[] sentences;
}
