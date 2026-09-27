using EQApp.Models;

namespace EQApp.Data;

public static class EQTestData
{
    public static List<EQQuestion> GetQuestions()
    {
        var questions = new List<EQQuestion>();

        // Вопрос 1 - Самоосознание
        var q1 = new EQQuestion(1, "Когда я испытываю сильную эмоцию, я:", EQCategory.SelfAwareness);
        q1.AddOption("Сразу понимаю, что именно чувствую и почему", 10);
        q1.AddOption("Понимаю, но не всегда могу объяснить причину", 7);
        q1.AddOption("Замечаю эмоцию, но не сразу понимаю причину", 4);
        q1.AddOption("Часто не замечаю, пока эмоция не пройдёт", 2);
        questions.Add(q1);

        // Вопрос 2 - Саморегуляция
        var q2 = new EQQuestion(2, "В стрессовой ситуации я обычно:", EQCategory.SelfRegulation);
        q2.AddOption("Сохраняю спокойствие и контролирую реакцию", 10);
        q2.AddOption("Стараюсь успокоиться и найти решение", 7);
        q2.AddOption("Реагирую эмоционально, но быстро прихожу в норму", 4);
        q2.AddOption("Теряю контроль над эмоциями", 2);
        questions.Add(q2);

        // Вопрос 3 - Эмпатия
        var q3 = new EQQuestion(3, "Когда я вижу, что друг расстроен, я:", EQCategory.Empathy);
        q3.AddOption("Чувствую его состояние и знаю, как поддержать", 10);
        q3.AddOption("Понимаю, что он расстроен, и предлагаю помощь", 7);
        q3.AddOption("Замечаю, но не всегда знаю, что делать", 4);
        q3.AddOption("Не всегда замечаю, если не говорят прямо", 2);
        questions.Add(q3);

        // Вопрос 4 - Мотивация
        var q4 = new EQQuestion(4, "Когда у меня не получается что-то важное, я:", EQCategory.Motivation);
        q4.AddOption("Воспринимаю как опыт и продолжаю попытки", 10);
        q4.AddOption("Расстраиваюсь, но быстро беру себя в руки", 7);
        q4.AddOption("Сильно расстраиваюсь и теряю уверенность", 4);
        q4.AddOption("Бросаю дело, если не получается сразу", 2);
        questions.Add(q4);

        // Вопрос 5 - Социальные навыки
        var q5 = new EQQuestion(5, "В конфликтной ситуации я обычно:", EQCategory.SocialSkills);
        q5.AddOption("Ищу компромисс, слышу обе стороны", 10);
        q5.AddOption("Стараюсь спокойно обсудить проблему", 7);
        q5.AddOption("Отстаиваю свою позицию, не слушая других", 4);
        q5.AddOption("Ухожу от конфликта или агрессирую", 2);
        questions.Add(q5);

        // Вопрос 6 - Самоосознание
        var q6 = new EQQuestion(6, "Я знаю свои сильные и слабые стороны:", EQCategory.SelfAwareness);
        q6.AddOption("Чётко осознаю и работаю над развитием", 10);
        q6.AddOption("В целом понимаю, но не всегда", 7);
        q6.AddOption("Затрудняюсь оценить себя объективно", 4);
        q6.AddOption("Не задумывался об этом", 2);
        questions.Add(q6);

        // Вопрос 7 - Саморегуляция
        var q7 = new EQQuestion(7, "Когда я злюсь, я:", EQCategory.SelfRegulation);
        q7.AddOption("Могу выразить гнев конструктивно, без агрессии", 10);
        q7.AddOption("Сдерживаюсь, но потом переживаю", 7);
        q7.AddOption("Часто срываюсь на других", 4);
        q7.AddOption("Не могу контролировать гнев", 2);
        questions.Add(q7);

        // Вопрос 8 - Эмпатия
        var q8 = new EQQuestion(8, "Я могу понять чувства человека, даже если он о них не говорит:", EQCategory.Empathy);
        q8.AddOption("Всегда, по выражению лица и языку тела", 10);
        q8.AddOption("Часто могу понять", 7);
        q8.AddOption("Иногда угадываю", 4);
        q8.AddOption("Редко, мне нужны прямые слова", 2);
        questions.Add(q8);

        // Вопрос 9 - Мотивация
        var q9 = new EQQuestion(9, "Я ставлю себе цели и достигаю их:", EQCategory.Motivation);
        q9.AddOption("Всегда, даже если сложно", 10);
        q9.AddOption("Чаще всего достигаю", 7);
        q9.AddOption("Иногда бросаю на полпути", 4);
        q9.AddOption("Редко довожу до конца", 2);
        questions.Add(q9);

        // Вопрос 10 - Социальные навыки
        var q10 = new EQQuestion(10, "В компании незнакомых людей я:", EQCategory.SocialSkills);
        q10.AddOption("Легко нахожу общий язык и знакомлюсь", 10);
        q10.AddOption("Могу поддержать разговор", 7);
        q10.AddOption("Чувствую себя неловко", 4);
        q10.AddOption("Избегаю общения", 2);
        questions.Add(q10);

        return questions;
    }
}