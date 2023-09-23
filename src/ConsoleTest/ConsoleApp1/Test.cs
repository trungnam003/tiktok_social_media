using Xabe.FFmpeg;

namespace ConsoleApp1;

public class Test
{
    public static async Task Test2()
    {
        const string ffmpegPath = @"C:\ffmpeg\bin\";
        const string storagePath = @"B:\Workspace\project\tiktok_social_media\tiktok_social_media\storage\videos";
        var path = Path.Combine(storagePath, "video2.mp4");
        var a = await FFmpeg.GetMediaInfo(path);
        var o = Path.Combine(storagePath, $"output2{Guid.NewGuid().ToString()}.png");
        IConversion conversion = await FFmpeg.Conversions.FromSnippet.Snapshot(path, o, TimeSpan.FromSeconds(1));
        IConversionResult result = await conversion.Start();
        Console.WriteLine(FFmpeg.Conversions.GetHashCode());

    }
    
    public static async Task Test3()
    {
        const string ffmpegPath = @"C:\ffmpeg\bin\";
        const string storagePath = @"B:\Workspace\project\tiktok_social_media\tiktok_social_media\storage\videos";
        var path = Path.Combine(storagePath, "video.mp4");
        var a = await FFmpeg.GetMediaInfo(path);
        var o = Path.Combine(storagePath, "output.png");
        IConversion conversion = await FFmpeg.Conversions.FromSnippet.Snapshot(path, o, TimeSpan.FromSeconds(1));
        IConversionResult result = await conversion.Start();
        Console.WriteLine(FFmpeg.Conversions.GetHashCode());

    }
}