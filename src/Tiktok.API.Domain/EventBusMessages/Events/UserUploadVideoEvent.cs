namespace Tiktok.API.Domain.EventBusMessages.Events;
#nullable disable
public record UserUploadVideoEvent : EventBase
{
    public string VideoId { get; set; }
    public string VideoName { get; set; }
    
    public bool UseExternalAudio { get; set; }
    
    public string ExternalAudioId { get; set; }
    
}