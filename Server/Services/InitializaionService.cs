﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using JsonRpc.Contracts;
using JsonRpc.Messages;
using JsonRpc.Server;
using LanguageServer.VsCode.Contracts;
using Newtonsoft.Json.Linq;

namespace Lextm.AnsiC.LanguageServer.Services
{
    public class InitializaionService : LanguageServiceBase
    {
        [JsonRpcMethod(AllowExtensionData = true)]
        public InitializeResult Initialize(int processId, Uri rootUri, ClientCapabilities capabilities,
            JToken initializationOptions = null, string trace = null)
        {
            Session.Project.WorkspaceRoot = new Uri($"file://{rootUri.LocalPath}").LocalPath;
            // inform the language server client of server capabilities.
            return new InitializeResult(new ServerCapabilities
            {
                //HoverProvider = true,
                //SignatureHelpProvider = new SignatureHelpOptions(new[] {'(', ')'}),
                CompletionProvider = new CompletionOptions(true, new[]{'/'}),
                TextDocumentSync = new TextDocumentSyncOptions
                {
                    OpenClose = true,
                    WillSave = true,
                    Change = TextDocumentSyncKind.Incremental
                }
            });
        }

        [JsonRpcMethod(IsNotification = true)]
        public async Task Initialized()
        {
            await Session.Client.Window.ShowMessage(MessageType.Info, $"Hello from language server {Process.GetCurrentProcess().Id}.");
        }

        [JsonRpcMethod]
        public void Shutdown()
        {

        }

        [JsonRpcMethod(IsNotification = true)]
        public void Exit()
        {
            Session.StopServer();
        }

        [JsonRpcMethod("$/cancelRequest", IsNotification = true)]
        public void CancelRequest(MessageId id)
        {
            RequestContext.Features.Get<IRequestCancellationFeature>()?.TryCancel(id);
        }
    }
}