using System;
using UnityEngine;
using UnityEngine.UI;

public class Footer : MonoBehaviour
{
    [Serializable]
    public class FooterButtonData
    {
        public ScreenTypes screenActivator;
        public Button button;
        public Image bg;
        public Sprite activeState;
        public Sprite deActiveState;
        public bool isActive;
    }

    [SerializeField] private int _firstActiveButton;
    [SerializeField] private FooterButtonData[] _buttons;

    public void Init()
    {
        ButtonPressed(_buttons[_firstActiveButton]);

        for (int i = 0; i < _buttons.Length; i++)
        {
            int index = i;
            _buttons[index].button.onClick.AddListener(() => ButtonPressed(_buttons[index]));
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            int index = i;
            _buttons[index].button.onClick.RemoveListener(() => ButtonPressed(_buttons[index]));
        }
    }

    private void ButtonPressed(FooterButtonData footerButton)
    {
        Debug.Log(footerButton.screenActivator);
        ResetButtons();

        if (!footerButton.isActive)
        {
            footerButton.isActive = true;
            footerButton.bg.sprite = footerButton.activeState;
            UIManager.Instance.ShowScreen(footerButton.screenActivator);
        }
    }

    private void ResetButtons()
    {
        foreach (var button in _buttons)
        {
            button.isActive = false;
            button.bg.sprite = button.deActiveState;
        }
    }
}
