﻿using IdentityModel.Client;
using IdentityServer.Client1.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace IdentityServer.Client1.Controllers
{
    public class DummyController : Controller
    {
        private readonly IConfiguration _configuration;

        public DummyController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient client = new HttpClient();
            var discoveryEndPoint = await client.GetDiscoveryDocumentAsync("https://localhost:44383"); //run discoveryEndpoint
            if (discoveryEndPoint.IsError)
            {
                return View();
            }
            else
            {
                ClientCredentialsTokenRequest request = new ClientCredentialsTokenRequest();
                request.ClientId = _configuration["Client:ClientId"];
                request.ClientSecret = _configuration["Client:ClientSecret"];
                request.Address = discoveryEndPoint.TokenEndpoint;
                 var token = await client.RequestClientCredentialsTokenAsync(request);
                if (token.IsError)
                {
                    //some logging
                    return View();
                }
                else
                {
                    client.SetBearerToken(token.AccessToken); // set access token just one time.
                    var response = await client.GetAsync("https://localhost:44321/api/JunkData/GetJunkData");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();   // reading content as json
                        var model = JsonConvert.DeserializeObject<List<ResponseModel>>(content);
                        return View(model);
                    }
                    else
                    {
                        //logging
                        return View();
                    }
                    
                }
                
            }
        }
    }
}
