public class NPCEntity : EntityBase
{
    void Awake()
    {
        Initialize();
        StartCoroutine(NotifyNextFrame());
    }
}

