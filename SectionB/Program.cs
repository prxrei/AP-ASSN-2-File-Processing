using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SectionA;

// Using Enum for Discount Types
public enum DiscountType
{
    A,
    B,
    C
}

public class Program
{
    // Method to calculate discounts based on the discount type
    public static void calculateDiscount(List<Product> products)
    {
        foreach (var product in products)
        {
            try
            {
                switch (Enum.Parse<DiscountType>(product.DiscountType))
                {
                    case DiscountType.A:
                        product.DiscountPrice = Math.Round(product.Price * (1 - 0.10), 2);
                        break;
                    case DiscountType.B:
                        product.DiscountPrice = Math.Round(product.Price * (1 - 0.06), 2);
                        break;
                    case DiscountType.C:
                        product.DiscountPrice = Math.Round(product.Price * (1 - 0.02), 2);
                        break;
                    default:
                        product.DiscountPrice = product.Price;
                        break;
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"Invalid discount type for product {product.Name} ({product.Barcode}). Setting discount price to original price.");
                product.DiscountPrice = product.Price;
            }
        }
    }

    // Method to update the master list with discount amounts
    public static void updateDiscAmountToMasterlist(string newFilePath, List<Product> products)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(newFilePath))
            {
                foreach (var product in products)
                {
                    writer.WriteLine($"{product.Barcode}|{product.Name}|{product.Description}|{product.ReleaseDate:dd/MM/yyyy}|{product.Feature}|{product.Price}|{product.Quantity}|{product.DiscountType}|{product.QuantitySold}|{product.Weight}|{product.PackagingMaterial}|{product.DiscountPrice}");
                }
            }
        }
        catch (IOException err)
        {
            Console.WriteLine($"Error writing to {newFilePath} - {err.Message}");
        }
        catch (Exception err)
        {
            Console.WriteLine($"Unexpected error - {err.Message}");
        }
    }

    // Main method to run the program
    public static async Task Main(string[] args)
    {
        string filePath = "../ProdMasterlist.txt";
        string newFilePath = "../ProdMasterlistB.txt";

        var products = SectionA.Program.readProdMasterList(filePath);

        // Calculate discounts asynchronously using await
        await Task.Run(() => calculateDiscount(products));

        double totalDiscount = 0;

        foreach (var product in products)
        {
            totalDiscount += product.DiscountPrice;

            Console.WriteLine($"{product.Name} ({product.Barcode})");
            Console.WriteLine($"Price: {product.Price}");
            Console.WriteLine($"Discount Price: ${product.DiscountPrice}");
            Console.WriteLine(new string('-', 50));
        }

        Console.WriteLine($"\nTotal discount price: ${Math.Round(totalDiscount, 2)} for {products.Count} products.");

        updateDiscAmountToMasterlist(newFilePath, products);

        Console.WriteLine("\nGenerated ProdMasterlistB.txt in root directory.");
    }
}
