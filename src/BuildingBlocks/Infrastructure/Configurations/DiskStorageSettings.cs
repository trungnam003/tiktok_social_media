namespace Infrastructure.Configurations;
#nullable disable
public class DiskStorageSettings
{
    public string StoragePath { get; set; }
    public static readonly string Video = "videos";
    public static readonly string Image = "images";
    public static readonly string Thumbnail = "thumbnails";
    public static readonly string Audio = "audios";
}