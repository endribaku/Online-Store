
using System.Data.Common;
using MySqlConnector;

namespace OnlineStore;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        Console.Title = "Online Store";
        Console.ForegroundColor = ConsoleColor.Green;
        
       
        //connection string
        MySqlConnectorFactory factory = MySqlConnectorFactory.Instance;
        MySqlConnectionStringBuilder mySqlConnectionStringBuilder = new MySqlConnectionStringBuilder();
        mySqlConnectionStringBuilder.Server = "127.0.0.1";
        mySqlConnectionStringBuilder.Port = 3306;
        mySqlConnectionStringBuilder.UserID = "root";
        mySqlConnectionStringBuilder.Password = "my-secret-pw";
        mySqlConnectionStringBuilder.Database = "online_store";
        
        Console.WriteLine(mySqlConnectionStringBuilder.ConnectionString);
        
        OnlineStoreSystem system = new OnlineStoreSystem(factory, mySqlConnectionStringBuilder.ConnectionString);
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
        List<Product> products = system.GetProducts();
        if (products.Count == 0)
        {
            Console.WriteLine("No products found");
            return;
        }
        foreach (Product product in products)
        {
            Console.WriteLine($"Product Id: {product.ProductId} Name: {product.Name} Price: ({product.Price})");
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
        List <Customer> customers = system.GetCustomers();
        if (customers.Count == 0)
        {
            Console.WriteLine("No Customers found");
            return;
        }
        foreach (Customer customer in customers)
        {
            Console.WriteLine($"Customer Id: {customer.CustomerId} Name: {customer.FirstName} {customer.LastName}");
        }
    }

    private static void AddCustomerMenu(OnlineStoreSystem system)
    {
        Console.WriteLine("Who would you like to add?");
        
        string customerFirstName = "";
        string customerLastName = "";
        bool validFirstName = false;
        bool validLastName = false;
        while (!validFirstName || !validLastName)
        {
            Console.WriteLine("Enter customer First name:");
            customerFirstName = Console.ReadLine()!;
            if (customerFirstName.Length > 0)
            {
                validFirstName = true;
            }
            Console.WriteLine("Enter customer Last name:");
            customerLastName = Console.ReadLine()!;
            if (customerLastName.Length > 0)
            {
                validLastName = true;
            }
        }
        
        system.AddCustomer(customerFirstName, customerLastName);
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
                Console.WriteLine("Customer selected with id {0}", customerId);
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
        Cart shoppingCart = system.GetActiveCustomerCart();
        if (shoppingCart == null)
        {
            Console.WriteLine("No shopping cart. Select Customer");
        } else if (shoppingCart.Items.Count == 0)
        {
            Console.WriteLine("Shopping cart is empty");
        }
        else
        {
            Console.WriteLine("Cart Items count: {0}", shoppingCart.Items.Count);
            foreach (CartItem shopping in shoppingCart.Items)
            {
                Console.WriteLine($"Product Id: {shopping.ProductId} Name: {shopping.Product.Name} UnitPrice: ({shopping.Product.Price}) Quantity: {shopping.Quantity} Total:{shopping.Product.Price * shopping.Quantity}");
            }
        }
    }

    private static void AddToCart(OnlineStoreSystem system)
    {
        bool onCartMenu = true;
        if (!system.HasActiveCustomer())
        {
            Console.WriteLine("No active customer");
            return;
        }
        
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
        
        CustomerOrder order = system.Checkout();
        if (order == null)
        {
            Console.WriteLine("Shopping cart is empty, Order couldn't be placed.");
        }
        else
        {
            Console.WriteLine("Order placed");
            Console.WriteLine($"Customer Name: {order.CustomerName}, Total: {order.Total}, Date: {order.Date}");
        }
    }

    private static void DisplayOrders(OnlineStoreSystem system)
    {
        List<CustomerOrder> orders = system.GetOrders();
        if (orders.Count == 0)
        {
            Console.WriteLine("No orders found.");
            return;
        }

        foreach (CustomerOrder order in orders)
        {
            Console.WriteLine($"OrderId: {order.OrderId} Customer: {order.CustomerName} Total: {order.Total} Date: {order.Date}");
        }
        
    }
    
    
    
}

