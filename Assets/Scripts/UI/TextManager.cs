using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManager
{
    public enum TimeFormat
    {
        ColonSeparated,    // HH:MM:SS
        LettersSeparated   // 99h 22m 33s
    }

    public void SetText(object message, TMP_Text textRow, bool formatKNumber = false, string frontAddedMessage = "", string endAddedMessage = "", bool addToPrevious = false)
    {
        string formattedText = GetFormattedText(message, formatKNumber);

        if (addToPrevious)
        {
            textRow.text += frontAddedMessage + formattedText + endAddedMessage;
        }
        else 
        {
            textRow.text = frontAddedMessage + formattedText + endAddedMessage;
        }
    }
    public void SetText(object message, Text textRow, bool formatKNumber = false, string frontAddedMessage = "", string endAddedMessage = "", bool addToPrevious = false)
    {
        string formattedText = GetFormattedText(message, formatKNumber);

        if (addToPrevious)
        {
            textRow.text += frontAddedMessage + formattedText + endAddedMessage;
        }
        else
        {
            textRow.text = frontAddedMessage + formattedText + endAddedMessage;
        }
    }

    public void SetTimerText(
    TMP_Text textRow,
    float seconds,
    bool showMinutes = false,
    bool showHours = false,
    string frontAddedMessage = "",
    string endAddedMessage = "",
    bool addToPrevious = false,
    string symbolBetween = ":",
    TimeFormat format = TimeFormat.ColonSeparated)
    {
        string formattedText = FormatTime(seconds, showMinutes, showHours, symbolBetween, format);

        if (addToPrevious)
        {
            textRow.text += frontAddedMessage + formattedText + endAddedMessage;
        }
        else
        {
            textRow.text = frontAddedMessage + formattedText + endAddedMessage;
        }
    }

    private string FormatTime(
    float seconds,
    bool showMinutes,
    bool showHours,
    string symbolBetween,
    TimeFormat format)
    {
        if (!showMinutes)
            showHours = false;
        if (showHours)
            showMinutes = true;

        int totalSeconds = Mathf.FloorToInt(seconds);
        int secs = totalSeconds % 60;
        int mins = (totalSeconds / 60) % 60;
        int hrs = totalSeconds / 3600;

        switch (format)
        {
            case TimeFormat.ColonSeparated:
                if (!showMinutes)
                    return secs.ToString("D2");
                else if (showMinutes && !showHours)
                    return mins.ToString("D2") + symbolBetween + secs.ToString("D2");
                else
                    return hrs.ToString("D2") + symbolBetween + mins.ToString("D2") + symbolBetween + secs.ToString("D2");

            case TimeFormat.LettersSeparated:
                string result = "";
                if (showHours)
                    result += hrs + "h ";
                if (showMinutes)
                    result += mins + "m ";
                result += secs + "s";
                return result.TrimEnd();

            default:
                return secs.ToString("D2");
        }
    }

    private string GetFormattedText(object message, bool formatKNumber =false)
    {
        if (formatKNumber && message is int number)
        {
            return FormatKNumber(number);
        }

        return message.ToString();
    }

    private string FormatKNumber(int number)
    {
        return number >= 1000
            ? (number / 1000f).ToString("0.#") + "K"
            : number.ToString();
    }
}