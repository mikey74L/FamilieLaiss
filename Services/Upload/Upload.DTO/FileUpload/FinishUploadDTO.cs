using System.ComponentModel.DataAnnotations;

namespace Upload.DTO.FileUpload;

public class FinishUploadDto
{
    [Required]
    [MinLength(1)]
    public string TargetFilename { get; set; } = string.Empty;

    [Required]
    [StringLength(400, MinimumLength = 1)]
    public string OriginalFilename { get; set; } = string.Empty;

    public long LastChunkNumber { get; set; }
}
