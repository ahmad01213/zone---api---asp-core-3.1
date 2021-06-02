using System;
using System.Collections.Generic;
using webApp.Models;

namespace webApp.Dtos
{
    public class FoodDetailResponse
    {
        public OptionGroup optionGroup { get; set; }
        public List<Option> options { get; set; }
    }
}
