﻿using System;
using System.Collections.Generic;
using System.Text;

namespace XeroMachine2Machine.Model
{
    public class RefreshTokenResponse
    {
        public string id_token { get; set; }
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }

        public DateTime expire_date { get; set; }
    }
}
