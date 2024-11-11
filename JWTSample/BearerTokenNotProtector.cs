using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;

namespace JWTSample;

public record TicketAndPurpose(AuthenticationTicket Data, string? Purpose);

public class BearerTokenNotProtector : ISecureDataFormat<AuthenticationTicket>
{
    public string Protect(AuthenticationTicket data)
    {
        return JsonSerializer.Serialize(data, new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.Preserve,
        });
    }

    public string Protect(AuthenticationTicket data, string? purpose)
    {
        return JsonSerializer.Serialize(new TicketAndPurpose(data, purpose), new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.Preserve,
        });
    }

    public AuthenticationTicket? Unprotect(string? protectedText)
    {
        if (protectedText == null)
            return null;
        
        return JsonSerializer.Deserialize<AuthenticationTicket?>(protectedText);
    }

    public AuthenticationTicket? Unprotect(string? protectedText, string? purpose)
    {
        if (protectedText == null)
            return null;
        
        return JsonSerializer.Deserialize<TicketAndPurpose?>(protectedText)?.Data;
    }
}