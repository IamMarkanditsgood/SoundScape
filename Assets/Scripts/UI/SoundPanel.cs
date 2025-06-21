using UnityEngine;
using UnityEngine.UI;

public class SoundPanel : DefaultSoundPanel
{
    public Button _deleteButton;
    public bool isLikeCollection;

    public void Start()
    {
        _deleteButton.onClick.AddListener(Deleate);
    }

    public void OnDestroy()
    {
        _deleteButton.onClick.RemoveListener(Deleate);
    }

    private void Deleate()
    {
        Vibrate();
        DeleatePopup deleatePopup = (DeleatePopup)UIManager.Instance.GetPopup(PopupTypes.DeletePopup);
        deleatePopup.Init(presetName, isLikeCollection);
        UIManager.Instance.ShowPopup(PopupTypes.DeletePopup);
    }
    public void Vibrate()
    {
#if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
#endif
    }
}
