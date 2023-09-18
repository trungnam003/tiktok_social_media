using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Shared.SeedWork;
#nullable  disable
public class MailRequest
{
    [EmailAddress]
    public string From { get; set; }
    
    [EmailAddress]
    public string To { get; set; }
    
    public IEnumerable<string> ToAddAddress { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }
    
    public IFormFileCollection Attachments { get; set; }
    
}