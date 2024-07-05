using System.ComponentModel.DataAnnotations;

namespace Upload.DTO.FileUpload;

public class AddUploadChunckDto
{
    [Required]
    [MinLength(1)]
    public string TargetFilename { get; set; } = string.Empty;

    public long ChunkNumber { get; set; }

    public int ChunkSize { get; set; }

    [Required]
    [MinLength(1)]
    public string ChunkData { get; set; } = string.Empty;
}
