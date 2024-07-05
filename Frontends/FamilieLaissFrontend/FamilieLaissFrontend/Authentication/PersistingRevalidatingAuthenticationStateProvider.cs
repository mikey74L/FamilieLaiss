using FamilieLaissModels.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Security.Claims;

namespace FamilieLaissFrontend.Authentication;

public class PersistingRevalidatingAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly PersistentComponentState _state;
    private readonly IdentityOptions _options;

    private readonly PersistingComponentStateSubscription _subscription;

    private Task<AuthenticationState>? _authenticationStateTask;

    public PersistingRevalidatingAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,
        PersistentComponentState state,
        IOptions<IdentityOptions> options)
        : base(loggerFactory)
    {
        _scopeFactory = scopeFactory;
        _state = state;
        _options = options.Value;

        AuthenticationStateChanged += OnAuthenticationStateChanged;
        _subscription = state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
    }

    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    protected override async Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        // Get the user manager from a new scope to ensure it fetches fresh data
        await using var scope = _scopeFactory.CreateAsyncScope();
        return ValidateSecurityStampAsync(authenticationState.User);
    }

    private bool ValidateSecurityStampAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity?.IsAuthenticated is false)
        {
            return false;
        }

        return true;
    }

    private void OnAuthenticationStateChanged(Task<AuthenticationState> authenticationStateTask)
    {
        _authenticationStateTask = authenticationStateTask;
    }

    private async Task OnPersistingAsync()
    {
        if (_authenticationStateTask is null)
        {
            throw new UnreachableException($"Authentication state not set in {nameof(RevalidatingServerAuthenticationStateProvider)}.{nameof(OnPersistingAsync)}().");
        }

        var authenticationState = await _authenticationStateTask;
        var principal = authenticationState.User;

        if (principal.Identity?.IsAuthenticated == true)
        {
            var userId = principal.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;
            var name = principal.FindFirst("name")?.Value;
            var email = principal.FindFirst(x => x.Type == "https://familielaiss.de/email")?.Value ?? "";
            var gender = principal.FindFirst(x => x.Type == "https://familielaiss.de/gender")?.Value ?? "";
            var givenName = principal.FindFirst(x => x.Type == "https://familielaiss.de/given_name")?.Value ?? "";
            var familyName = principal.FindFirst(x => x.Type == "https://familielaiss.de/family_name")?.Value ?? "";
            var street = principal.FindFirst(x => x.Type == "https://familielaiss.de/street")?.Value ?? "";
            var houseNumber = principal.FindFirst(x => x.Type == "https://familielaiss.de/hnr")?.Value ?? "";
            var zipCode = principal.FindFirst(x => x.Type == "https://familielaiss.de/zip")?.Value ?? "";
            var city = principal.FindFirst(x => x.Type == "https://familielaiss.de/city")?.Value ?? "";
            var country = principal.FindFirst(x => x.Type == "https://familielaiss.de/country")?.Value ?? "";
            var auth0Id = principal.FindFirst(x => x.Type == "https://familielaiss.de/user_id")?.Value ?? "";

            if (email is not null)
            {
                _state.PersistAsJson(nameof(UserInfo), new UserInfo
                {
                    UserId = userId ?? "",
                    Name = name ?? "",
                    Email = email,
                    Gender = gender,
                    GivenName = givenName,
                    FamilyName = familyName,
                    Street = street,
                    HouseNumber = houseNumber,
                    ZipCode = zipCode,
                    City = city,
                    Country = country,
                    Auth0Id = auth0Id
                });
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        _subscription.Dispose();

        AuthenticationStateChanged -= OnAuthenticationStateChanged;

        base.Dispose(disposing);
    }
}

