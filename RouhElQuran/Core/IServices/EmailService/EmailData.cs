using Microsoft.AspNetCore.Http;

namespace Core.IServices;

public class EmailData
{
    public int Id { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string To { get; set; }
    public IList<IFormFile>? attachment { get; set; }
}