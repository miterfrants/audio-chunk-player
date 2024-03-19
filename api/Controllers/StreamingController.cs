using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("audio")]
public class StreamingController : ControllerBase
{
    private static FileStream audioFileStream;

    public StreamingController(
    )
    {
        audioFileStream = new FileStream("sample.mp3", FileMode.Open, FileAccess.Read);
    }

    private const int ChunkSize = 1024 * 1024; // 设置每个分段的大小

    [HttpGet("chunk")]
    public IActionResult GetAudioChunk([FromQuery] int chunkIndex)
    {
        byte[] buffer = new byte[ChunkSize];
        long startPosition = chunkIndex * ChunkSize;
        audioFileStream.Seek(startPosition, SeekOrigin.Begin);
        int bytesRead = audioFileStream.Read(buffer, 0, buffer.Length);

        if (bytesRead > 0)
        {
            return File(buffer, "audio/mpeg"); // Return the audio chunk
        }
        return NotFound();
    }
}