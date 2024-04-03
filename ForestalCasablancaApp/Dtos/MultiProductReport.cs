﻿#nullable enable
namespace BosquesNalcahue.Dtos
{
    public class MultiProductReport : BaseReport
    {
        public int FinalQuantity { get; set; }
        public double FinalVolume { get; set; }
        public List<Product>? Products { get; set; }
    }
}
