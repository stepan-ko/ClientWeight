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
        Task ReadStatusDataAsync(StatusData statusData);
        Task ReadMiscStatusDataAsync(MiscStatusData miscStatusData);
        Task ReadOrderDataAsync(OrderData orderData);
        Task ReadStaticOrderDataAsync(StaticOrderData staticOrderData);
        Task ReadConfigDataAsync(ConfigData configData);
    }
}
