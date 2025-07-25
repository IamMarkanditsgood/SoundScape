using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryScreen : BasicScreen
{
    [SerializeField] private PresetIamgeManager _presetImageManager;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _createNewPreset;

    [SerializeField] private Transform container;
    [SerializeField] private GameObject libraryPanelPrefab;
    private List<GameObject> _libraryPanels = new();

    private void Start()
    {
        _backButton.onClick.AddListener(Back);
        _createNewPreset.onClick.AddListener(CreateNewPresetPressed);
    }

    private void OnDestroy()
    {
        _backButton.onClick.RemoveListener(Back);
        _createNewPreset.onClick.RemoveListener(CreateNewPresetPressed);
    }

    public override void ResetScreen()
    {

    }

    public override void SetScreen()
    {
        CreatePanels();
    }

    public void CreatePanels()
    {
        foreach (GameObject panel in _libraryPanels)
        {
            Destroy(panel);
        }
        _libraryPanels.Clear();
        for (int i = 0; i < GameManager.instance.Presets.Count;i++)
        {
            GameObject panel = Instantiate(libraryPanelPrefab, container);
            panel.GetComponent<LibraryPanel>().SetPreset(GameManager.instance.Presets[i], _presetImageManager);
            _libraryPanels.Add(panel);
        }
    }

    private void CreateNewPresetPressed()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.PresetCreator);
    }
    private void Back()
    {
        UIManager.Instance.ShowScreen(ScreenTypes.Home);
    }
}
