namespace RouhElQuran.SendEmail
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public DateTime RevokedOn { get; set; }

        public bool IsActive => RevokedOn == null && !IsExpired;
    }
}