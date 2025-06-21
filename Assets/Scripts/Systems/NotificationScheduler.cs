using UnityEngine;
using Unity.Notifications.iOS;
using System;

public class NotificationScheduler : MonoBehaviour
{
    private string[] messages = new string[]
   {
        "Your daily reward is waiting. Come back and collect your bonus now!",
        "Limited-time event unlocked. Play now and grab exclusive rewards.",
        "Haven’t spun today? Your lucky streak might just be one tap away.",
        "A brand new slot just dropped. Discover it now in Social Slots.",
        "You’ve slipped in the leaderboard. Play now and reclaim your spot."
   };

    void Start()
    {
        // Перевіряємо, чи користувач увімкнув сповіщення в UI
        if (SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.Notification) == 1)
        {
            // Перевіряємо статус дозволу
            var settings = iOSNotificationCenter.GetNotificationSettings();

            if (settings.AuthorizationStatus == AuthorizationStatus.Authorized)
            {
                // Видаляємо старі сповіщення
                iOSNotificationCenter.RemoveAllScheduledNotifications();

                ScheduleNotification();
            }
            else
            {
               // Debug.Log("Сповіщення не дозволені або ще не дозволені.");
            }
        }
    }

    private void ScheduleNotification()
    {
        string randomMessage = messages[UnityEngine.Random.Range(0, messages.Length)];

        var notification = new iOSNotification()
        {
            Identifier = "daily_notification",
            Title = "We are waiting for you!",
            Body = randomMessage,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            Trigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(24, 0, 0), // Через 24 години
                Repeats = false
            }
        };

        iOSNotificationCenter.ScheduleNotification(notification);
        //Debug.Log($"Сповіщення заплановано: {randomMessage}");
    }

    private void OnApplicationQuit()
    {
        // Перезаплановуємо сповіщення при виході з гри
        if (SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.Notification) == 1)
        {
            ScheduleNotification();
        }
    }
}
