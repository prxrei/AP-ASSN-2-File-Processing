using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SectionA; // Add this to reference the SectionA namespace

public enum DiscountType
{
    A,
    B,
    C
}

public class Program
{
    public static void calculateDiscount(List<Product> products)
    {
        foreach (var product in products)
        {
            switch (Enum.Parse<DiscountType>(product.DiscountType))
            {
                case DiscountType.A:
                    product.DiscountPrice = Math.Round(product.Price * (1-0.10), 2);
                    break;
                case DiscountType.B:
                    product.DiscountPrice = Math.Round(product.Price * (1-0.06), 2);
                    break;
                case DiscountType.C:
                    product.DiscountPrice = Math.Round(product.Price * (1-0.02), 2);
                    break;
                default:
                    product.DiscountPrice = 0.0;
                    break;
            }
        }
    }

    public static void updateDiscAmountToMasterlist(string originalFilePath, string newFilePath, List<Product> products)
    {
        using (StreamWriter writer = new StreamWriter(newFilePath))
        {
            foreach (var product in products)
            {
                writer.WriteLine($"{product.Barcode}|{product.Name}|{product.Description}|{product.ReleaseDate:dd/MM/yyyy}|{product.Feature}|${product.Price}|{product.Quantity}|{product.DiscountType}|{product.QuantitySold}|{product.Weight}|{product.PackagingMaterial}|{product.DiscountPrice}");
            }
        }
    }

    public static async Task Main(string[] args)
    {
        string filePath = "../ProdMasterlist.txt";
        string newFilePath = "../ProdMasterlistB.txt";
        var products = SectionA.Program.readProdMasterList(filePath);

        await Task.Run(() => calculateDiscount(products));

        foreach (var product in products)
        {
            Console.WriteLine($"{product.Name} - {product.Description} - {product.Price} - {product.DiscountType} - {product.DiscountPrice}");
        }

        double totalDiscount = 0;
        foreach (var product in products)
        {
            totalDiscount += product.DiscountPrice;
        }

        Console.WriteLine($"\nTotal Discount: {totalDiscount}");
        Console.WriteLine($"Total Products: {products.Count}");

        updateDiscAmountToMasterlist(filePath, newFilePath, products);

        Console.WriteLine("New masterlist with discounts generated.");
    }
}
