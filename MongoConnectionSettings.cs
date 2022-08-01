class MongoConnectionSettings
{
    public const string position = "Mongo";
    public string Username { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string Host { get; set; } = String.Empty;
    public int Port { get; set; } = 27017;

    public string MONGO_URL
    {
        get => $"mongodb://{Username}:{Password}@{Host}:{Port}";
    }
}

