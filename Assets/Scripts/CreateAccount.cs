using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateAccount : MonoBehaviour
{
    [SerializeField] private string url;

    [SerializeField] private TMP_Text alert;
    [SerializeField] private TMP_InputField NameInput;

    private string responce;

    void Awake()
    {
        if (PlayerPrefs.GetInt("ac") == 1) {
            //go to chat scene'
            SceneManager.LoadScene("ChatScene");
        }

        StartCoroutine(PostReq(url, "_______start"));

    }

    public void MakeAccount() 
    {
        name = NameInput.text;

        StartCoroutine(PostReq(url, name));

        print(responce);

        if (responce == "Account Created") {
            print(name);
            alert.text = responce;
            PlayerPrefs.SetString("name", name);
            PlayerPrefs.SetInt("ac", 1);

            SceneManager.LoadScene("ChatScene");

        } else if (responce == "Username Aldredy Exists") {
            alert.text = responce;
        }

    }

    public void DeleteMemory() {
        PlayerPrefs.DeleteAll();
    }

    IEnumerator PostReq(string url, string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            responce = www.downloadHandler.text;
        }

    }
}
