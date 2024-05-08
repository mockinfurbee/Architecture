using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.MySwagger
{
    public class MySwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public MySwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions swagger)
        {
            foreach (var desc in _provider.ApiVersionDescriptions)
            {
                swagger.SwaggerDoc(desc.GroupName, FillDataForApiVersion(desc));
            }
        }

        private static OpenApiInfo FillDataForApiVersion(ApiVersionDescription description)
        {
            var data = new OpenApiInfo()
            {
                Title = "Architecture",
                Version = description.ApiVersion.ToString(),
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Url = new Uri("https://t.me/VladislavGashenko")
                }
            };

            if (description.IsDeprecated)
            {
                data.Description += "<b><br>This API version deprecated.</b>";
            }

            return data;
        }
    }
}
