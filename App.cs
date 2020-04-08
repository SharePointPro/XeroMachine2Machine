using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XeroMachine2Machine.Helpers;
using Xero.NetStandard.OAuth2.Api;

namespace XeroMachine2Machine
{
    public class App
    {
        private readonly IConfigurationRoot _config;
        private readonly ILogger<App> _logger;
        private readonly string _clientId;
        private readonly TokenHelper _tokenHelper;
        private readonly string _tenantId;

        public App(IConfigurationRoot config, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<App>();
            _config = config;
            _clientId = _config.GetSection("clientId").Get<string>();
            _tenantId = _config.GetSection("tenantId").Get<string>();
            _tokenHelper = new TokenHelper(_clientId);
        }

        public async Task Run(string[] args)
        {
            //Handle Cli Commands
            if (args.Length == 2 && CliCommands.CLI_COMMANDS.Contains(args[0]))
            {
                switch (args[0])
                {
                    case CliCommands.GET_TENANTS:
                        //Get Tenant ID and save to Text file
                        TenantHelper.GetTenants(args[1]).Wait();
                        return;
                    case CliCommands.INIT:
                        //Initial setup of refresh file
                        var response = await _tokenHelper.Refresh(args[1]);
                        FileHelper.WriteFile(response);
                        //Allow to fall through
                        break;
                }
            }
            else if (args.Length == 2)
            {
                Console.WriteLine($"Possible commands are {string.Join(", ", CliCommands.CLI_COMMANDS)}");
                return;
            }


        await Execute();
        }


        private async Task Execute()
        {
            var AccountingApi = new AccountingApi();
            var response = await AccountingApi.GetInvoicesAsync(await _tokenHelper.GetAccessToken(), _tenantId);
        }

    }
}
