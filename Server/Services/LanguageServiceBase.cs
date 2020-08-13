using JsonRpc.Server;

namespace Lextm.AnsiC.LanguageServer.Services
{
    public class LanguageServiceBase : JsonRpcService
    {
        protected SessionState Session => RequestContext.Features.Get<SessionState>();
    }
}