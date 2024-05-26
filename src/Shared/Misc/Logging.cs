using System;
namespace Svk.Shared.Misc;

public class Logging
{
    public IEnumerable<UpdateAction>? UpdateActions { get; set; }
   
    public class UpdateAction
    {
        public enum Actions
        {
            GET, POST, PUT
        }

        //TODO: change to user object
        public string? User { get; set; }
        public Actions Action { get; set; }
        public DateTime Timestamp { get; set; }

    }
}

