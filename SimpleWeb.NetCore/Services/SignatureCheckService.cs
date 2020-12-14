using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SimpleWeb.NetCore.Services
{
    public class SignatureCheckService
    {
        private readonly IConfiguration _config;

        public SignatureCheckService(IConfiguration configs)
        {
            _config = configs;
        }

        public bool Verify(IHeaderDictionary headers, object model)
        {
            
            if (!headers.TryGetValue("timestamp", out StringValues timestamp))
            {
                return false;
            }

            if (!int.TryParse(timestamp, out int seconds))
            {
                return false;
            }

            if (DateTimeOffset.Now.ToUnixTimeSeconds() - seconds > 600)
            {
                return false;
            }


            if (!headers.TryGetValue("signature", out StringValues signature))
            {
                return false;
            }

            List<string> args = new List<string>();

            args.Add(timestamp.ToString());
            args.Add(_config["AppSecret"]);
            args.Add(model.ToString());

            var sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(string.Join(".", args)));
            return Convert.ToBase64String(hash) == signature;
        }
    }
}
