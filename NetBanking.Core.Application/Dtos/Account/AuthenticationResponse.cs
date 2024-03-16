﻿namespace NetBanking.Core.Application.Dtos.Account
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string UseName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public string PhoneNumber { get; set; }
        public string Identification { get; set; }
        public bool UserStatus { get; set; }
        public List<string> Roles { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }

    }
}
