using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weight
{
    public interface IWeightConnectionService
    {
        Task<bool> ConnectAsync(string host, int port);
        Task DisconnectAsync();
        bool IsConnected { get; }
    }
}
