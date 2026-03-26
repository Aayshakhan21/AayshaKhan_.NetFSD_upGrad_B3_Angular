using System;
using System.Data;

namespace ConsoleApp2
{
    class Program
    {
        static void Main()
        {
            ProductDataAccess dal = new ProductDataAccess();

        while (true)
            {
                Console.WriteLine("\n---- PRODUCT MANAGEMENT  ----- ");
                Console.WriteLine("1. Insert Product");
                Console.WriteLine("2. View All Products");
                Console.WriteLine("3. Get Product By Id");
                Console.WriteLine("4. Update Product");
                Console.WriteLine("5. Delete Product");
                Console.WriteLine("6. Exit");
                Console.Write("Enter choice: ");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input!");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Product p = new Product();

                        Console.Write("Enter Product Name: ");
                        p.ProductName = Console.ReadLine();

                        Console.Write("Enter Category: ");
                        p.Category = Console.ReadLine();

                        Console.Write("Enter Price: ");
                        p.Price = decimal.Parse(Console.ReadLine());

                        dal.InsertProduct(p);
                        Console.WriteLine("Inserted successfully.");
                        break;

                    case 2:
                        Console.WriteLine("\n--- Product List ---");

                        DataTable dt = dal.GetProducts();

                        foreach (DataRow row in dt.Rows)
                        {
                            Console.WriteLine($"{row["ProductId"]} | {row["ProductName"]} | {row["Category"]} | {row["Price"]}");
                        }
                        break;

                    case 3:
                        Console.Write("Enter Product Id: ");
                        int pid = int.Parse(Console.ReadLine());

                        DataTable dt2 = dal.GetProducts();
                        bool found = false;

                        foreach (DataRow row in dt2.Rows)
                        {
                            if ((int)row["ProductId"] == pid)
                            {
                                Console.WriteLine($"{row["ProductId"]} | {row["ProductName"]} | {row["Category"]} | {row["Price"]}");
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                            Console.WriteLine("Product not found.");
                        break;

                    case 4:
                        Product up = new Product();

                        Console.Write("Enter Product Id: ");
                        up.ProductId = int.Parse(Console.ReadLine());

                        Console.Write("Enter New Name: ");
                        up.ProductName = Console.ReadLine();

                        Console.Write("Enter New Category: ");
                        up.Category = Console.ReadLine();

                        Console.Write("Enter New Price: ");
                        up.Price = decimal.Parse(Console.ReadLine());

                        dal.UpdateProduct(up);
                        Console.WriteLine("Updated successfully.");
                        break;

                    case 5:
                        Console.Write("Enter Product Id to delete: ");
                        int id = int.Parse(Console.ReadLine());

                        dal.DeleteProduct(id);
                        Console.WriteLine("Deleted successfully.");
                        break;

                    case 6:
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
        }
    }

}
