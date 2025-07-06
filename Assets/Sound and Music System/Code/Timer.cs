public class Timer
{
    private float maxTime;
    public float currentTime;
    public float Progress => currentTime / maxTime;

    public Timer() { }

    public Timer(float _maxTime)
    {
        maxTime = _maxTime; currentTime = 0;
    }

    public void Tick(float deltaTime)
    {
        if (currentTime < maxTime)
            currentTime += deltaTime;
    }

    public void SetMaxTime(float newMaxTime) { maxTime = newMaxTime; currentTime = 0; }
    public float GetMaxTime() => maxTime;

    public void Reset() => currentTime = 0;

    public float GetCurrentTime() => currentTime;

    public bool IsCompleted() => currentTime >= maxTime;
}
