namespace Shintio.Essentials.Utils;

public class OutputProgress
{
    public delegate void OutputProgressDelegate(OutputProgress progress);

    public event OutputProgressDelegate? ValueUpdated;
    public event OutputProgressDelegate? Started;
    public event OutputProgressDelegate? Stopped;

    private double _value = 0;
    private TaskCompletionSource _source = new();

    public OutputProgress(string title, double max)
    {
        Title = title;
        Max = max;
    }

    public string Title { get; }
    public double Max { get; }

    public double Value
    {
        get => _value;
        set
        {
            _value = value;
            ValueUpdated?.Invoke(this);
        }
    }

    public Task Start()
    {
        Started?.Invoke(this);
        
        return _source.Task;
    }

    public void Stop()
    {
        Stopped?.Invoke(this);
        _source.TrySetResult();
    }
    
    public Task Wait()
    {
        return _source.Task;
    }
}