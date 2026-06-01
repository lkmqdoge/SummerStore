namespace SummerStore.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app
            .MapGroup("/api/auth");

        // group.MapPost("login");
    }

    public static async Task Register()
    {

    }
    public static async Task Login()
    {

    }
}

