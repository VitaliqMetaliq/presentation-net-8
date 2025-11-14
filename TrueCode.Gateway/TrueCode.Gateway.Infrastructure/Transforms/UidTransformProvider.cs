using System.Net;
using TrueCode.Gateway.Application;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace TrueCode.Gateway.Infrastructure.Transforms
{
    public class UidTransformProvider : ITransformProvider
    {
        public void Apply(TransformBuilderContext context)
        {
            if (context.Route.RouteId == "finance_routes")
            {
                context.AddRequestTransform(transformContext =>
                {
                    var uid = transformContext.HttpContext.User
                        .FindFirst(ApplicationConstants.Auth.UidClaim)?.Value;

                    if (!string.IsNullOrEmpty(uid))
                    {
                        transformContext.ProxyRequest.Headers.Add(ApplicationConstants.Auth.UserIdHeader, uid);
                    }

                    // transformContext.ProxyRequest.Version = HttpVersion.Version20;

                    return ValueTask.CompletedTask;
                });
            }
        }

        public void ValidateCluster(TransformClusterValidationContext context)
        {
            // noop
        }

        public void ValidateRoute(TransformRouteValidationContext context)
        {
            // noop
        }
    }
}
