using UnityEngine;
using TMPro;


public class TextBox : MonoBehaviour
{
    [SerializeField] private TMP_Text msgBox;
    [SerializeField] private TMP_Text nameBox;

    public string msgText;
    public string name;

    void Start()
    {
        try {
            msgBox.text = msgText;
            nameBox.text = name;
        }
        catch {

        }
    }
}
