using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService1
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "My name is cuilishen,Hello " + request.Name
            });
        }

        public override async Task LotsOfReplies(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            int count = 1;
            while (count <= 5)   // !context.CancellationToken.IsCancellationRequested && 
            {
                await responseStream.WriteAsync(new HelloReply
                {
                    Message = $"Hello {count}" + request.Name
                });
                count++;
                await Task.Delay(2000);
            }

            // return Task.CompletedTask;
        }

        public override async Task PingPongHello(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            try
            {
                while (!context.CancellationToken.IsCancellationRequested)
                {
                    var asyncRequests = requestStream.ReadAllAsync();
                    // �ͻ���������"��ƹ��"
                    await foreach (var req in asyncRequests)
                    {
                        var send = Reverse(req.Name);
                        await responseStream.WriteAsync(new HelloReply
                        {
                            Message = send,
                            Id = req.Id + 1
                        });
                        Debug.WriteLine($"��{req.Id}�غϣ�������յ� {req.Name}����ʼ��{req.Id + 1}�غ�,����˻ط� {send}");
                    }
                }
            }
            catch (RpcException ex)
            {
                System.Diagnostics.Debug.WriteLine($"{ex.Message}");
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine($"{ex.Message}");
            }
        }

        static string Reverse(string str)
        {
            char[] arr = str.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }



    }
}
