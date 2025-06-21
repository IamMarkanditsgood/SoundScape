using UnityEngine;
using Unity.Notifications.iOS;
using System;

public class NotificationScheduler : MonoBehaviour
{
    private string[] messages = new string[]
   {
        "Your daily reward is waiting. Come back and collect your bonus now!",
        "Limited-time event unlocked. Play now and grab exclusive rewards.",
        "Haven�t spun today? Your lucky streak might just be one tap away.",
        "A brand new slot just dropped. Discover it now in Social Slots.",
        "You�ve slipped in the leaderboard. Play now and reclaim your spot."
   };

    void Start()
    {
        // ����������, �� ���������� ������� ��������� � UI
        if (SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.Notification) == 1)
        {
            // ���������� ������ �������
            var settings = iOSNotificationCenter.GetNotificationSettings();

            if (settings.AuthorizationStatus == AuthorizationStatus.Authorized)
            {
                // ��������� ���� ���������
                iOSNotificationCenter.RemoveAllScheduledNotifications();

                ScheduleNotification();
            }
            else
            {
               // Debug.Log("��������� �� �������� ��� �� �� ��������.");
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
                TimeInterval = new TimeSpan(24, 0, 0), // ����� 24 ������
                Repeats = false
            }
        };

        iOSNotificationCenter.ScheduleNotification(notification);
        //Debug.Log($"��������� �����������: {randomMessage}");
    }

    private void OnApplicationQuit()
    {
        // ��������������� ��������� ��� ����� � ���
        if (SaveManager.PlayerPrefs.LoadInt(GameSaveKeys.Notification) == 1)
        {
            ScheduleNotification();
        }
    }
}
