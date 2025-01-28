using System.Reflection;
using Bormech.Data.Validate;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Bormech.Server.Filters;

public class RegexSchemaFilter:ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.MemberInfo != null)
        {
            var attribute = context.MemberInfo.GetCustomAttribute<CertificationNumberValidationAttribute>();
            if (attribute != null)
            {
                schema.Pattern = CertificationNumberValidationAttribute.GetPattern();
            }
        }
    }
}