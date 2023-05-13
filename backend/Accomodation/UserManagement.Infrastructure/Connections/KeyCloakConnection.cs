using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Abstractions.Connections;
using UserManagement.Application.Dtos;
using UserManagement.Application.Users.Commands;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Connections;

public class KeyCloakConnection : IKeyCloakConnection
{
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    public KeyCloakConnection(IConfiguration config)
    {
        _config = config;
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(_config["Jwt:KeycloakApiUrl"])
        };

    }
    public async Task<bool> DeleteUserAsync(string userId)
    {
        string? accessToken = await GetAccessTokenAsync();
        if (accessToken == null) return false;

        var resourceUrl = "/admin/realms/" + _config["Jwt:RealmName"] + "/users/" + userId;
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var request = new HttpRequestMessage(HttpMethod.Delete, resourceUrl);
        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode) return true;
        return false;
    }
    public async Task<bool> CreateUserAsync(CreateUserCommand createUserCommand)
    {
        string? accessToken = await GetAccessTokenAsync();
        if (accessToken == null) return false;

        var resourceUrl = "/admin/realms/" + _config["Jwt:RealmName"] + "/users";
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
       
        var request = new HttpRequestMessage(HttpMethod.Post, resourceUrl) { 
            Content = JsonContent.Create(new {
                email = createUserCommand.Email,
                enabled = true,
                credentials = new List<object> {
                        new {
                            Type = "password",
                            Value = createUserCommand.Password,
                            Temporary = false,
                        }
                    },
                groups = createUserCommand.Roles
            }
        )};
        var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode) return true;
        return false;
    }
    public async Task<string?> GetUserIdAsync(string email)
    {
        string? accessToken = await GetAccessTokenAsync();
        if (accessToken == null) return null;

        var resourceUrl = "/admin/realms/" + _config["Jwt:RealmName"] + "/users?email=" + email;
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var request = new HttpRequestMessage(HttpMethod.Get, resourceUrl);
       
        var response = await _httpClient.SendAsync(request);
        string responseBody = await response.Content.ReadAsStringAsync();
        JArray responseArray = JArray.Parse(responseBody);

        if (response.IsSuccessStatusCode) return responseArray[0]["id"]?.ToString();
        return null;
    }
    private async Task<string?> GetAccessTokenAsync()
    {
        var nvc = new List<KeyValuePair<string, string>>();
        nvc.Add(new KeyValuePair<string, string>("client_id", _config["Jwt:ApiClientId"]));
        nvc.Add(new KeyValuePair<string, string>("client_secret", _config["Jwt:ApiClientSecret"]));
        nvc.Add(new KeyValuePair<string, string>("grant_type", _config["Jwt:ApiClientGrantType"]));

        var req = new HttpRequestMessage(HttpMethod.Post, "/realms/master/protocol/openid-connect/token") { Content = new FormUrlEncodedContent(nvc) };
        var res = await _httpClient.SendAsync(req);
        if (!res.IsSuccessStatusCode) return null;

        var responseContent = await res.Content.ReadAsStringAsync();
        var json = JObject.Parse(responseContent);
        if (json["access_token"] is null) return null;

        return json["access_token"].Value<string>();
    }
}
