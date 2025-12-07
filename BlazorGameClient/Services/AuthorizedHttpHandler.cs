using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

public class AuthorizedHttpHandler : DelegatingHandler
{
    private readonly IAccessTokenProvider _tokenProvider;

    public AuthorizedHttpHandler(IAccessTokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Obtenir le token d'accès
            var tokenResult = await _tokenProvider.RequestAccessToken(
                new AccessTokenRequestOptions
                {
                    Scopes = new[] { "openid", "profile", "roles" }
                });

            if (tokenResult.TryGetToken(out var token))
            {
                // Ajouter le token Bearer à l'en-tête Authorization
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", token.Value);
            }
        }
        catch (AccessTokenNotAvailableException ex)
        {
            // Si le token n'est pas disponible, rediriger vers la page de connexion
            ex.Redirect();
            throw;
        }
        catch (Exception)
        {
            // Erreur silencieuse lors de la récupération du token
        }

        return await base.SendAsync(request, cancellationToken);
    }
}

