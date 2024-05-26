using Svk.Domain.Common;

namespace Svk.Domain;

public class LogEntry : Entity
{
    private Actions _action = default!;

    private String _content = default!;

    private DateTime _timestamp = default!;

    //TODO make reference to user
    private String _user = default!;

    private LogEntry()
    {
    }

    public LogEntry(Actions action, string user, DateTime timestamp, string content)
    {
        Action = action;
        User = user;
        Timestamp = timestamp;
        Content = content;
    }

    public Actions Action
    {
        get => _action;
        set => _action = value;
    }

    public String User
    {
        get => _user;
        set => _user = value;
    }

    public DateTime Timestamp
    {
        get => _timestamp;
        set => _timestamp = value;
    }

    public String Content
    {
        get => _content;
        set => _content = value;
    }
}