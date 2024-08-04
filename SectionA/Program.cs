﻿using System;
using System.Collections.Generic;
using System.IO;

//ChatGPT is used to improve exception handling for file generation.

namespace SectionA
{
    public class Program
    {
        public static List<Product> readProdMasterList(string filePath)
        {
            var productList = new List<Product>();

            try
            {
                var lines = File.ReadAllLines(filePath);

                if (lines.Length <= 1)
                {
                    Console.WriteLine("Error: The product master list file is empty or only contains the header.");
                    return productList;
                }

                for (int i = 1; i < lines.Length; i++)
                    {
                    var parts = lines[i].Split('|');
                    if (parts.Length < 11)
                    {
                        Console.WriteLine($"Error: Invalid product entry on line {i + 1}. Skipping this entry.");
                        continue;
                    }

                    try
                    {
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
                    catch (Exception error)
                    {
                        Console.WriteLine($"Error processing product entry on line {i + 1}: {error.Message}");
                    }
                }
            }
            catch (FileNotFoundException error)
            {
                Console.WriteLine($"Error: File not found - {error.Message}");
            }
            catch (IOException error)
            {
                Console.WriteLine($"Error reading file - {error.Message}");
            }
            catch (Exception error)
            {
                Console.WriteLine($"Unexpected error - {error.Message}");
            }

            return productList;
        }

        public static void generateInfoForMarketing(List<Product> products)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("Marketing.txt"))
                {
                    foreach (var product in products)
                    {
                        sw.WriteLine(product.FormatForMarketing());
                    }
                }
            }
            catch (IOException error)
            {
                Console.WriteLine($"Error writing to Marketing.txt - {error.Message}");
            }
            catch (Exception error)
            {
                Console.WriteLine($"Unexpected error - {error.Message}");
            }
        }

        public static void generateInfoForSales(List<Product> products)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("Sales.txt"))
                {
                    foreach (var product in products)
                    {
                        sw.WriteLine(product.FormatForSales());
                    }
                }
            }
            catch (IOException error)
            {
                Console.WriteLine($"Error writing to Sales.txt - {error.Message}");
            }
            catch (Exception error)
            {
                Console.WriteLine($"Unexpected error - {error.Message}");
            }
        }

        public static void generateInfoForLogistics(List<Product> products)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("Logistics.txt"))
                {
                    foreach (var product in products)
                    {
                        sw.WriteLine(product.FormatForLogistics());
                    }
                }
            }
            catch (IOException error)
            {
                Console.WriteLine($"Error writing to Logistics.txt - {error.Message}");
            }
            catch (Exception error)
            {
                Console.WriteLine($"Unexpected error - {error.Message}");
            }
        }

        public delegate void GenerateInfo(List<Product> products);

        public static void Main(string[] args)
        {
            string filePath = "../ProdMasterlist.txt";
            var products = readProdMasterList(filePath);

            if (products.Count == 0)
            {
                Console.WriteLine("No valid products found in the master list. Exiting program.");
                return;
            }

            GenerateInfo generateInfo = generateInfoForMarketing;
            generateInfo += generateInfoForSales;
            generateInfo += generateInfoForLogistics;

            generateInfo(products);

            Console.WriteLine("\nFiles have been generated.");
        }
    }
}
