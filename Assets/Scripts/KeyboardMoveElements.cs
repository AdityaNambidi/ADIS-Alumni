using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyboardMoveElements : MonoBehaviour
{
    public bool InpClicked = false;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform UiElements;

    [SerializeField] private TMP_Text tst;
    [SerializeField] private float tstInp;

    void Update()
    {
        if (InpClicked) {
            float keyboardH =  GetRelativeKeyboardHeight(UiElements, false)/341.3333f;
            //if (keyboardH == 386/192) { keyboardH = 0; }
            //tst.text = keyboardH.ToString();
            UiElements.position = new Vector2(UiElements.position.x, keyboardH);

            print(Screen.height);
        } 
    }


    private static int GetKeyboardHeight(bool includeInput)
    {
#if UNITY_EDITOR
        return 0;
#elif UNITY_ANDROID
        using (AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject unityPlayer = unityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
            AndroidJavaObject view = unityPlayer.Call<AndroidJavaObject>("getView");
            AndroidJavaObject dialog = unityPlayer.Get<AndroidJavaObject>("mSoftInputDialog");
            if (view == null || dialog == null)
                return 0;
            var decorHeight = 0;
            if (includeInput)
            {
                AndroidJavaObject decorView = dialog.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");
                if (decorView != null)
                    decorHeight = decorView.Call<int>("getHeight");
            }
            using (AndroidJavaObject rect = new AndroidJavaObject("android.graphics.Rect"))
            {
                view.Call("getWindowVisibleDisplayFrame", rect);
                return Screen.height - rect.Call<int>("height") + decorHeight;
            }
        }
#elif UNITY_IOS
        return (int)TouchScreenKeyboard.area.height;
#endif
    }


    public static int GetRelativeKeyboardHeight(RectTransform rectTransform, bool includeInput)
    {
        int keyboardHeight = GetKeyboardHeight(includeInput);
        float screenToRectRatio = Screen.height / rectTransform.rect.height;
        float keyboardHeightRelativeToRect = keyboardHeight / screenToRectRatio;

        return (int) keyboardHeightRelativeToRect;
    }
}
