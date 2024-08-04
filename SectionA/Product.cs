using System;

namespace SectionA
{
    public class Product
    {
        public string Barcode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string Feature { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string DiscountType { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public double Weight { get; set; }
        public string PackagingMaterial { get; set; } = string.Empty;
        public double DiscountPrice { get; set; } = 0.0;

        public string FormatForMarketing()
        {
            return $"{Name} - {Description} - {Price} - {Feature} - {ReleaseDate:dd/MM/yyyy}";
        }

        public string FormatForSales()
        {
            return $"{Barcode} - {Name} - {Description} - {Price} - {Quantity} - {QuantitySold} - {DiscountType}";
        }

        public string FormatForLogistics()
        {
            return $"{Barcode} - {Name} - {Weight} - {PackagingMaterial}";
        }
    }
}
