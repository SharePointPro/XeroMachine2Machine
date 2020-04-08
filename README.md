# XeroMachine2Machine
Demo of how to access Xero Auth2 without human auth using PCKE flow for desktop applications.

The steps to setup machine 2 machine auth with Xero's new Auth2 model is found here: https://developer.xero.com/documentation/api-guides/machine-2-machine

This console app is my implemetnation of the above.

# Setup
The steps to get the app working are below:
1. Setup an app at https://developer.xero.com/myapps/  
2. Get the first refresh token using xoauth, a human will be asked to login and approve the app (details here: https://developer.xero.com/documentation/api-guides/machine-2-machine)  
3. With the initial refresh token run XeroMachine2Machine.exe init <refreshtoken> (or do this in code Program.cs line 25) This creates and stores an the token details encrypted to settings.dat
4. Any future executions can be done without the parameter
5. Call  _tokenHelper.GetAccessToken() to get the token, if the token is expired, this will refresh it, and save the new details to settings.dat

Once you have an access token you can access the Xero.NetStandard.Oauth Library like normal (https://github.com/XeroAPI/Xero-NetStandard)

    var AccountingApi = new AccountingApi();
    var response = await AccountingApi.GetInvoicesAsync(await _tokenHelper.GetAccessToken(), _tenantId);
            
            
# TenantId
To get the tenant Id, you first need to get the inital access code from step 2. Then you can use the access code to call https://api.xero.com/connections 
to get a full list of your tenants. More info is https://developer.xero.com/documentation/oauth2/pkce-flow or check out TenantHelper.cs
