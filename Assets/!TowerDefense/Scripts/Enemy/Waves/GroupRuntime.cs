public class GroupRuntime
{
    public Group Data { get; }
    public int SpawnedCount { get; private set; }
    public float Timer { get; private set; }

    public bool IsCompleted => SpawnedCount >= Data.EnemyCount;

    public GroupRuntime(Group data)
    {
        Data = data;
        Timer = 0f;
        SpawnedCount = 0;
    }

    public bool CanSpawn(float deltaTime)
    {
        if (IsCompleted)
            return false;

        Timer += deltaTime;

        if (Timer >= Data.TimeBtwSpawn)
        {
            Timer -= Data.TimeBtwSpawn;
            SpawnedCount++;
            return true;
        }

        return false;
    }
}
