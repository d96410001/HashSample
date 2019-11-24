using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProxy.Models
{
    public class HashOptions
    {
        public string AESKey { get; set; }
        public string AES_IV { get; set; }
        public string ApiEndPoint { get; set; }

    }
}
