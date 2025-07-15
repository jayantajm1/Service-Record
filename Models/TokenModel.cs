namespace Service_Record.Models
{
    public class TokenModel
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public TokenModel(string a, string r)
        {
            access_token = a;
            refresh_token = r;
        }


    }
}
