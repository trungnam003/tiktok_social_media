// See https://aka.ms/new-console-template for more information


using ConsoleApp1;
using Xabe.FFmpeg;

internal class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            const string ffmpegPath = @"C:\ffmpeg\bin\";
            const string videoPath = @"B:\Workspace\project\tiktok_social_media\tiktok_social_media\storage";
            const string audioPath = @"B:\Workspace\project\tiktok_social_media\tiktok_social_media\storage";

            Console.WriteLine("Hello, World!");
            FFmpeg.SetExecutablesPath(ffmpegPath);

            var fileName = "abv.mp4";
            var audioName = "audio.mp3";
            var inputVideo = Path.Combine(videoPath, fileName);
            var inputAudio = Path.Combine(audioPath, audioName);

            // var mediaInfoAudio = await FFmpeg.GetMediaInfo(inputAudio);
            Console.WriteLine($"-i {inputVideo} -i {inputAudio} -map 0:v -map 1:a -c:v copy -shortest");
            // var conversionResult = FFmpeg.Conversions.New()
            //     .AddParameter($"-i {inputVideo} -i {inputAudio} -map 0:v -map 1:a -c:v copy -shortest")
            //     // .AddStream(mediaInfo.Streams.First())
            //     // .AddStream(mediaInfoAudio.Streams.First())
            //     // .AddParameter("-map 0:v -map 1:a -c:v copy -shortest")
            //     // // .AddParameter("-map 1:a")
            //     // // .AddParameter("-c:v copy")
            //     // // .AddParameter("-shortest")
            //     .SetOutput(Path.Combine(videoPath, fileName));

            // await conversionResult.Start();
            var mediaInfo = await FFmpeg.GetMediaInfo(inputVideo).ConfigureAwait(false);

            IVideoStream? videoStream = mediaInfo.VideoStreams.First()?.SetCodec(VideoCodec.png);

            var conversionResult = await FFmpeg.Conversions.New()
                .AddStream(videoStream)
                .ExtractNthFrame(3, (number) => Path.Combine(videoPath, $"frame{number}.png"))
                .Start();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


    }
}