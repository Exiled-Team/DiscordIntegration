namespace DiscordIntegration.Dependency
{
    using Newtonsoft.Json;

    public class RemoteCommand
    {
        public ActionType Action { get; set; }
        public object[] Parameters { get; set; }

        [JsonConstructor]
        public RemoteCommand(ActionType action, object data)
        {
            Action = action;
            Parameters = new object[] { data };
        }

        public RemoteCommand(ActionType action, params object[] data)
        {
            Action = action;
            Parameters = data;
        }
    }
}