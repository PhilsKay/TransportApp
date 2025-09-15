namespace TransportApp.Domain.DTO.Config;
public record JwtSettings
{
    public string Site { get; set; }
    public string Secret { get; set; }
    public TimeSpan TokenLifeTime { get; set; }
    public string Audience { get; set; }
}
