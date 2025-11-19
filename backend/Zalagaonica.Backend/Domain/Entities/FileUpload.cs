using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class FileUpload
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string Base64 { get; set; } = string.Empty;

        public Guid? ArticleId { get; set; }
        public Article? Article { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
