public class Buff 
{
    public string ID;
    public BuffType Type;
    public string Characteristic;

    public float Value;

    public float Duration;
    public float TimeLeft;
    public bool IsExpired => Duration > 0 && TimeLeft <= 0;

    public Buff(
        string id,
        string characteristic,
        BuffType buffType,
        float value,
        float duration = 0)
    {
        ID = id;
        Type = buffType;
        Characteristic = characteristic;
        Value = value;
        Duration = duration;
        TimeLeft = duration;
    }

    public void Tick(float deltaTime)
    {
        if (Duration > 0) 
            TimeLeft -= deltaTime;
    }
}
