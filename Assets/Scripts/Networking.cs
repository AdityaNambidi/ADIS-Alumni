using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using TMPro;

// do memory of msgs with player prefs and lists

public class Networking : MonoBehaviour
{

    [SerializeField] private TMP_InputField InputField;
    [SerializeField] private TMP_InputField recv;
    [SerializeField] private Transform Content;
    [SerializeField] private GameObject textBox;

    [SerializeField] private KeyboardMoveElements scrollUI;

    private bool MakeMsgBox = false;
    private string RcvText = "";

    private string name;

    WebSocket ws;

    private void Start()
    {
       ws = new WebSocket("wss://JsChatServer.fangcreator.repl.co/");

        ws.Connect();

        ws.OnMessage += (sender, e) =>
        {
            print(e.Data);
            MakeMsgBox = true;
            RcvText = e.Data;
        };

        ws.Send("{\"type\":\"SetName\", \"name\":\"" + PlayerPrefs.GetString("name") + "\"}");

        name = PlayerPrefs.GetString("name");
    }

    

    void Update() {
        
        if (MakeMsgBox) {
            Message msg = JsonUtility.FromJson<Message>(RcvText);
            print(RcvText);
            ShowMessage(msg.text, msg.name);
            MakeMsgBox = false;
        }

        if (InputField.isFocused) {
            scrollUI.InpClicked = true;
        }

        if (InputField.text.Length != 0)
            if (InputField.text[InputField.text.Length-1] == '\n'){
                InputField.text = InputField.text.Replace("\n", "");
                SendMessage();
            }
    }


    private void ShowMessage(string msg, string name) {

        GameObject msgBox = Instantiate(textBox, new Vector2(0, 0), Quaternion.identity);

        msgBox.GetComponent<TextBox>().msgText = msg;
        msgBox.GetComponent<TextBox>().name = name;

        if (name == "You") {
            msgBox.transform.Find("txtBG").gameObject.GetComponent<Image>().color =  new Color(10/255f, 70/255f, 68/255f);;
        }
        //print(msgBox.GetComponent<TextBox>().Box.GetComponent<RectTransform>().rect.height);

        msgBox.GetComponent<RectTransform>().parent = Content;
        msgBox.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);

    }

    public void SendMessage() {
        string msgText = InputField.text;

        if (name == "") {
            name = "noName";
        }

        Message sMsg = new Message();

        sMsg.text = msgText;
        sMsg.name = name; 
        sMsg.type = "text";
        sMsg.to = recv.text;

        string jsonMsg = JsonUtility.ToJson(sMsg);


        ws.Send(jsonMsg);

        InputField.text = "";

        ShowMessage(msgText, "You");

    }




}

public class Message 
{
    public string type;
    public string text;
    public string name;
    public string to;
}