using AccommodationGradingApplication.Grading.Support.Grpc.Protos;
using AccomodationGradingApplication.Abstractions.Messaging;
using AccomodationGradingDomain.Entities;
using AccomodationGradingDomain.Interfaces;
using Grpc.Core;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Rs.Ac.Uns.Ftn.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace AccomodationGradingApplication.Grading.Commands
{
    public sealed class UpdateHostGradingCommandHandler : ICommandHandler<UpdateHostGradingCommand, HostGrading>
    {
        private readonly IHostGradingRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;

        private HostGradingNotificationGrpcService.HostGradingNotificationGrpcServiceClient client;
        public UpdateHostGradingCommandHandler(IHostGradingRepository repository, IConfiguration configuration, IHostEnvironment env)
        {
            _repository = repository;
            _configuration = configuration;
            _env = env;
        }

        public IHostGradingRepository Get_repository()
        {
            return _repository;
        }

        public async Task<HostGrading> Handle(UpdateHostGradingCommand request, CancellationToken cancellationToken)
        {
            HostGrading hg = _repository.GetAsync(request.updateHostGradingDTO.Id).Result;
            if(hg is null)
            {
                throw new Exception("Grade with this id does not exist");
            }
            HostGrading hostGrading = HostGrading.Create(hg.Id, hg.HostEmail.EmailAddress, hg.GuestEmail.EmailAddress, DateTime.Now, request.updateHostGradingDTO.Grade);

            if (_env.EnvironmentName != "Cloud")
            {
                var channel = new Channel(_configuration.GetValue<string>("GrpcDruzina:Notification:Address") + ":" + _configuration.GetValue<int>("GrpcDruzina:Notification:Port"), ChannelCredentials.Insecure);
                client = new HostGradingNotificationGrpcService.HostGradingNotificationGrpcServiceClient(channel);
                MessageResponseProto4 response = await client.hostGradingAsync(new MessageProto4() { Email = hostGrading.HostEmail.EmailAddress, Grade = hostGrading.Grade });
            }
            else
            {
                using var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcDruzina:Notification:Address"), new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
                client = new HostGradingNotificationGrpcService.HostGradingNotificationGrpcServiceClient(channel);
                MessageResponseProto4 response = await client.hostGradingAsync(new MessageProto4() { Email = hostGrading.HostEmail.EmailAddress, Grade = hostGrading.Grade });
            }


            _repository.UpdateAsync(hg.Id, hostGrading);
            return hostGrading;
        }
    }
}
