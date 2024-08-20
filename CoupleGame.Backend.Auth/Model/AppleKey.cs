namespace CoupleGame.Backend.Auth.Model
{
    public class AppleKey
    {
        public string Kty { get; set; }
        public string Kid { get; set; }
        public string Use { get; set; }
        public string Alg { get; set; }
        public List<string> X5c { get; set; }
    }
}
