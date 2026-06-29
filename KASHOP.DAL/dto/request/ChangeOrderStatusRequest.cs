using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

using System.Threading.Tasks;

namespace KASHOP.DAL.dto.request
{
   public class ChangeOrderStatusRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatusEnum Status {  get; set; }
    }
}
