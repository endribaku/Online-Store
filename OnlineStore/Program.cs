
using OnlineStoreClassLibrary;

namespace OnlineStore;

class Program
{
    static void Main()
    {
        Console.Title = "Online Store";
        Console.ForegroundColor = ConsoleColor.Green;
        
        List<Product> products = new List<Product>() // starter products
        {
            new Product("Laptop", 999.99m),
            new Product("Smartphone", 699.50m),
            new Product("Headphones", 149.95m),
            new Product("Monitor", 229.99m),
            new Product("Keyboard", 89.99m)
        };
        
        OnlineStoreSystem system = new OnlineStoreSystem(products);
        bool isRunning = true;
        while (isRunning)
        {
            try
            {
                string operation = ShowMainMenu();
                switch (operation)
                {
                    case "0":
                        isRunning = false;
                        break;
                    case "1":
                        bool onProductMenu = true;
                        while (onProductMenu)
                        {
                            string productOperation = ShowProductMenu();
                            switch (productOperation)
                            {
                                case "0":
                                    onProductMenu = false;
                                    break;
                                case "1":
                                    DisplayProducts(system);
                                    break;
                                case "2":
                                    AddProductMenu(system);
                                    break;
                            }
                        }

                        break;
                    case "2":
                        bool onCustomerMenu = true;
                        while (onCustomerMenu)
                        {
                            string customerOperation = ShowCustomerMenu();
                            switch (customerOperation)
                            {
                                case "0":
                                    onCustomerMenu = false;
                                    break;
                                case "1":
                                    DisplayCustomers(system);
                                    break;
                                case "2":
                                    AddCustomerMenu(system);
                                    break;
                                case "3":
                                    SelectCustomerMenu(system);
                                    break;
                            }
                        }

                        break;
                    case "3":
                        bool onCartMenu = true;
                        while (onCartMenu)
                        {
                            string cartOperation = ShowCartMenu();
                            switch (cartOperation)
                            {
                                case "0":
                                    onCartMenu = false;
                                    break;
                                case "1":
                                    DisplayCart(system);
                                    break;
                                case "2":
                                    AddToCart(system);
                                    break;
                                case "3":
                                    Checkout(system);
                                    break;
                            }
                        }

                        break;
                    case "4":
                        DisplayOrders(system);
                        break;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Invalid Input {ex.Message}");
            }
        }
        
    }


    private static string ShowMainMenu()
    {
        Console.WriteLine("Welcome to the Online Store!");
        Console.WriteLine("What would you like to manage?");
        Console.WriteLine("[0] Exit");
        Console.WriteLine("[1] Products");
        Console.WriteLine("[2] Customers");
        Console.WriteLine("[3] Cart");
        Console.WriteLine("[4] Orders");

        return Console.ReadLine()!;
    }

    private static string ShowProductMenu()
    {
        Console.WriteLine("What would you like to do?");
        Console.WriteLine("[0] Exit");
        Console.WriteLine("[1] Display Products");
        Console.WriteLine("[2] Add Product");

        return Console.ReadLine()!;
    }


    private static void DisplayProducts(OnlineStoreSystem system)
    {
        List<ProductDto> products = system.GetProducts();
        if (products.Count == 0)
        {
            Console.WriteLine("No products found");
            return;
        }
        foreach (ProductDto product in products)
        {
            Console.WriteLine($"Product Id: {product.Id} Name: {product.Name} Price: ({product.Price})");
        }
    }

    private static void AddProductMenu(OnlineStoreSystem system)
    {
        Console.WriteLine("What would you like to add?");
        
        string productName = "";
        bool validName = false;
        while (!validName)
        {
            Console.WriteLine("Enter product name:");
            productName = Console.ReadLine()!;
            if (productName.Length > 0 && !system.CheckProductByName(productName))
            {
                validName = true;
            }
            else
            {
                Console.WriteLine("Invalid product name, either empty or product already in the system");
            }
        }
        
        string productPrice;
        bool validPrice = false;
        decimal productPriceDecimal = 0m;
        while (!validPrice)
        {
            Console.WriteLine("Enter product price:");
            productPrice = Console.ReadLine()!;
            
            bool isDecimal = Decimal.TryParse(productPrice, out productPriceDecimal);
            if (isDecimal && productPriceDecimal >= 0)
            {
                validPrice = true;
            }
            else
            {
                Console.WriteLine("Price must be a decimal number and bigger or equal to 0.");
            }
        }
        
        system.AddProduct(productName, productPriceDecimal);
    }

    private static string ShowCustomerMenu()
    {
        Console.WriteLine("What would you like to do?");
        Console.WriteLine("[0] Exit");
        Console.WriteLine("[1] Display Customers");
        Console.WriteLine("[2] Add Customer");
        Console.WriteLine("[3] Select Customer");

        
        return Console.ReadLine()!;
    }

    private static void DisplayCustomers(OnlineStoreSystem system)
    {
        List < CustomerDto > customers = system.GetCustomers();
        if (customers.Count == 0)
        {
            Console.WriteLine("No Customers found");
            return;
        }
        foreach (CustomerDto customer in customers)
        {
            Console.WriteLine($"Customer Id: {customer.Id} Name: {customer.Name}");
        }
    }

    private static void AddCustomerMenu(OnlineStoreSystem system)
    {
        Console.WriteLine("Who would you like to add?");
        
        string customerName = "";
        bool validName = false;
        while (!validName)
        {
            Console.WriteLine("Enter customer name:");
            customerName = Console.ReadLine()!;
            if (customerName.Length > 0 && !system.CheckCustomerByName(customerName))
            {
                validName = true;
            }
        }
        
        system.AddCustomer(customerName);
    }

    private static void SelectCustomerMenu(OnlineStoreSystem system)
    {
        bool validId = false;
        while (!validId)
        {
            Console.WriteLine("Which customer would you like to select? Select by Id");
            DisplayCustomers(system);
        
            string customerOption = Console.ReadLine()!;
            bool isInt = int.TryParse(customerOption, out var customerId);
            if (isInt && system.SelectActiveCustomer(customerId))
            {
                Console.WriteLine("Customer selected");
                validId = true;
            }
            else
            {
                Console.WriteLine("Invalid id");
            }
        }
    }

    private static string ShowCartMenu()
    {
        Console.WriteLine("What would you like to cart?");
        Console.WriteLine("[0] Exit");
        Console.WriteLine("[1] Display Cart");
        Console.WriteLine("[2] Add To Cart");
        Console.WriteLine("[3] Checkout");

        return Console.ReadLine()!;

    }

    private static void DisplayCart(OnlineStoreSystem system)
    {
        List<CartItemDto> shoppingCart = system.GetActiveCustomerCart();
        if (shoppingCart == null)
        {
            Console.WriteLine("No shopping cart. Select Customer");
        } else if (shoppingCart.Count == 0)
        {
            Console.WriteLine("Shopping cart is empty");
        }
        else
        {
            foreach (CartItemDto shopping in shoppingCart)
            {
                Console.WriteLine($"Product Id: {shopping.Id} Name: {shopping.Name} Price: ({shopping.Price}) Quantity: {shopping.Quantity}");
            }
        }
    }

    private static void AddToCart(OnlineStoreSystem system)
    {
        bool onCartMenu = true;
        
        while (onCartMenu)
        {
            Console.WriteLine("Press yes to proceed with adding to cart. Press no to cancel");
            string addToCartOption = Console.ReadLine()!;
            switch (addToCartOption.ToLower())
            {
                case "yes":
                    Console.WriteLine("What would you like to add? Choose what you want by Product Id");
                    DisplayProducts(system);
            
                    bool validProduct = false;
                    int productId = -1;
                    int quantity = -1;
                    while (!validProduct)
                    {
                        bool validProductId = false;
                        while (!validProductId)
                        {
                            Console.WriteLine("Enter product id:");
                            string productIdOption = Console.ReadLine()!;
                            bool isInt = int.TryParse(productIdOption, out productId);
                            if (isInt && system.CheckProductById(productId))
                            {
                                validProductId = true;
                            }
                        }
                        bool validQuantity = false;
                        while (!validQuantity)
                        {
                            Console.WriteLine("Enter quantity: ");
                            string quantityOption = Console.ReadLine()!;
                            bool isInt = int.TryParse(quantityOption, out quantity);
                            if (isInt && quantity > 0)
                            {
                                validQuantity = true;
                            } 
                        }
                
                        validProduct = true;
                    }
            

                    system.AddToCart(productId, quantity);
                    break;
                case "no":
                    onCartMenu = false;
                    break;
            }
        }
    }

    private static void Checkout(OnlineStoreSystem system)
    {
        if (!system.HasActiveCustomer())
        {
            Console.WriteLine("No active customer.");
            return;
        }
        
        OrderDto order = system.Checkout();
        if (order == null)
        {
            Console.WriteLine("Shopping cart is empty, Order couldn't be placed.");
        }
        else
        {
            Console.WriteLine("Order placed");
            Console.WriteLine($"OrderId: {order.Id} Customer Name: {order.Customer.Name}, Total: {order.Total}, Date: {order.OrderDate}");
        }
    }

    private static void DisplayOrders(OnlineStoreSystem system)
    {
        List<OrderDto> orders = system.GetOrders();
        if (orders.Count == 0)
        {
            Console.WriteLine("No orders found.");
            return;
        }

        foreach (OrderDto order in orders)
        {
            Console.WriteLine($"OrderId: {order.Id} Customer: {order.Customer.Name} Total: {order.Total} Date: {order.OrderDate}");
        }
        
    }
    
    
    
}

