using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementApplication.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }
}
