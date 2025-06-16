using System.IO;
using UnityEngine;

public class JsonStorage
{
    public void SaveToJson<T>(string key, T data)
    {
        string filePath = GetFilePath(key);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log($"Saving  {data} in {filePath} path done correct!");
    }

    public T LoadFromJson<T>(string key)
    {
        string filePath = GetFilePath(key);

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"File {filePath} has not been found");
            return default;
        }

        string json = File.ReadAllText(filePath);

        Debug.Log($"Loading  {key} from {filePath} path done correct!");
        return JsonUtility.FromJson<T>(json);        
    }

    public bool Exists(string key)
    {
        return File.Exists(GetFilePath(key));
    }

    private string GetFilePath(string key)
    {   
        return Path.Combine(Application.persistentDataPath, $"{key}.json");
    }
    // 🔥 Новый метод: удаляет все файлы в указанной папке
    public void DeleteAllSaveFiles()
    {
        if (Directory.Exists(Application.persistentDataPath))
        {
            string[] files = Directory.GetFiles(Application.persistentDataPath);

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (IOException e)
                {
                    Debug.LogError($"Ошибка при удалении файла: {file}. {e.Message}");
                }
            }
        }
        else
        {
            Debug.LogWarning($"Папка не найдена: {Application.persistentDataPath}");
        }
    }
}