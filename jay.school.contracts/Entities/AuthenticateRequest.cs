

namespace jay.school.contracts.Entities
{
    public class AuthenticateRequest

    {

        public string channel { get; set; }
        public dynamic uid { get; set; }
        public uint expiredTs { get; set; } = 0;
        public int role { get; set; } = 1;

    }

    public class AuthenticateResponse

    {

        public string channel { get; set; }
        public dynamic uid { get; set; }
        public string token { get; set; }

    }
}
