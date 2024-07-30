using System;
using System.Collections.Generic;
using System.IO;

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

    public class Program
    {
        public static List<Product> readProdMasterList(string filePath)
        {
            var productList = new List<Product>();
            var lines = File.ReadAllLines(filePath);

            // Skip the header line
            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split('|');
                if (parts.Length < 11) continue; // Ensure there are enough parts to parse

                var product = new Product
                {
                    Barcode = parts[0],
                    Name = parts[1],
                    Description = parts[2],
                    ReleaseDate = DateTime.TryParse(parts[3], out DateTime releaseDate) ? releaseDate : DateTime.MinValue,
                    Feature = parts[4],
                    Price = double.TryParse(parts[5], out double price) ? price : 0.0,
                    Quantity = int.TryParse(parts[6], out int quantity) ? quantity : 0,
                    DiscountType = parts[7],
                    QuantitySold = int.TryParse(parts[8], out int quantitySold) ? quantitySold : 0,
                    Weight = double.TryParse(parts[9], out double weight) ? weight : 0.0,
                    PackagingMaterial = parts[10]
                };

                productList.Add(product);
            }
            return productList;
        }

        public static void generateInfoForMarketing(List<Product> products)
        {
            using (StreamWriter sw = new StreamWriter("Marketing.txt"))
            {
                foreach (var product in products)
                {
                    sw.WriteLine(product.FormatForMarketing());
                }
            }
        }

        public static void generateInfoForSales(List<Product> products)
        {
            using (StreamWriter sw = new StreamWriter("Sales.txt"))
            {
                foreach (var product in products)
                {
                    sw.WriteLine(product.FormatForSales());
                }
            }
        }

        public static void generateInfoForLogistics(List<Product> products)
        {
            using (StreamWriter sw = new StreamWriter("Logistics.txt"))
            {
                foreach (var product in products)
                {
                    sw.WriteLine(product.FormatForLogistics());
                }
            }
        }

        public delegate void GenerateInfo(List<Product> products);

        public static void Main(string[] args)
        {
            string filePath = "../ProdMasterlist.txt";
            var products = readProdMasterList(filePath);

            GenerateInfo generateInfo = generateInfoForMarketing;
            generateInfo += generateInfoForSales;
            generateInfo += generateInfoForLogistics;

            generateInfo(products);

            Console.WriteLine("Files have been generated.");
        }
    }
}
