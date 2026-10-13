namespace Domain.Enums;

public enum LeadStatus
{
    New = 1,                // Новый
    InProgress,             // В работе
    Ndz,                    // НДЗ
    CallBack,               // Перезвонить
    Thinking,               // Думает
    DocumentsRequested,     // Документы запрошены
    DocumentsReceived,      // Документы получены
    Bid,                    // Заявка
    NonTarget,              // Не подходит
    Refusal                 // Отказ
}