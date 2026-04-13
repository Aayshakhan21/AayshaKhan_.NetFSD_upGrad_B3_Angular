using System;

class Product
{
    private int _productId;
    private string _productName;
    private double _unitPrice;
    private int _qty;

    public Product(int productId)
    {
        _productId = productId;
    }

    public int ProductId
    {
        get { return _productId; }
    }

    public string ProductName
    {
        get { return _productName; }
        set { _productName = value; }
    }

    public double UnitPrice
    {
        get { return _unitPrice; }
        set { _unitPrice = value; }
    }

    public int Quantity
    {
        get { return _qty; }
        set { _qty = value; }
    }

    public void ShowDetails()
    {
        double total = _unitPrice * _qty;

        Console.WriteLine("Product Id: " + _productId);
        Console.WriteLine("Product Name: " + _productName);
        Console.WriteLine("Unit Price: " + _unitPrice);
        Console.WriteLine("Quantity: " + _qty);
        Console.WriteLine("Total Amount: " + total);
    }

    static void Main()
    {
        Console.Write("Enter Product Id: ");
        int id = int.Parse(Console.ReadLine());

        Product p = new Product(id);

        Console.Write("Enter Product Name: ");
        p.ProductName = Console.ReadLine();

        Console.Write("Enter Unit Price: ");
        p.UnitPrice = double.Parse(Console.ReadLine());

        Console.Write("Enter Quantity: ");
        p.Quantity = int.Parse(Console.ReadLine());

        Console.WriteLine("\nProduct Details:");
        p.ShowDetails();
    }
}