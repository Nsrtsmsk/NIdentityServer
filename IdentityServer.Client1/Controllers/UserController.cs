using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;

namespace IdentityServer.Client1.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
		{
			
			return View();
		}

        public async Task LogOut()
        {

			await HttpContext.SignOutAsync("Cookies");// Bu uygulama içerisinde sıgnOut olur
            await HttpContext.SignOutAsync("oidc");// IdentityServer da SignOut olur.


        }
		public async Task<IActionResult> GetRefreshToken()
		{
			HttpClient httpClient = new HttpClient();
            DiscoveryDocumentResponse discoverData = await httpClient.GetDiscoveryDocumentAsync("https://localhost:44383");
            if (discoverData.IsError)
			{
				// some logging or error page direction
			}
			var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
			RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest();
            refreshTokenRequest.ClientId = _configuration["Client1Mvc:ClientId"];
			refreshTokenRequest.ClientSecret = _configuration["Client1Mvc:ClientSecret"];
			refreshTokenRequest.RefreshToken = refreshToken;
			refreshTokenRequest.Address = discoverData.TokenEndpoint;

            TokenResponse token = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);
			if (token.IsError)
			{
				//some logging or error page direction
			}
            List<AuthenticationToken> tokens = new List<AuthenticationToken>() 
			{ 
				new AuthenticationToken{Name=OpenIdConnectParameterNames.IdToken,Value=token.IdentityToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)},

            };
			AuthenticateResult authenticationResult = await HttpContext.AuthenticateAsync();
            AuthenticationProperties? properties = authenticationResult.Properties;
			properties.StoreTokens(tokens);
			await HttpContext.SignInAsync("Cookies", authenticationResult.Principal, properties);


            return RedirectToAction("Index");	
		}
		[Authorize(Roles ="admin")]
		public IActionResult AdminAction()
		{ 

			return View();
		}
        [Authorize(Roles = "customer")]
        public IActionResult CustomerAction()
        {

            return View();
        }

    }
}
