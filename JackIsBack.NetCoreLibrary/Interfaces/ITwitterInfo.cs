namespace JackIsBack.NetCoreLibrary.Interfaces
{
    public interface ITwitterInfo
    {
        TwitterSecrets Secrets { get; set; }
        TwitterApiLinks ApiLinks { get; set; }
    }
}