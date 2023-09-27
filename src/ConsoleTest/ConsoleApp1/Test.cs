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

//

// try
// {
//     const string ffmpegPath = @"C:\ffmpeg\bin\";
//     const string videoPath = @"B:\Workspace\project\tiktok_social_media\tiktok_social_media\storage";
//     const string audioPath = @"B:\Workspace\project\tiktok_social_media\tiktok_social_media\storage";
//
//     Console.WriteLine("Hello, World!");
//     FFmpeg.SetExecutablesPath(ffmpegPath);
//
//     var fileName = "abv.mp4";
//     var audioName = "audio.mp3";
//     var inputVideo = Path.Combine(videoPath, fileName);
//     var inputAudio = Path.Combine(audioPath, audioName);
//
//     // var mediaInfoAudio = await FFmpeg.GetMediaInfo(inputAudio);
//     Console.WriteLine($"-i {inputVideo} -i {inputAudio} -map 0:v -map 1:a -c:v copy -shortest");
//     // var conversionResult = FFmpeg.Conversions.New()
//     //     .AddParameter($"-i {inputVideo} -i {inputAudio} -map 0:v -map 1:a -c:v copy -shortest")
//     //     // .AddStream(mediaInfo.Streams.First())
//     //     // .AddStream(mediaInfoAudio.Streams.First())
//     //     // .AddParameter("-map 0:v -map 1:a -c:v copy -shortest")
//     //     // // .AddParameter("-map 1:a")
//     //     // // .AddParameter("-c:v copy")
//     //     // // .AddParameter("-shortest")
//     //     .SetOutput(Path.Combine(videoPath, fileName));
//
//     // await conversionResult.Start();
//     var mediaInfo = await FFmpeg.GetMediaInfo(inputVideo).ConfigureAwait(false);
//
//     IVideoStream? videoStream = mediaInfo.VideoStreams.First()?.SetCodec(VideoCodec.png);
//
//     var conversionResult = await FFmpeg.Conversions.New()
//         .AddStream(videoStream)
//         .ExtractNthFrame(3, (number) => Path.Combine(videoPath, $"frame{number}.png"))
//         .Start();
// }
// catch (Exception e)
// {
//     Console.WriteLine(e);
//     throw;
// }