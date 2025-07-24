using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : BasicScreen
{
    [SerializeField] private Button _vibration;
    [SerializeField] private Button _notifications;
    [SerializeField] private Button _privacyPolicy;
    [SerializeField] private Button[] _autoStop;

    [SerializeField] private Sprite _activeButton;
    [SerializeField] private Sprite _inactiveButton;

    [Header("Autostop")]
    [SerializeField] private GameObject _autostopButton;
    [SerializeField] private GameObject _autostopPanel;
    [SerializeField] private Button _addAutostopTime;
    [SerializeField] private Button _removeAutostopTime;
    [SerializeField] private Button _saveAutostop;
    [SerializeField] private Button _resetAutostop;
    [SerializeField] private TMPro.TMP_Text _autostopTimeText;
    [SerializeField] private TMPro.TMP_Text _autostopTimeText2;

    private int currentAutostopTime = 120; 
    private int defaultAutostopTime = 120; // Default autostop time in seconds

    private void Start()
    {
        _privacyPolicy.onClick.AddListener(PrivacyPolice); 
        _vibration.onClick.AddListener(Vibration);
        _notifications.onClick.AddListener(Notification);
        
        _autoStop[0].onClick.AddListener(AutoStop);
        _autoStop[1].onClick.AddListener(AutoStop);

        _addAutostopTime.onClick.AddListener(AddAutostopTime);
        _removeAutostopTime.onClick.AddListener(RemoveAutostopTime);
        _saveAutostop.onClick.AddListener(SaveAutostop);
        _resetAutostop.onClick.AddListener(ResetAutostop);
    }

    private void OnDestroy()
    {
        _privacyPolicy.onClick.RemoveListener(PrivacyPolice);
        _vibration.onClick.RemoveListener(Vibration);
        _notifications.onClick.RemoveListener(Notification);
        _autoStop[0].onClick.RemoveListener(AutoStop);
        _autoStop[1].onClick.RemoveListener(AutoStop);

        _addAutostopTime.onClick.RemoveListener(AddAutostopTime);
        _removeAutostopTime.onClick.RemoveListener(RemoveAutostopTime);
        _saveAutostop.onClick.RemoveListener(SaveAutostop);
        _resetAutostop.onClick.RemoveListener(ResetAutostop);
    }
    
    public override void ResetScreen()
    {
    }

    public override void SetScreen()
    {
        SetButtons();
        SetAutostop();
    }

    private void PrivacyPolice()
    {
        Application.OpenURL("https://www.privacypolicies.com/live/6029e8a7-3557-4f5c-827c-c21ae7031a85");
    }

    private void SetAutostop()
    {
        if(SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.AutoStop) == 0)
        {
            _autostopPanel.SetActive(false);
            _autostopButton.SetActive(true);
        }
        else
        {
            _autostopPanel.SetActive(true);
            _autostopButton.SetActive(false);
            currentAutostopTime = SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.AutoStop);
            TextManager textManager = new TextManager();
            textManager.SetTimerText(_autostopTimeText, currentAutostopTime, showMinutes: true);
            _autostopTimeText2.text = $"{currentAutostopTime / 60} min";

        }
    }

    private void SetButtons()
    {
        if (SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.Vibro) == 1)
        {
            _vibration.gameObject.GetComponent<Image>().sprite = _activeButton;
        }
        else
        {
            _vibration.gameObject.GetComponent<Image>().sprite = _inactiveButton;
        }
        if (SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.Notification) == 1)
        {
            _notifications.gameObject.GetComponent<Image>().sprite = _activeButton;
        }
        else
        {
            _notifications.gameObject.GetComponent<Image>().sprite = _inactiveButton;
        }
    }
    private void Vibration()
    {
        if (SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.Vibro) == 1)
        {
            SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.Vibro, 0);
            _vibration.gameObject.GetComponent<Image>().sprite = _inactiveButton;
        }
        else
        {
            SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.Vibro, 1);
            _vibration.gameObject.GetComponent<Image>().sprite = _activeButton;
        }

    }

    private void Notification()
    {
        if (SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.Notification) == 1)
        {
            SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.Notification, 0);
            _notifications.gameObject.GetComponent<Image>().sprite = _inactiveButton;
        }
        else
        {
            SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.Notification, 1);
            _notifications.gameObject.GetComponent<Image>().sprite = _activeButton;
        }
    }

    private void AutoStop()
    {
        if(SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.AutoStop) == 0)
        {
            SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.Notification, 120);
            _autostopButton.SetActive(false);
            _autostopPanel.SetActive(true);
        }
        else
        {
            SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.AutoStop, 0);
            _autostopButton.SetActive(true);
            _autostopPanel.SetActive(false);
        }
    }

    private void AddAutostopTime()
    {
        currentAutostopTime += 60; // Add 1 minute
        TextManager textManager = new TextManager();
        textManager.SetTimerText(_autostopTimeText, currentAutostopTime, showMinutes: true);
        _autostopTimeText2.text = $"{currentAutostopTime / 60} min";
    }

    private void RemoveAutostopTime()
    {
        if (currentAutostopTime > 60)
        {
            currentAutostopTime -= 60; // Remove 1 minute
            TextManager textManager = new TextManager();
            textManager.SetTimerText(_autostopTimeText, currentAutostopTime, showMinutes: true);
            _autostopTimeText2.text = $"{currentAutostopTime / 60} min";
        }
    }

    private void ResetAutostop()
    {
        TextManager textManager = new TextManager();
        currentAutostopTime = defaultAutostopTime;
        textManager.SetTimerText(_autostopTimeText, currentAutostopTime, showMinutes: true);
        _autostopTimeText2.text = $"{currentAutostopTime / 60} min";        
    }

    private void SaveAutostop()
    {
        SaveManager.PlayerPrefs.SaveInt(GameSaveKeys.AutoStop, currentAutostopTime);
    }
}
