using EQApp.Interfaces;  

namespace EQApp.Delegates;

// для события завершения упражнения 
public delegate void ExerciseCompletedHandler(ExerciseResult result);

// для фильтрации (возвращает true/false)
public delegate bool FilterDelegate<T>(T item);