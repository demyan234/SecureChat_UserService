using ChatService.Proto;
using Grpc.Core;
using SecureChatUserMicroService.Application.Common.Interfaces.IGrpcClients;

namespace SecureChatUserMicroService.Application.GrpcServices.Clients
{
    public class ChatGrpcClientService(ChatGrpcService.ChatGrpcServiceClient chatGrpcServiceClient)
        : IChatGrpcClient
    {
        private readonly ChatGrpcService.ChatGrpcServiceClient _chatGrpcServiceClient =
            chatGrpcServiceClient ?? throw new ArgumentNullException(nameof(chatGrpcServiceClient));

        public async Task<AddUserResponse> AddUser(AddUserRequest request)
        {
            try
            {
                var response = await _chatGrpcServiceClient.AddUserAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Cancelled, ex.Message));
            }
        }

        public async Task<RemoveUserResponse> RemoveUser(RemoveUserRequest request)
        {
            try
            {
                var response = await _chatGrpcServiceClient.RemoveUserAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Cancelled, ex.Message));
            }
        }
    }
}