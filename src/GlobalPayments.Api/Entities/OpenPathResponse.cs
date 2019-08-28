﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalPayments.Api.Entities
{
    public class OpenPathResponse
    {
        public OpenPathStatusType Status { get; set; }
        public string Message { get; set; }
        public long TransactionId { get; set; }
    }
}
