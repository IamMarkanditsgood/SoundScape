using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private string sceneName; // Назва сцени, яку потрібно завантажити
    [SerializeField] private TMP_Text progressText; // Текстове поле для відсотка завантаження
    [SerializeField] private Image progressBar; // Заповнення прогрес-бару

    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // Чекаємо завершення завантаження

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // Приводимо до шкали 0-100%
            if (progressText != null)
            {
                progressText.text = $"{progress * 100:F0}%";
            }
            if (progressBar != null)
            {
                progressBar.fillAmount = progress;
            }

            if (progress >= 1f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
