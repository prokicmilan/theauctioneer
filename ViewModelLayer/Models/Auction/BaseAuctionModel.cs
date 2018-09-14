﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelLayer.Models.Auction
{
    public class BaseAuctionModel : BaseModel
    {

        [Required(ErrorMessage = "You must enter a name for the auction.")]
        [MaxLength(50, ErrorMessage = "Maximum length is 50 characters.")]
        public string Name { get; set; }

        [MaxLength(2048, ErrorMessage = "Maximum length is 2048 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "You must enter a starting price for the auction")]
        public int Price { get; set; }
        
    }
}
