using System;
using System.Collections.Generic;
using System.Text;

namespace EQApp.Interfaces;

// Интерфейс - контракт, который обязывает реализовать метод GenerateReport
public interface IReportGenerator
{
    string GenerateReport();
}

// Интерфейс для сортируемых объектов
public interface ISortable
{
    int CompareTo(object? obj);
}