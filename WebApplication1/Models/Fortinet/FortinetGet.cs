using System;
using System.Collections.Generic;

namespace WebApplication1.Models.Fortinet
{
    public class FortinetGet
    {
        public Meta Meta { get; set; }
        public IList<Object> Objects { get; set; }
    }
}