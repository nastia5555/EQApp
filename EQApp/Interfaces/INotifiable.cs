using System;
using System.Collections.Generic;
using System.Text;

namespace EQApp.Interfaces;

// Интерфейс для уведомлений - любой класс может его реализовать
public interface INotifiable
{
    void SendNotification(string message);
    bool IsNotificationEnabled { get; set; }
}