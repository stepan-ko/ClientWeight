using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weight.Data;

namespace Weight
{
    public interface IWeightDataReader
    {
        Task<StatusData> ReadStatusDataAsync();
        Task<MiscStatusData> ReadMiscStatusDataAsync();
        Task<OrderData> ReadOrderDataAsync();
        Task<StaticOrderData> ReadStaticOrderDataAsync();
        Task<ConfigData> ReadConfigDataAsync();
    }
}
