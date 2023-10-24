namespace KebabMaster.Orders.DTOs;

public class TokenResponse
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }    
}