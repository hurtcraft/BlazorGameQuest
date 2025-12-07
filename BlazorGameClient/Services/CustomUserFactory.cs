using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Security.Claims;
using System.Text.Json;

public class CustomUserFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
{
    public CustomUserFactory(IAccessTokenProviderAccessor accessor) 
        : base(accessor)
    {
    }

    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(
        RemoteUserAccount account,
        RemoteAuthenticationUserOptions options)
    {
        var user = await base.CreateUserAsync(account, options);
        
        if (user.Identity is ClaimsIdentity identity && identity.IsAuthenticated)
        {
            // Extraire les rôles depuis le token
            var roles = new List<string>();
            
            // Méthode 1 : Depuis le claim "roles" directement
            var rolesClaim = identity.FindFirst("roles");
            if (rolesClaim != null)
            {
                // Si c'est un tableau JSON, le parser
                if (rolesClaim.Value.StartsWith("["))
                {
                    try
                    {
                        var rolesArray = JsonSerializer.Deserialize<string[]>(rolesClaim.Value);
                        if (rolesArray != null)
                        {
                            roles.AddRange(rolesArray);
                        }
                    }
                    catch
                    {
                        // Si ce n'est pas un JSON, traiter comme une valeur simple
                        roles.Add(rolesClaim.Value);
                    }
                }
                else
                {
                    roles.Add(rolesClaim.Value);
                }
            }
            
            // Méthode 2 : Depuis "realm_access.roles" (format Keycloak par défaut)
            var realmAccessClaim = identity.FindFirst("realm_access");
            if (realmAccessClaim != null)
            {
                try
                {
                    var realmAccess = JsonSerializer.Deserialize<JsonElement>(realmAccessClaim.Value);
                    if (realmAccess.TryGetProperty("roles", out var rolesElement))
                    {
                        foreach (var role in rolesElement.EnumerateArray())
                        {
                            var roleValue = role.GetString();
                            if (!string.IsNullOrEmpty(roleValue) && !roles.Contains(roleValue))
                            {
                                roles.Add(roleValue);
                            }
                        }
                    }
                }
                catch
                {
                    // Ignorer si le parsing échoue
                }
            }
            
            // Ajouter les rôles comme claims de type Role
            foreach (var role in roles)
            {
                if (!identity.HasClaim(ClaimTypes.Role, role))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }
            }
        }
        
        return user;
    }
}


