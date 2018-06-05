using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.FrontWeb.Models
{
    public class HouseSearchViewModel
    {
        public RegionDTO[] regions { get; set; }
        public HouseDTO[] houses { get; set; }
    }
}