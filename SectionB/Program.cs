using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SectionA;

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
                Console.WriteLine($"Invalid discount type for product {product.Name} ({product.Barcode}). Setting discount price to 0.");
                product.DiscountPrice = product.Price;
            }
        }
    }

    public static void updateDiscAmountToMasterlist(string originalFilePath, string newFilePath, List<Product> products)
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
        catch (IOException ex)
        {
            Console.WriteLine($"Error writing to {newFilePath} - {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error - {ex.Message}");
        }
    }

    public static async Task Main(string[] args)
    {
        string filePath = "../ProdMasterlist.txt";
        string newFilePath = "../ProdMasterlistB.txt";
        var products = SectionA.Program.readProdMasterList(filePath);

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

        updateDiscAmountToMasterlist(filePath, newFilePath, products);

        Console.WriteLine("\nGenerated ProdMasterlistB.txt in root directory.");
    }
}
