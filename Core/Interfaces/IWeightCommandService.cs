using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weight
{
    public interface IWeightCommandService
    {
        Task StartCommandAsync(int commandNumber);

        Task StartNewOrderAsync(NewOrderData newOrderData);

        //Task<bool> ReviseOrderAsync(uint newWeight, uint newDraft);
        //Task<bool> SetGatePositionAsync(int position);
    }
}
