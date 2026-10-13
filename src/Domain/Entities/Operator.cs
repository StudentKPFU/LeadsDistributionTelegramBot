namespace Domain.Entities;

public class Operator
{
    public int Id { get; private set; }
    public long TelegramId { get; private set; }
    public string Name { get; private set; }

    private Operator() { }

    public Operator(long telegramId, string name)
    {
        if (telegramId <= 0)
            throw new ArgumentException("Telegram ID должен быть больше нуля", nameof(telegramId));
            
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя оператора не может быть пустым", nameof(name));

        TelegramId = telegramId;
        Name = name;
    }
}