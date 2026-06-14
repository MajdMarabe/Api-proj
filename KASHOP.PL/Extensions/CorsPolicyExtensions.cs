namespace KASHOP.PL.Extensions
{
    public static class CorsPolicyExtensions
    {
        public const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public static IServiceCollection AddCorsPolicy(this IServiceCollection Services)
        {
            Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin()
                                            .AllowAnyMethod()
                                            .AllowAnyHeader();
                                  });
            });
            return Services;
        }

    }
}
