using Domain.Enums;

namespace Domain.Entities;

public class Lead
{
    public int Id { get; private set; }
    
    public string City { get; private set; }
    public int? Age { get; private set; }
    public string Speciality { get; private set; }
    public string Rank { get; private set; }
    public string CriminalRecords { get; private set; }
    
    public LeadStatus Status { get; private set; }
    public int CallsCount { get; private set; }
    public int? CurrentOperatorId { get; private set; }
    public DateTime? NextContactDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime LastUpdatedAt { get; private set; }

    public Operator? CurrentOperator { get; private set; }

    private Lead() { }

    public Lead(string city, int? age, string speciality, string rank, string criminalRecords)
    {
        City = city;
        Age = age;
        Speciality = speciality;
        Rank = rank;
        CriminalRecords = criminalRecords;
        
        Status = LeadStatus.New;
        CallsCount = 0;
        CreatedAt = DateTime.UtcNow;
        LastUpdatedAt = DateTime.UtcNow;
    }

    // Бизнес-процесс: Выдача лида оператору
    public void AssignToOperator(int operatorId)
    {
        if (Status != LeadStatus.New && Status != LeadStatus.Ndz)
            throw new InvalidOperationException($"Нельзя назначить лида в статусе {Status}");

        CurrentOperatorId = operatorId;
        Status = LeadStatus.InProgress;
        NextContactDate = null; // При взятии в работу сбрасываем старые таймеры
        UpdateTimestamp();
    }

    // Бизнес-процесс: Оператор не дозвонился
    public void RegisterNdz()
    {
        if (CurrentOperatorId == null)
            throw new InvalidOperationException("Нельзя поставить НДЗ лиду без оператора");

        CallsCount++;
        Status = LeadStatus.Ndz;
        UpdateTimestamp();
    }

    // Бизнес-процесс: Запланирован перезвон
    public void ScheduleCallBack(DateTime contactTime)
    {
        if (contactTime <= DateTime.UtcNow)
            throw new ArgumentException("Время перезвона должно быть в будущем", nameof(contactTime));

        Status = LeadStatus.CallBack;
        NextContactDate = contactTime;
        UpdateTimestamp();
    }

    // Бизнес-процесс: Перевод в терминальный статус (Отказ, Заявка, Не подходит) или промежуточный (Думает, Документы)
    public void ChangeStatus(LeadStatus newStatus)
    {
        if (Status == newStatus) return;

        Status = newStatus;
        
        // Если статус финальный или не требует таймера, очищаем время перезвона
        if (newStatus == LeadStatus.Refusal || 
            newStatus == LeadStatus.NonTarget || 
            newStatus == LeadStatus.Bid)
        {
            NextContactDate = null;
        }

        UpdateTimestamp();
    }

    private void UpdateTimestamp()
    {
        LastUpdatedAt = DateTime.UtcNow;
    }
}