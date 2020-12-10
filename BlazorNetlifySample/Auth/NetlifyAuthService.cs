using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorNetlifySample.Auth
{
    public class NetlifyAuthService : IAuthService
    {
        private readonly IJSRuntime _JSRuntime;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public NetlifyAuthService(AuthenticationStateProvider authenticationStateProvider, IJSRuntime jSRuntime)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _JSRuntime = jSRuntime;
            InitAsync();
        }

        public async Task InitAsync()
        {
            await _JSRuntime.InvokeVoidAsync("initAuth");
            var user = await _JSRuntime.InvokeAsync<NetlifyLoginResult>("getCurrentUser");
            if (user == null)
            {
                await LogoutAsync();
            }
        }

        public async Task<LoginResult> LoginAsync(LoginModel loginModel)
        {
            try
            {
                var result = await _JSRuntime.InvokeAsync<NetlifyLoginResult>("loginAsync", new object[] { loginModel.UserID, loginModel.Password }).ConfigureAwait(false);
                // トークンを取得
                var res = new LoginResult()
                {
                    IsSuccessful = true,
                    IDToken = result.AccessToken
                };
                await ((SpaAuthticateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.UserID, res.IDToken);
                return res;
            }
            catch (Exception e)
            {
                return new LoginResult()
                {
                    IsSuccessful = false,
                    Error = e
                };
            }
        }

        public async Task LogoutAsync()
        {
            await _JSRuntime.InvokeAsync<string>("logoutAsync", new object[] { }).ConfigureAwait(false);
            await ((SpaAuthticateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class NetlifyLoginResult
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
    }

}
