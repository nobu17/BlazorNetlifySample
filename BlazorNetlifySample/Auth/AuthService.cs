﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNetlifySample.Auth
{
    public interface IAuthService
    {
        Task InitAsync();
        Task<LoginResult> LoginAsync(LoginModel loginModel);
        Task LogoutAsync();
    }

    public class LoginModel
    {
        public string UserID { get; set; }
        public string Password { get; set; }
    }

    public class LoginResult
    {
        public bool IsSuccessful { get; set; }
        public Exception Error { get; set; }
        public string IDToken { get; set; }
    }
}
