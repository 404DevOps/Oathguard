using UnityEngine;

public class PooledAutoReturn : MonoBehaviour
{
    private float _timeRemaining;
    private bool _running;

    public void Begin(float lifetime)
    {
        _timeRemaining = lifetime;
        _running = true;
    }

    public void Cancel() => _running = false;

    private void Update()
    {
        if (!_running) return;

        _timeRemaining -= Time.deltaTime;
        if (_timeRemaining <= 0f)
        {
            _running = false;
            Pooled.Release(gameObject);
        }
    }
}
