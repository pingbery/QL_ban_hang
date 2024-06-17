using System;
using System.Collections.Generic;
using System.Net;
using MySql.Data.MySqlClient;

class Program
{
    public static string connectionString = "server=localhost;user=root;password=Binh6179;database=qly_ban_hang;";

    static void Main(string[] args)
    {
        while(true)
        {
            DrawMenuFrame("MENU LOGIN",new string[]{
                "1. Login",
                "2. Register",
                "3. Forgot Password",
                "0. Exit",
            });

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Login();
                    break;
                case 2:
                    Register();
                    break;
                case 3:
                    ForgotPassword();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Select Again.");
                    return;
            }
        }
    }
    static void Login()
    {
        Console.WriteLine("Login:");
        Console.Write("Enter your login name: ");
        string username = Console.ReadLine();
        Console.Write("Enter your password: ");
        string password = Console.ReadLine();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM users WHERE username = @Loginname AND password = @Password";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Loginname", username);
            command.Parameters.AddWithValue("@Password", password);

            long count = (long)command.ExecuteScalar();
            if (count > 0)
            {
                Console.WriteLine("Logged in successfully!");
                MainMenu();
            }
            else
            {
                Console.WriteLine("Username or password is incorrect.");
            }
        }
    }
    static bool IsUsernameExists(string username)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM users WHERE username = @Loginname";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Loginname", username);

            long count = (long)command.ExecuteScalar();
            return count > 0;
        }
    }
    static void Register()
    {
        Console.WriteLine("Register:");
        Console.Write("Enter your login name: ");
        string username = Console.ReadLine();

        if (IsUsernameExists(username))
        {
            Console.WriteLine("Error: Username already exists. Please choose another username.");
            Register();
            return;
        }
        Console.Write("Enter your pasword: ");
        string password = Console.ReadLine();
        Console.Write("Enter your email: ");
        string email = Console.ReadLine();
        Console.Write("Enter your phone number: ");
        int phoneNumber = int.Parse(Console.ReadLine());
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "INSERT INTO users (username, password, email, phone_number) VALUES (@Loginname, @Password, @Email, @Phonenumber)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Loginname", username);
            command.Parameters.AddWithValue("@Password", password);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Phonenumber", phoneNumber);

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("Sign Up Success!");
            }
        }
    }
    static bool VerifyUserInfo(string username, string email, int phoneNumber)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM users WHERE username = @Loginname AND email = @Email AND phone_number = @Phonenumber";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Loginname", username);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Phonenumber", phoneNumber);

            long count = (long)command.ExecuteScalar();
            return count > 0;
        }
    }
    static bool UpdatePassword(string username, string password)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "UPDATE users SET password = @Pasword WHERE username = @Loginname";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Loginname", username);
            command.Parameters.AddWithValue("@Pasword", password);

            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }
    }
    static void ForgotPassword()
    {
        Console.WriteLine("Forgot Pasword:");
        Console.Write("Enter your loggin name: ");
        string username = Console.ReadLine();
        Console.Write("Enter your email: ");
        string email = Console.ReadLine();
        Console.Write("Enter your phone number: ");
        int phone = int.Parse(Console.ReadLine());

        // Kiểm tra thông tin với cơ sở dữ liệu
        if (VerifyUserInfo(username, email, phone))
        {
            Console.WriteLine("Accurate information. Please enter a new password.");
            Console.Write("Enter your new password: ");
            string password = Console.ReadLine();

            // Cập nhật mật khẩu mới vào cơ sở dữ liệu
            if (UpdatePassword(username, password))
            {
                Console.WriteLine("Password has been reset successfully.");
            }
        }
        else
        {
            Console.WriteLine("Incorrect information. Unable to reset password");
        }
    }
    
    static void MainMenu()
    {
        while(true)
        {
            Console.Clear();
            DrawMenuFrame("MENU", new string[] {
                "1. Category",
                "2. Policies and Customer Support",
                "3. Documents",
                "0. Exit"
            });

            int choice = int.Parse(Console.ReadLine());

            switch(choice)
            {
                case 1:
                    MainMenu1();
                    break;
                case 2:
                    MainMenu2();
                    break;
                case 3:
                    MainMenu3();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    static void MainMenu1()
    {
        while(true)
        {
            Console.Clear();
            DrawMenuFrame("MENU 1", new string[] {
                "1. Customer Management",
                "2. Product Management",
                "3. Category Management",
                "4. Warehouse Management",
                "5. Coupon Management",
                "0. Exit"
            });

            int choice = int.Parse(Console.ReadLine());

            switch(choice)
            {
                case 1:
                    CustomersManagementMenu();
                    break;
                case 2:
                    ProductsManagementMenu();
                    break;
                case 3:
                    CategoryManagementMenu();
                    break;
                case 4:
                    WarehouseManagementMenu();
                    break;
                case 5:
                    CouponsManagementMenu();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    static void MainMenu2()
    {
        while(true)
        {
            Console.Clear();
            DrawMenuFrame("MENU 2", new string[] {
                "1. Policy Management",
                "2. Customer Support",
                "0. Exit",
            });

            int choice = int.Parse(Console.ReadLine());

            switch(choice)
            {
                case 1:
                    PolicyManagementMenu();
                    break;
                case 2:
                    CustomerSupportMenu();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    static void MainMenu3()
    {
        while (true)
        {
            Console.Clear();
            DrawMenuFrame("MENU 3", new string[] {
                "1. Invoice Management",
                "2. Orders Management",
                "3. Export Receipts Management",
                "4. Receipts Management",
                "0. Exit",
            });

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    InvoiceManagementMenu();
                    break;
                case 2:
                    OrdersManagementMenu();
                    break;
                case 3:
                    ExportReceiptsManagementMenu();
                    break;
                case 4:
                    ReceiptsManagementMenu();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    static void DrawMenuFrame(string title, string[] options)
    {
        int width = title.Length > 20 ? title.Length : 20;
        Console.WriteLine(new string('═', width));
        Console.WriteLine(title);
        Console.WriteLine(new string('═', width));

        foreach (var option in options)
        {
            Console.WriteLine(option);
        }

        Console.WriteLine(new string('═', width));
        Console.Write("Select an option: ");
    }

    static void CustomersManagementMenu()
    {
        List<Customer> customerList = new List<Customer>();
        while (true)
        {
            DrawMenuFrame("===Customer Management===",new string[]{
                "1. View Customer List",
                "2. Add Customer",
                "3. Edit Customer Information",
                "4. Delete Customer",
                "5. Search Customer",
                "0. Exit",
            });

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    DisplayCustomer(customerList);
                    break;
                case 2:
                    AddCustomer(customerList);
                    break;
                case 3:
                    EditCustomerInfo(customerList, connectionString);
                    break;
                case 4:
                    DeleteCustomerInfo(customerList,connectionString);
                    break;
                case 5:
                    SearchCustomerByID(customerList,connectionString);
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    static void ProductsManagementMenu()
    {
        List<Product> productList = new List<Product>();
        while (true)
        {
            DrawMenuFrame("Product Management", new string[] {
            "1. View Product List",
            "2. Add Product",
            "3. Edit Product Information",
            "4. Delete Product Information",
            "5. Search Product",
            "0. Exit",
            });

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    DisplayProduct(productList);
                    break;
                case 2:
                    AddProduct(productList);
                    break;
                case 3:
                    EditProductInfo(productList, connectionString);
                    break;
                case 4:
                    DeleteProductInfo(productList, connectionString);
                    break;
                case 5:
                    SearchProductByID(productList, connectionString);
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    static void CategoryManagementMenu()
    {
        List<CategoryOfProduct> categoryList = new List<CategoryOfProduct>();
        while (true)
        {
            DrawMenuFrame("Category Management", new string[] {
            "1. View Category List",
            "2. Add Category",
            "3. Edit Category Information",
            "4. Delete Category",
            "5. Search Category",
            "0. Exit",
            });

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    DisplayCategory(categoryList);
                    break;
                case 2:
                    AddCategory(categoryList);
                    break;
                case 3:
                    EditCategoryInfo(categoryList, connectionString);
                    break;
                case 4:
                    DeleteCategoryInfo(categoryList, connectionString);
                    break;
                case 5:
                    SearchCategoryByID(categoryList, connectionString);
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    static void CouponsManagementMenu()
    {
        List<Coupon> couponList = new List<Coupon>();
        while (true)
        {
            DrawMenuFrame("=== Coupon Management ===", new string[] {
            "1. View Coupon List",
            "2. Add Coupon",
            "3. Edit Coupon Information",
            "4. Delete Coupon",
            "5. Search Coupon",
            "0. Exit",
            });

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    DisplayCoupons(couponList);
                    break;
                case 2:
                    AddCoupon(couponList);
                    break;
                case 3:
                    EditCouponInfo(couponList, connectionString);
                    break;
                case 4:
                    DeleteCoupon(couponList, connectionString);
                    break;
                case 5:
                    SearchCouponByID(couponList, connectionString);
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    static void WarehouseManagementMenu()
    {
        List<Warehouse> warehouseList = new List<Warehouse>();
        while (true)
        {
            DrawMenuFrame("Warehouse Management", new string[]{
            "1. View Warehouse List",
            "2. Add Warehouse Information",
            "3. Edit Warehouse Information",
            "4. Delete Warehouse Information",
            "5. Search Warehouse Information by ID",
            "0. Exit"
            });

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    DisplayWarehouse(warehouseList);
                    break;
                case 2:
                    AddWarehouseInfo(warehouseList, connectionString);
                    break;
                case 3:
                    EditWarehouseInfo(warehouseList, connectionString);
                    break;
                case 4:
                    DeleteWarehouseInfo(warehouseList, connectionString);
                    break;
                case 5:
                    SearchWarehouseByID(warehouseList, connectionString);
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    static void PolicyManagementMenu()
    {
        List<Policy> policyList = new List<Policy>();
        while (true)
        {
            DrawMenuFrame("Policy Management", new string[] {
            "1. Display Policies",
            "2. Add Policy",
            "3. Edit Policy",
            "4. Delete Policy",
            "0. Exit",
            });

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    DisplayPolicy(policyList);
                    break;
                case 2:
                    AddPolicy(policyList);
                    break;
                case 3:
                    EditPolicy(policyList, connectionString);
                    break;
                case 4:
                    DeletePolicy(policyList, connectionString);
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    static void CustomerSupportMenu()
    {
        List<Issue> issueList = new List<Issue>();
        while (true)
        {
            DrawMenuFrame("Customer Support Management", new string[] {
            "1. View List of Issues",
            "2. Add Issue",
            "3. Edit Issue",
            "4. Delete Issue",
            "0. Exit",
            });

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    DisplayIssues(issueList);
                    break;
                case 2:
                    AddIssue(issueList, connectionString);
                    break;
                case 3:
                    EditIssue(issueList, connectionString);
                    break;
                case 4:
                    DeleteIssue(issueList, connectionString);
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    static void InvoiceManagementMenu()
    {
        List<Invoice> invoiceList = new List<Invoice>();
        while (true)
        {
            DrawMenuFrame("Invoice Management", new string[] {
            "1. View List of Invoices",
            "2. Add Invoice",
            "3. Edit Invoice",
            "4. Delete Invoice",
            "5. Search Invoice",
            "0. Exit",
            });

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    DisplayInvoice(invoiceList);
                    break;
                case 2:
                    AddInvoice(invoiceList);
                    break;
                case 3:
                    EditInvoice(invoiceList,connectionString);
                    break;
                case 4:
                    DeleteInvoice(invoiceList,connectionString);
                    break;
                case 5:
                    SearchInvoice(invoiceList,connectionString);
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    static void OrdersManagementMenu()
    {
        List<Order> orderList = new List<Order>();
        while (true)
        {
            DrawMenuFrame("Order Management", new string[] {
                "1. View List of Orders",
                "2. Add Order",
                "3. Edit Order",
                "4. Delete Order",
                "5. Search Order",
                "6. Áp dụng mã giảm giá vào đơn hàng",
                "0. Back",
            });

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    DisplayOrder(orderList);
                    break;
                case 2:
                    AddOrder(orderList,connectionString);
                    break;
                case 3:
                    EditOrderInfo(orderList,connectionString);
                    break;
                case 4:
                    DeleteOrderInfo(orderList,connectionString);
                    break;
                case 5:
                    SearchOrderByID(orderList,connectionString);
                    break;
                case 6:
                    Console.Write("Nhập mã đơn hàng: ");
                    int orderID = int.Parse(Console.ReadLine());
                    Console.Write("Nhập mã giảm giá: ");
                    string couponCode = Console.ReadLine();
                    ApplyCouponToOrder(orderID, couponCode, connectionString);
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    static void ExportReceiptsManagementMenu()
    {
        List<ExportReceipt> exportReceiptList = new List<ExportReceipt>();
        while (true)
        {
            DrawMenuFrame("Export Receipt Management", new string[] {
                "1. View List of Export Receipts",
                "2. Add Export Receipt",
                "3. Edit Export Receipt",
                "4. Delete Export Receipt",
                "5. Search Export Receipt",
                "0. Back",
            });

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    DisplayExportReceipt(exportReceiptList);
                    break;
                case 2:
                    AddExportReceipt(exportReceiptList);
                    break;
                case 3:
                    EditExportReceiptInfo(exportReceiptList,connectionString);
                    break;
                case 4:
                    DeleteExportReceiptInfo(exportReceiptList,connectionString);
                    break;
                case 5:
                    SearchExportReceiptByID(exportReceiptList,connectionString);
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    static void ReceiptsManagementMenu()
    {
        List<ImportReceipt> importReceiptList = new List<ImportReceipt>();
        while (true)
        {
            DrawMenuFrame("Import Receipt Management", new string[] {
                "1. View List of Import Receipts",
                "2. Add Import Receipt",
                "3. Edit Import Receipt",
                "4. Delete Import Receipt",
                "5. Search Import Receipt",
                "0. Exit",
            });

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    DisplayImportReceipt(importReceiptList);
                    break;
                case 2:
                    AddImportReceipt(importReceiptList);
                    break;
                case 3:
                    EditImportReceiptInfo(importReceiptList,connectionString);
                    break;
                case 4:
                    DeleteImportReceiptInfo(importReceiptList,connectionString);
                    break;
                case 5:
                    SearchImportReceiptByID(importReceiptList,connectionString);
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void DisplayCustomer(List<Customer> customerList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM customer";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("-----Customer List-----");
            while (reader.Read())
            {
                Console.WriteLine($"Customer ID: {reader["CustomerID"]}, Name: {reader["Name"]}, Email: {reader["Email"]}, Gender: {reader["Sex"]}, PhoneNumber: {reader["PhoneNumber"]}, Address: {reader["Address"]}, Date of Birth: {reader["DateOfBirth"]}");
            }
            reader.Close();
        }
    }
    static void AddCustomer(List<Customer> customerList)
    {
        Console.WriteLine("Enter information for the new customer: ");
        Customer customer = new Customer();
        Console.Write("Enter customer ID: ");
        customer.CustomerID = int.Parse(Console.ReadLine());
        Console.Write("Enter full name: ");
        customer.Name = Console.ReadLine();
        Console.Write("Enter email: ");
        customer.Email = Console.ReadLine();
        Console.Write("Enter phone number: ");
        customer.Phonenumber = int.Parse(Console.ReadLine());
        Console.Write("Enter address: ");
        customer.Address = Console.ReadLine();
        Console.Write("Enter Sex: ");
        customer.Sex = Console.ReadLine();
        Console.Write("Enter date of birth: ");
        customer.DateOfBirth = DateTime.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO customer (CustomerID, Name, Email, PhoneNumber, Address, Sex, DateOfBirth) VALUES (@CustomerID, @Name, @Email, @Phone, @Address, @Gender, @DateOfBirth)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                command.Parameters.AddWithValue("@Name", customer.Name);
                command.Parameters.AddWithValue("@Email", customer.Email);
                command.Parameters.AddWithValue("@Phone", customer.Phonenumber);
                command.Parameters.AddWithValue("@Address", customer.Address);
                command.Parameters.AddWithValue("@Gender", customer.Sex);
                command.Parameters.AddWithValue("@DateOfBirth", customer.DateOfBirth);
                command.ExecuteNonQuery();
            }
            customerList.Add(customer);
            Console.WriteLine("Customer added successfully!");
    }
    static void EditCustomerInfo(List<Customer> customerList, string connectionString)
    {
        Console.Write("Enter customer ID to edit: ");
        int customerID = int.Parse(Console.ReadLine());
        Customer customer = customerList.Find(c => c.CustomerID == customerID);

        if (customer != null)
        {
            Console.WriteLine("Enter new information: ");
            Console.Write("Enter full name: ");
            customer.Name = Console.ReadLine();
            Console.Write("Enter gender: ");
            customer.Sex = Console.ReadLine();
            Console.Write("Enter email: ");
            customer.Email = Console.ReadLine();
            Console.Write("Enter phone number: ");
            customer.Phonenumber = int.Parse(Console.ReadLine());
            Console.Write("Enter address: ");
            customer.Address = Console.ReadLine();
            Console.Write("Enter date of birth: ");
            customer.DateOfBirth = DateTime.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE customer SET Name = @Name, Sex = @Gender, Email = @Email, PhoneNumber = @Phone, Address = @Address, DateOfBirth = @DateOfBirth WHERE CustomerID = @CustomerID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                command.Parameters.AddWithValue("@Name", customer.Name);
                command.Parameters.AddWithValue("@Gender", customer.Sex);
                command.Parameters.AddWithValue("@Email", customer.Email);
                command.Parameters.AddWithValue("@Phone", customer.Phonenumber);
                command.Parameters.AddWithValue("@Address", customer.Address);
                command.Parameters.AddWithValue("@DateOfBirth", customer.DateOfBirth);
                command.ExecuteNonQuery();
            }
            Console.WriteLine("Customer information updated successfully!");
        }
        else
        {
            Console.WriteLine("Customer with this ID not found.");
        }
    }
    static void DeleteCustomerInfo(List<Customer> customerList, string connectionString)
    {
        Console.WriteLine("Enter customer ID to delete: ");
        int customerID = int.Parse(Console.ReadLine());

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM customer WHERE CustomerID = @CustomerID";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " customer(s) deleted from the database.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
    static void SearchCustomerByID(List<Customer> customerList, string connectionString)
    {
        Console.Write("Enter customer ID to find: ");
        int customerID = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM customer WHERE CustomerID = @CustomerID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CustomerID", customerID);

            connection.Open();
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine("Customer information:");
                    Console.WriteLine($"Customer ID: {reader["CustomerID"]}");
                    Console.WriteLine($"Full Name: {reader["Name"]}");
                    Console.WriteLine($"Phone: {reader["PhoneNumber"]}");
                    Console.WriteLine($"Email: {reader["Email"]}");
                    Console.WriteLine($"Address: {reader["Address"]}");
                    Console.WriteLine($"Date of Birth: {reader["DateOfBirth"]}");
                }
                else
                {
                    Console.WriteLine("No customer found with this ID.");
                }
            }
        }
    }
    
    static void DisplayProduct(List<Product> productList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM product";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("-----Product List-----");
            while (reader.Read())
            {
                Console.WriteLine($"Product ID: {reader["ProductID"]}, Category ID: {reader["CategoryID"]}, Product Name: {reader["ProductName"]}, Description: {reader["Description"]}, Price: {reader["Price"]}");
            }
            reader.Close();
        }
    }
    static void AddProduct(List<Product> productList)
    {
        Console.WriteLine("Enter information for the new product: ");
        Product product = new Product();
        Console.Write("Enter product ID: ");
        product.ProductID = int.Parse(Console.ReadLine());
        Console.Write("Enter category ID: ");
        product.CategoryID = int.Parse(Console.ReadLine());
        Console.Write("Enter product name: ");
        product.ProductName = Console.ReadLine();
        Console.Write("Enter description: ");
        product.Description = Console.ReadLine();
        Console.Write("Enter price: ");
        product.Price = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO products (ProductID, CategoryID, ProductName, Description, Price) VALUES (@ProductID, @CategoryID, @ProductName, @Description, @Price)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductID", product.ProductID);
            command.Parameters.AddWithValue("@CategoryID", product.CategoryID);
            command.Parameters.AddWithValue("@ProductName", product.ProductName);
            command.Parameters.AddWithValue("@Description", product.Description);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.ExecuteNonQuery();
        }

        productList.Add(product);
        Console.WriteLine("Product added successfully!");
    }
    static void EditProductInfo(List<Product> productList, string connectionString)
    {
        Console.WriteLine("Enter product ID to edit: ");
        int productID = int.Parse(Console.ReadLine());
        Product product = productList.Find(p => p.ProductID == productID);

        if (product != null)
        {
            Console.WriteLine("Enter new information: ");
            Console.Write("Enter category ID: ");
            product.CategoryID = int.Parse(Console.ReadLine());
            Console.Write("Enter product name: ");
            product.ProductName = Console.ReadLine();
            Console.Write("Enter description: ");
            product.Description = Console.ReadLine();
            Console.Write("Enter price: ");
            product.Price = int.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE product SET CategoryID = @CategoryID, ProductName = @ProductName, Description = @Description, Price = @Price WHERE ProductID = @ProductID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductID", product.ProductID);
                command.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.ExecuteNonQuery();
            }
            Console.WriteLine("Product information updated successfully!");
        }
        else
        {
            Console.WriteLine("Product with this ID not found.");
        }
    }
    static void DeleteProductInfo(List<Product> productList, string connectionString)
    {
        Console.WriteLine("Enter product ID to delete: ");
        int productID = int.Parse(Console.ReadLine());

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM product WHERE ProductID = @ProductID";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " product(s) deleted from the database.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
    static void SearchProductByID(List<Product> productList, string connectionString)
    {
        Console.Clear();
        DrawMenuFrame("Choose search criteria: ", new string[] {
            "1. Search by product ID",
            "2. Search by product name",
            "3. Search by category ID",
            "4. Search by price range",
            "Choose criteria: ",
        });
            
        int choice = int.Parse(Console.ReadLine());

        string query = "";
        MySqlCommand command = new MySqlCommand();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            switch (choice)
            {
                case 1:
                    Console.Write("Enter product ID: ");
                    int productID = int.Parse(Console.ReadLine());
                    query = "SELECT * FROM product WHERE ProductID = @ProductID";
                    command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductID", productID);
                    break;
                case 2:
                    Console.Write("Enter product name: ");
                    string productName = Console.ReadLine();
                    query = "SELECT * FROM product WHERE ProductName LIKE @ProductName";
                    command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductName", "%" + productName + "%");
                    break;
                case 3:
                    Console.Write("Enter category ID: ");
                    int categoryID = int.Parse(Console.ReadLine());
                    query = "SELECT * FROM product WHERE CategoryID = @CategoryID";
                    command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CategoryID", categoryID);
                    break;
                case 4:
                    Console.Write("Enter minimum price: ");
                    int minPrice = int.Parse(Console.ReadLine());
                    Console.Write("Enter maximum price: ");
                    int maxPrice = int.Parse(Console.ReadLine());
                    query = "SELECT * FROM product WHERE Price BETWEEN @MinPrice AND @MaxPrice";
                    command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MinPrice", minPrice);
                    command.Parameters.AddWithValue("@MaxPrice", maxPrice);
                    break;
                default:
                    Console.WriteLine("Invalid criteria.");
                    return;
            }

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("-----Search Results-----");
                while (reader.Read())
                {
                    Console.WriteLine($"Product ID: {reader["id_product"]}, Category ID: {reader["id_category"]}, Product Name: {reader["ProductName"]}, Description: {reader["Description"]}, Price: {reader["Price"]}, Image: {reader["image"]}");
                }
            }
        }
    }
    
    static void DisplayCategory(List<CategoryOfProduct> categoryList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM category";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("-----Category List-----");
            while (reader.Read())
            {
                Console.WriteLine($"Category ID: {reader["CategoryID"]}, Category Name: {reader["CategoryName"]}");
            }
            reader.Close();
        }
    }
    static void AddCategory(List<CategoryOfProduct> categoryList)
    {
        Console.WriteLine("Enter information for the new category: ");
        CategoryOfProduct category = new CategoryOfProduct();
        Console.Write("Enter category ID: ");
        category.CategoryID = int.Parse(Console.ReadLine());
        Console.Write("Enter category name: ");
        category.CategoryName = Console.ReadLine();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO category (CategoryID, CategoryName) VALUES (@CategoryID, @CategoryName)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CategoryID", category.CategoryID);
            command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
            command.ExecuteNonQuery();
        }

        categoryList.Add(category);
        Console.WriteLine("Category added successfully!");
    }
    static void EditCategoryInfo(List<CategoryOfProduct> categoryList, string connectionString)
    {
        Console.WriteLine("Enter category ID to edit: ");
        int categoryID = int.Parse(Console.ReadLine());
        CategoryOfProduct category = categoryList.Find(c => c.CategoryID == categoryID);

        if (category != null)
        {
            Console.WriteLine("Enter new information: ");
            Console.Write("Enter category name: ");
            category.CategoryName = Console.ReadLine();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE category SET CategoryName = @CategoryName WHERE CategoryID = @CategoryID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryID", category.CategoryID);
                command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                command.ExecuteNonQuery();
            }
            Console.WriteLine("Category information updated successfully!");
        }
        else
        {
            Console.WriteLine("Category with this ID not found.");
        }
    }
    static void DeleteCategoryInfo(List<CategoryOfProduct> categoryList, string connectionString)
    {
        Console.WriteLine("Enter category ID to delete: ");
        int categoryID = int.Parse(Console.ReadLine());

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM category WHERE CategoryID = @CategoryID";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " category(s) deleted from the database.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
    static void SearchCategoryByID(List<CategoryOfProduct> categoryList, string connectionString)
    {
        Console.Write("Enter category ID to find: ");
        int categoryID = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM category WHERE CategoryID = @CategoryID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CategoryID", categoryID);

            connection.Open();
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine("Category Information:");
                    Console.WriteLine($"Category Name: {reader["CategoryName"]}");
                }
                else
                {
                    Console.WriteLine("No category found with this ID.");
                }
            }
        }
    }

    
    static void DisplayWarehouse(List<Warehouse> warehouseList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM warehouse";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("-----Warehouse List-----");
            while (reader.Read())
            {
                Console.WriteLine($"Warehouse ID: {reader["WarehouseID"]}, Product ID: {reader["ProductID"]}, Quantity: {reader["quantity"]}");
            }
            reader.Close();
        }
    }
    static void AddWarehouseInfo(List<Warehouse> warehouseList, string connectionString)
    {
        Console.WriteLine("Enter new warehouse information: ");
        Warehouse wh = new Warehouse();
        Console.Write("Enter warehouse ID: ");
        wh.WareHouseID = int.Parse(Console.ReadLine());
        Console.Write("Enter product ID: ");
        wh.ProductID = int.Parse(Console.ReadLine());
        Console.Write("Enter quantity: ");
        wh.Quantity = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO warehouse (id_wareWarehouseIDhouse, ProductID, Quantity) VALUES (@WarehouseID, @ProductID, @Quantity)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@WarehouseID", wh.WareHouseID);
            command.Parameters.AddWithValue("@ProductName", wh.ProductID);
            command.Parameters.AddWithValue("@Quantity", wh.Quantity);
            command.ExecuteNonQuery();
        }

        warehouseList.Add(wh);
        Console.WriteLine("Warehouse information added successfully!");
    }
    static void EditWarehouseInfo(List<Warehouse> warehouseList, string connectionString)
    {
        Console.WriteLine("Enter warehouse ID to edit: ");
        int warehouseID = int.Parse(Console.ReadLine());
        Warehouse wh = warehouseList.Find(w => w.WareHouseID == warehouseID);
        if (wh != null)
        {
            Console.WriteLine("Enter new information: ");
            Console.Write("Enter product name: ");
            wh.ProductID = int.Parse(Console.ReadLine());
            Console.Write("Enter quantity: ");
            wh.Quantity = int.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE warehouse SET ProductID=@ProductID, Quantity=@Quantity WHERE WareHouseID=@WarehouseID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@WarehouseID", wh.WareHouseID);
                command.Parameters.AddWithValue("@ProductID", wh.ProductID);
                command.Parameters.AddWithValue("@Quantity", wh.Quantity);
                command.ExecuteNonQuery();
            }
            Console.WriteLine("Warehouse information updated successfully!");
        }
        else
        {
            Console.WriteLine("Warehouse with this ID not found.");
        }
    }
    static void DeleteWarehouseInfo(List<Warehouse> warehouseList, string connectionString)
    {
        Console.WriteLine("Enter warehouse ID to delete: ");
        int warehouseID = int.Parse(Console.ReadLine());

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM warehouse WHERE WarehouseID = @WarehouseID";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@WarehouseID", warehouseID);
                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " warehouse(s) deleted from the database.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
    static void SearchWarehouseByID(List<Warehouse> warehouseList, string connectionString)
    {
        Console.Write("Enter warehouse ID to find: ");
        int warehouseID = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM warehouse WHERE WarehouseID = @WarehouseID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@WarehouseID", warehouseID);

            connection.Open();
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine("Warehouse Information:");
                    Console.WriteLine($"Product ID: {reader["ProductID"]}");
                    Console.WriteLine($"Quantity: {reader["Quantity"]}");
                }
                else
                {
                    Console.WriteLine("No warehouse found with this ID.");
                }
            }
        }
    }
       
    
    static void DisplayCoupons(List<Coupon> couponList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM coupon";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("----- Coupon List -----");
            while (reader.Read())
            {
                Console.WriteLine($"Coupon ID: {reader["id_coupon"]}, Coupon Code: {reader["coupon_code"]}, Discount Amount: {reader["discount_amount"]}, Is Active: {reader["is_active"]}");
            }
            reader.Close();
        }
    }
    static void AddCoupon(List<Coupon> couponList)
    {
        Console.WriteLine("Enter information for the new coupon: ");
        Coupon coupon = new Coupon();
        Console.Write("Enter coupon code: ");
        coupon.CouponCode = Console.ReadLine();
        Console.Write("Enter discount amount: ");
        coupon.DiscountAmount = decimal.Parse(Console.ReadLine());
        coupon.IsActive = true;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO coupon (coupon_code, discount_amount, is_active) VALUES (@CouponCode, @DiscountAmount, @IsActive)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CouponCode", coupon.CouponCode);
            command.Parameters.AddWithValue("@DiscountAmount", coupon.DiscountAmount);
            command.Parameters.AddWithValue("@IsActive", coupon.IsActive);
            command.ExecuteNonQuery();
        }
        couponList.Add(coupon);
        Console.WriteLine("Coupon added successfully!");
    }
    static void EditCouponInfo(List<Coupon> couponList, string connectionString)
    {
        Console.Write("Enter the coupon ID to edit: ");
        int couponID = int.Parse(Console.ReadLine());
        Coupon coupon = couponList.Find(c => c.CouponID == couponID);

        if (coupon != null)
        {
            Console.WriteLine("Enter new information: ");
            Console.Write("Enter coupon code: ");
            coupon.CouponCode = Console.ReadLine();
            Console.Write("Enter discount amount: ");
            coupon.DiscountAmount = decimal.Parse(Console.ReadLine());
            Console.Write("Is the coupon active (1 for active, 0 for inactive): ");
            coupon.IsActive = Console.ReadLine() == "1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE coupon SET coupon_code = @CouponCode, discount_amount = @DiscountAmount, is_active = @IsActive WHERE id_coupon = @CouponID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CouponID", coupon.CouponID);
                command.Parameters.AddWithValue("@CouponCode", coupon.CouponCode);
                command.Parameters.AddWithValue("@DiscountAmount", coupon.DiscountAmount);
                command.Parameters.AddWithValue("@IsActive", coupon.IsActive);
                command.ExecuteNonQuery();
            }
            Console.WriteLine("Coupon information updated successfully!");
        }
        else
        {
            Console.WriteLine("Coupon with this ID not found.");
        }
    }
    static void DeleteCoupon(List<Coupon> couponList, string connectionString)
    {
        Console.Write("Enter the coupon ID to delete: ");
        int couponID = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "DELETE FROM coupon WHERE id_coupon = @CouponID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CouponID", couponID);
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Coupon coupon = couponList.Find(c => c.CouponID == couponID);
                couponList.Remove(coupon);
                Console.WriteLine("Coupon deleted successfully!");
            }
            else
            {
                Console.WriteLine("Coupon with this ID not found.");
            }
        }
    }
    static void SearchCouponByID(List<Coupon> couponList, string connectionString)
    {
        Console.Write("Enter the coupon ID to search: ");
        int couponID = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM coupon WHERE id_coupon = @CouponID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CouponID", couponID);

            connection.Open();
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine("Coupon Information:");
                    Console.WriteLine($"Coupon ID: {reader["id_coupon"]}");
                    Console.WriteLine($"Coupon Code: {reader["coupon_code"]}");
                    Console.WriteLine($"Discount Amount: {reader["discount_amount"]}");
                    Console.WriteLine($"Is Active: {reader["is_active"]}");
                }
                else
                {
                    Console.WriteLine("No coupon found with this ID.");
                }
            }
        }
    }
        
    
    static void DisplayPolicy(List<Policy> policyList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM policy";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("-----Policy List-----");
            while (reader.Read())
            {
                Console.WriteLine($"Policy ID: {reader["ID"]}, Policy Title: {reader["Title"]}, Content: {reader["Content"]}");
            }
            reader.Close();
        }
    }
    static void AddPolicy(List<Policy> policyList)
    {
        Console.WriteLine("Enter new policy information: ");
        Policy policy = new Policy();
        Console.Write("Enter policy ID: ");
        policy.ID = int.Parse(Console.ReadLine());
        Console.Write("Enter policy title: ");
        policy.Title = Console.ReadLine();
        Console.Write("Enter policy content: ");
        policy.Content = Console.ReadLine();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO policy (ID, Title, Content) VALUES (@PolicyID, @PolicyTitle, @PolicyContent)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@PolicyID", policy.ID);
            command.Parameters.AddWithValue("@PolicyTitle", policy.Title);
            command.Parameters.AddWithValue("@PolicyContent", policy.Content);
            command.ExecuteNonQuery();
        }

        policyList.Add(policy);
        Console.WriteLine("Added a new policy!");
    }
    static void EditPolicy(List<Policy> policyList, string connectionString)
    {
        Console.Write("Enter the policy ID to edit: ");
        int policyID = int.Parse(Console.ReadLine());
        Policy policy = policyList.Find(p => p.ID == policyID);

        if (policy != null)
        {
            Console.Write("Enter new policy title: ");
            policy.Title = Console.ReadLine();
            Console.Write("Enter new policy content: ");
            policy.Content = Console.ReadLine();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE policy SET Title = @PolicyTitle, Content = @PolicyContent WHERE ID = @PolicyID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@PolicyID", policy.ID);
                command.Parameters.AddWithValue("@PolicyTitle", policy.Title);
                command.Parameters.AddWithValue("@PolicyContent", policy.Content);
                command.ExecuteNonQuery();
            }
            Console.WriteLine("Policy updated successfully!");
        }
        else
        {
            Console.WriteLine("Policy with this ID not found.");
        }
    }
    static void DeletePolicy(List<Policy> policyList, string connectionString)
    {
        Console.Write("Enter the policy ID to delete: ");
        int policyID = int.Parse(Console.ReadLine());

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM policy WHERE ID = @PolicyID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@PolicyID", policyID);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " policy(ies) deleted.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
        
    
    static void DisplayIssues(List<Issue> issueList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM customer_support";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("-----Support Issues List-----");
            while (reader.Read())
            {
                Console.WriteLine($"Issue ID: {reader["ID"]}, Issue Title: {reader["IssueTitle"]}, Issue Description: {reader["IssueContent"]}, Issue Date: {reader["CreatedAt"]}");
            }
            reader.Close();
        }
    }
    static void AddIssue(List<Issue> issueList, string connectionString)
    {
        Console.WriteLine("Enter the information of the issue encountered: ");
        Issue issue = new Issue();
        Console.Write("Enter issue ID: ");
        issue.ID = int.Parse(Console.ReadLine());
        Console.Write("Enter issue title: ");
        issue.IssueTitle = Console.ReadLine();
        Console.Write("Enter issue description: ");
        issue.IssueContent = Console.ReadLine();
        Console.Write("Enter issue date (yyyy-MM-dd HH:mm:ss): ");
        issue.CreatedAt = DateTime.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO customer_support (ID, IssueTitle, IssueContent, CreatedAt) VALUES (@IssueID, @IssueTitle, @IssueDescription, @IssueDate)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@IssueID", issue.ID);
            command.Parameters.AddWithValue("@IssueTitle", issue.IssueTitle);
            command.Parameters.AddWithValue("@IssueDescription", issue.IssueContent);
            command.Parameters.AddWithValue("@IssueDate", issue.CreatedAt);
            command.ExecuteNonQuery();
        }

        issueList.Add(issue);
        Console.WriteLine("Issue has been submitted to the support team!");
    }
    static void EditIssue(List<Issue> issueList, string connectionString)
    {
        Console.Write("Enter the issue ID to edit: ");
        int issueID = int.Parse(Console.ReadLine());
        Issue issue = issueList.Find(i => i.ID == issueID);

        if (issue != null)
        {
            Console.Write("Enter new issue title: ");
            issue.IssueTitle = Console.ReadLine();
            Console.Write("Enter new issue description: ");
            issue.IssueContent = Console.ReadLine();
            Console.Write("Enter issue date (yyyy-MM-dd HH:mm:ss): ");
            issue.CreatedAt = DateTime.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE customer_support SET IssueTitle = @IssueTitle, IssueContent = @IssueDescription, CreatedAt = @IssueDate WHERE ID = @IssueID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@IssueID", issue.ID);
                command.Parameters.AddWithValue("@IssueTitle", issue.IssueTitle);
                command.Parameters.AddWithValue("@IssueDescription", issue.IssueContent);
                command.Parameters.AddWithValue("@IssueDate", issue.CreatedAt);
                command.ExecuteNonQuery();
            }
            Console.WriteLine("Issue information has been updated!");
        }
        else
        {
            Console.WriteLine("Issue with this ID not found.");
        }
    }
    static void DeleteIssue(List<Issue> issueList, string connectionString)
    {
        Console.Write("Enter the issue ID to delete: ");
        int issueID = int.Parse(Console.ReadLine());

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM customer_support WHERE ID = @IssueID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@IssueID", issueID);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " issue(s) deleted from the system.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
        
    
    static void DisplayInvoice(List<Invoice> invoiceList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM invoice";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("----- Invoice List -----");
            while (reader.Read())
            {
                Console.WriteLine($"Invoice ID: {reader["InvoiceID"]},Product ID: {reader["ProductID"]},Customer ID: {reader["CustomerID"]}, Invoice Date: {reader["InvoiceDate"]}, Total Amount: {reader["TotalAmount"]}");
            }
            reader.Close();
        }
    }
    static void AddInvoice(List<Invoice> invoiceList)
    {
        Invoice invoice = new Invoice();
        Console.Write("Enter invoice ID: ");
        invoice.InvoiceID = int.Parse(Console.ReadLine());
        Console.Write("Enter product ID: ");
        invoice.ProductID = int.Parse(Console.ReadLine());
        Console.Write("Enter customer ID: ");
        invoice.CustomerID = int.Parse(Console.ReadLine());
        Console.Write("Enter invoice date (yyyy-MM-dd): ");
        invoice.InvoiceDate = DateTime.Parse(Console.ReadLine());
        Console.Write("Enter total amount: ");
        invoice.TotalAmount = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO invoice (InvoiceID,ProductID,CustomerID, InvoiceDate, TotalAmount) VALUES (@InvoiceID, @ProductID,@CustomerID, @InvoiceDate, @TotalAmount)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@InvoiceID", invoice.InvoiceID);
            command.Parameters.AddWithValue("@ProductID", invoice.ProductID);
            command.Parameters.AddWithValue("@CustomerID", invoice.CustomerID);
            command.Parameters.AddWithValue("@InvoiceDate", invoice.InvoiceDate);
            command.Parameters.AddWithValue("@TotalAmount", invoice.TotalAmount);
            command.ExecuteNonQuery();
        }

        invoiceList.Add(invoice);
        Console.WriteLine("Invoice added successfully!");
    }
    static void EditInvoice(List<Invoice> invoiceList,string connectionString)
    {
        Console.Write("Enter the invoice ID to edit: ");
        int invoiceID = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM invoice WHERE InvoiceID = @InvoiceID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@InvoiceID", invoiceID);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine("Enter new information: ");
                    Console.Write("Enter product ID: ");
                    int productID = int.Parse(Console.ReadLine());
                    Console.Write("Enter customer ID: ");
                    int customerID = int.Parse(Console.ReadLine());
                    Console.Write("Enter invoice date (yyyy-MM-dd): ");
                    DateTime invoiceDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("Enter total amount: ");
                    int totalAmount = int.Parse(Console.ReadLine());

                    reader.Close();

                    query = "UPDATE Invoice SET ProductID = @ProductID,CustomerID = @CustomerID, InvoiceDate = @InvoiceDate, TotalAmount = @TotalAmount WHERE InvoiceID = @InvoiceID";
                    command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@InvoiceID", invoiceID);
                    command.Parameters.AddWithValue("@ProductID", productID);
                    command.Parameters.AddWithValue("@CustomerID", customerID);
                    command.Parameters.AddWithValue("@InvoiceDate", invoiceDate);
                    command.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    command.ExecuteNonQuery();

                    Console.WriteLine("Invoice information updated successfully!");
                }
                else
                {
                    Console.WriteLine("No invoice found with this ID.");
                }
            }
        }
    }
    static void DeleteInvoice(List<Invoice> invoiceList,string connectionString)
    {
        Console.Write("Enter the invoice ID to delete: ");
        int invoiceID = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "DELETE FROM Invoice WHERE InvoiceID = @InvoiceID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@InvoiceID", invoiceID);
            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine(rowsAffected + " invoice(s) deleted from the database.");
        }
    }
    static void SearchInvoice(List<Invoice> invoiceList,string connectionString)
    {
        Console.Write("Enter the invoice ID to search: ");
        int invoiceID = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM invoice WHERE InvoiceID = @InvoiceID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@InvoiceID", invoiceID);

            connection.Open();
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine("Invoice Information:");
                    Console.WriteLine($"Invoice ID: {reader["InvoiceID"]}");
                    Console.WriteLine($"Product ID: {reader["ProductID"]}");
                    Console.WriteLine($"Customer ID: {reader["CustomerID"]}");
                    Console.WriteLine($"Invoice Date: {reader["InvoiceDate"]}");
                    Console.WriteLine($"Total Amount: {reader["TotalAmount"]}");
                }
                else
                {
                    Console.WriteLine("No invoice found with this ID.");
                }
            }
        }
    }

    
    static void DisplayOrder(List<Order> orderList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM order";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("----- Order List -----");
            while (reader.Read())
            {
                Console.WriteLine($"Order ID: {reader["OrderID"]},Customer ID: {reader["CustomerID"]},Product ID: {reader["ProductID"]},Coupon Code: {reader["SaleID"]},Order Quantity: {reader["OrderQuantity"]}, Order Date: {reader["OrderDate"]}, Shipping Address: {reader["ShippingAddress"]}, Total Amount: {reader["TotalAmount"]}");
            }
            reader.Close();
        }
    }
    static void AddOrder(List<Order> orderList, string connectionString)
    {
        Order order = new Order();
        Console.Write("Enter order ID: ");
        order.OrderID = int.Parse(Console.ReadLine());
        Console.Write("Enter customer ID: ");
        order.CustomerID = int.Parse(Console.ReadLine());
        Console.Write("Enter product ID: ");
        order.ProductID = int.Parse(Console.ReadLine());
        Console.Write("Enter sale ID: ");
        order.SaleID = int.Parse(Console.ReadLine());
        Console.Write("Enter order quantity ID: ");
        order.OrderQuantity = int.Parse(Console.ReadLine());
        Console.Write("Enter order date (yyyy-MM-dd): ");
        order.OrderDate = DateTime.Parse(Console.ReadLine());
        Console.Write("Enter shipping address: ");
        order.ShippingAddress = Console.ReadLine();
        Console.Write("Enter total amount: ");
        order.TotalAmount = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO order (OrderID, CustomerID, ProductID, SaleID,OrderQuantity, OrderDate, ShippingAddress, TotalAmount) VALUES (@OrderID, @CustomerID, @ProductID, @SaleID,OrderQuantity, @OrderDate, @ShippingAddress, @TotalAmount)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@OrderID", order.OrderID);
            command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
            command.Parameters.AddWithValue("@ProductID", order.ProductID);
            command.Parameters.AddWithValue("@SaleID", order.SaleID);
            command.Parameters.AddWithValue("@OrderQuantity", order.OrderQuantity);
            command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
            command.Parameters.AddWithValue("@ShippingAddress", order.ShippingAddress);
            command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
            command.ExecuteNonQuery();
        }

        orderList.Add(order);
        Console.WriteLine("Order added successfully!");
    }
    static void EditOrderInfo(List<Order> orderList, string connectionString)
    {
        Console.Write("Enter the order ID to edit: ");
        int orderID = int.Parse(Console.ReadLine());
        Order order = orderList.Find(o => o.OrderID == orderID);

        if (order != null)
        {
            Console.WriteLine("Enter new information: ");
            Console.Write("Enter customer ID: ");
            order.CustomerID = int.Parse(Console.ReadLine());
            Console.Write("Enter product ID: ");
            order.ProductID = int.Parse(Console.ReadLine());
            Console.Write("Enter sale ID: ");
            order.SaleID = int.Parse(Console.ReadLine());
            Console.Write("Enter order quantity ID: ");
            order.OrderQuantity = int.Parse(Console.ReadLine());
            Console.Write("Enter order date (yyyy-MM-dd): ");
            order.OrderDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter delivery address: ");
            order.ShippingAddress = Console.ReadLine();
            Console.Write("Enter total amount: ");
            order.TotalAmount = int.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE order SET CustomerID = @CustomerID, ProductID = @ProductID, SaleID = @SaleID,OrderQuantity = @OrderQuantity, OrderDate = @OrderDate, ShippingAddress = @ShippingAddress, TotalAmount = @TotalAmount WHERE OrderID = @OrderID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrderID", order.OrderID);
                command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                command.Parameters.AddWithValue("@ProductID", order.ProductID);
                command.Parameters.AddWithValue("@SaleID", order.SaleID);
                command.Parameters.AddWithValue("@OrderQuantity", order.OrderQuantity);
                command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                command.Parameters.AddWithValue("@ShippingAddress", order.ShippingAddress);
                command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                command.ExecuteNonQuery();
            }
            Console.WriteLine("Order information updated successfully!");
        }
        else
        {
            Console.WriteLine("Order with this ID not found.");
        }
    }
    static void DeleteOrderInfo(List<Order> orderList, string connectionString)
    {
        Console.Write("Enter the order ID to delete: ");
        int orderID = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "DELETE FROM order WHERE OrderID = @OrderID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@OrderID", orderID);
            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine(rowsAffected + " order deleted from the database.");
        }

        Order order = orderList.Find(o => o.OrderID == orderID);
        if (order != null)
        {
            orderList.Remove(order);
        }
    }
    static void SearchOrderByID(List<Order> orderList, string connectionString)
    {
        Console.Write("Enter the order ID to search: ");
        int orderID = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM order WHERE OrderID = @Order";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Order", orderID);

            connection.Open();
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine("Order Information:");
                    Console.WriteLine($"Order ID: {reader["OrderID"]}");
                    Console.WriteLine($"Customer ID: {reader["CustomerID"]}");
                    Console.WriteLine($"Product ID: {reader["ProductID"]}");
                    Console.WriteLine($"Sale ID: {reader["SaleID"]}");
                    Console.WriteLine($"Order Date: {reader.GetDateTime("OrderDate")}");
                    Console.WriteLine($"Shipping Address: {reader["ShippingAddress"]}");
                    Console.WriteLine($"Total Amount: {reader["TotalAmount"]}");
                }
                else
                {
                    Console.WriteLine("No order found with this ID.");
                }
            }
        }
    }
    static void ApplyCouponToOrder(int orderID, string couponCode, string connectionString)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Lấy tổng số tiền của đơn hàng
            string query = "SELECT TotalAmount FROM `order` WHERE id_order = @OrderID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@OrderID", orderID);
            decimal orderTotal = (decimal)command.ExecuteScalar();

            // Lấy thông tin phiếu giảm giá
            query = "SELECT discount_amount, is_percentage, is_active FROM coupon WHERE coupon_code = @CouponCode";
            command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CouponCode", couponCode);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read() && (bool)reader["is_active"])
                {
                    decimal discountAmount = (decimal)reader["discount_amount"];
                    bool isPercentage = (bool)reader["is_percentage"];
                    decimal discount = isPercentage ? orderTotal * discountAmount / 100 : discountAmount;

                    reader.Close();

                    // Áp dụng giảm giá vào đơn hàng
                    decimal newTotal = orderTotal - discount;
                    query = "UPDATE `order` SET TotalAmount = @NewTotal WHERE id_order = @OrderID";
                    command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NewTotal", newTotal);
                    command.Parameters.AddWithValue("@OrderID", orderID);
                    command.ExecuteNonQuery();

                    // Hủy kích hoạt phiếu giảm giá
                    query = "UPDATE coupon SET is_active = FALSE WHERE coupon_code = @CouponCode";
                    command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CouponCode", couponCode);
                    command.ExecuteNonQuery();

                    Console.WriteLine($"Áp dụng mã giảm giá thành công! Tổng tiền mới: {newTotal}");
                }
                else
                {
                    Console.WriteLine("Mã giảm giá không hợp lệ hoặc đã hết hạn.");
                }
            }
        }
    }

    
    static void DisplayExportReceipt(List<ExportReceipt> exportReceiptList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM export_receipt";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("----- Export Receipt List -----");
            while (reader.Read())
            {
                Console.WriteLine($"ExportReceipt ID: {reader["ExportReceiptID"]},Product ID: {reader["ProductID"]}, Export Date: {reader["ExportReceiptDate"]}, Shipping Address: {reader["ShippingAddress"]}, Total Amount: {reader["TotalAmount"]}");
            }
            reader.Close();
        }
    }
    static void AddExportReceipt(List<ExportReceipt> exportReceiptList)
    {
        ExportReceipt er = new ExportReceipt();
        Console.Write("Enter ExportReceipt ID: ");
        er.ExportReceiptID = int.Parse(Console.ReadLine());
        Console.Write("Enter product ID: ");
        er.ExportReceiptID = int.Parse(Console.ReadLine());
        Console.Write("Enter export date (yyyy-MM-dd): ");
        er.ExportDate = DateTime.Parse(Console.ReadLine());
        Console.Write("Enter shipping address: ");
        er.ShippingAddress = Console.ReadLine();
        Console.Write("Enter total amount: ");
        er.TotalAmount = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO export_receipt (ExportReceiptID,ProductID, ExportReceiptDate, ShippingAddress, TotalAmount) VALUES (@ExportReceiptID, @ProductID, @ExportReceiptDate, @ShippingAddress, @TotalAmount)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ExportReceiptID", er.ExportReceiptID);
            command.Parameters.AddWithValue("@ProductID", er.ProductID);
            command.Parameters.AddWithValue("@ExportReceiptDate", er.ExportDate);
            command.Parameters.AddWithValue("@ShippingAddress", er.ShippingAddress);
            command.Parameters.AddWithValue("@TotalAmount", er.TotalAmount);
            command.ExecuteNonQuery();
        }

        Console.WriteLine("Export receipt added successfully!");
    }
    static void EditExportReceiptInfo(List<ExportReceipt> exportReceiptList, string connectionString)
    {
        Console.Write("Enter the ExportReceipt ID to edit: ");
        int receiptID = int.Parse(Console.ReadLine());
        ExportReceipt er = exportReceiptList.Find(o => o.ExportReceiptID == receiptID);

        if (er != null)
        {
            Console.WriteLine("Enter new information: ");
            Console.Write("Enter Product ID: ");
            er.ProductID = int.Parse(Console.ReadLine());
            Console.Write("Enter export date (yyyy-MM-dd): ");
            er.ExportDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter shipping address: ");
            er.ShippingAddress = Console.ReadLine();
            Console.Write("Enter total amount: ");
            er.TotalAmount = int.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE export_receipt SET ProductID = @ProductID, ExportReceiptDate = @ExportReceiptDate, ShippingAddress = @ShippingAddress, TotalAmount = @TotalAmount WHERE ExportReceiptID = @ExportReceiptID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ExportReceiptID", er.ExportReceiptID);
                command.Parameters.AddWithValue("@ProductID", er.ProductID);
                command.Parameters.AddWithValue("@ExportReceiptDate", er.ExportDate);
                command.Parameters.AddWithValue("@ShippingAddress", er.ShippingAddress);
                command.Parameters.AddWithValue("@TotalAmount", er.TotalAmount);
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Export receipt information updated successfully!");
        }
        else
        {
            Console.WriteLine("No Export receipt found with this ID.");
        }
    }
    static void DeleteExportReceiptInfo(List<ExportReceipt> exportReceiptList,string connectionString)
    {
        Console.Write("Enter the export receipt ID to delete: ");
        int receiptID = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "DELETE FROM export_receipt WHERE ExportReceiptID = @ExportReceiptID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ExportReceiptID", receiptID);
            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine(rowsAffected + " Export receipt deleted from the database.");
        }
    }
    static void SearchExportReceiptByID(List<ExportReceipt> exportReceiptList,string connectionString)
    {
        Console.Write("Enter the export receipt ID to search: ");
        int receiptID = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM export_receipt WHERE ExportReceiptID = @ExportReceiptID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ExportReceiptID", receiptID);

            connection.Open();
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine("Export Receipt Information:");
                    Console.WriteLine($"ExportReceipt ID: {reader["ExportReceiptID"]}");
                    Console.WriteLine($"Product ID: {reader["ProductID"]}");
                    Console.WriteLine($"Export Date: {reader["ExportReceiptDate"]}");
                    Console.WriteLine($"Delivery Address: {reader["ShippingAddress"]}");
                    Console.WriteLine($"Total Amount: {reader["TotalAmount"]}");
                }
                else
                {
                    Console.WriteLine("No export receipt found with this ID.");
                }
            }
        }
    }
    
    
    static void DisplayImportReceipt(List<ImportReceipt> importReceiptList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM import_receipt";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("----- Import Receipt List -----");
            while (reader.Read())
            {
                Console.WriteLine($"ImportReceipt ID: {reader["ImportReceiptID"]},Product ID: {reader["ProductID"]}, Import Date: {reader["ImportReceiptDate"]}, Total Amount: {reader["TotalAmount"]}");
            }
            reader.Close();
        }
    }
    static void AddImportReceipt(List<ImportReceipt> importReceiptList)
    {
        ImportReceipt ir = new ImportReceipt();
        Console.Write("Enter receipt ID: ");
        ir.ImportReceiptID = int.Parse(Console.ReadLine());
        Console.Write("Enter receipt ID: ");
        ir.ProductID = int.Parse(Console.ReadLine());
        Console.Write("Enter receipt date (yyyy-MM-dd): ");
        ir.ImportDate = DateTime.Parse(Console.ReadLine());
        Console.Write("Enter total amount: ");
        ir.TotalAmount = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO import_receipt (ImportReceiptID,ProductID, ImportReceiptDate, TongTien) VALUES (@ImportReceiptID, @ProductID, @ImportReceiptDate,@TotalAmount)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ImportReceiptID", ir.ImportReceiptID);
            command.Parameters.AddWithValue("@ProductID", ir.ProductID);
            command.Parameters.AddWithValue("@ImportReceiptDate", ir.ImportDate);
            command.Parameters.AddWithValue("@TotalAmount", ir.TotalAmount);
            command.ExecuteNonQuery();
        }

        Console.WriteLine("Import receipt added successfully!");
    }
    static void EditImportReceiptInfo(List<ImportReceipt> importReceiptList,string connectionString)
    {
        Console.Write("Enter the receipt ID to edit: ");
        int receiptID = int.Parse(Console.ReadLine());
        ImportReceipt ir = importReceiptList.Find(o => o.ImportReceiptID == receiptID);

        if (ir != null)
        {
            Console.WriteLine("Enter new information: ");
            Console.Write("Enter Product ID: ");
            ir.ProductID = int.Parse(Console.ReadLine());
            Console.Write("Enter ImportDate: ");
            ir.ImportDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter total amount: ");
            int totalAmount = int.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE import_receipt SET ProductID = @ProductID, ImportReceiptDate = @ImportReceiptDate, TotalAmount = @TotalAmount WHERE ImportReceiptID = @ImportReceiptID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ImportReceiptID", ir.ImportReceiptID);
                command.Parameters.AddWithValue("@ProductID", ir.ProductID);
                command.Parameters.AddWithValue("@ImportReceiptDate", ir.ImportDate);
                command.Parameters.AddWithValue("@TotalAmount", ir.TotalAmount);
                command.ExecuteNonQuery();

                Console.WriteLine("Import receipt information updated successfully!");
            }
        }
        else
        {
            Console.WriteLine("No Import receipt found with this ID.");
        }
    }
    static void DeleteImportReceiptInfo(List<ImportReceipt> importReceiptList,string connectionString)
    {
        Console.Write("Enter the receipt ID to delete: ");
        int receiptID = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "DELETE FROM import_receipt WHERE ImportReceiptID = @ImportReceiptID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ImportReceiptID", receiptID);
            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine(rowsAffected + " import receipt deleted from the database.");
        }
    }
    static void SearchImportReceiptByID(List<ImportReceipt> importReceiptList,string connectionString)
    {
        Console.Write("Enter the receipt ID to search: ");
        int receiptID = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM import_receipt WHERE ImportReceiptID = @ImportReceipt";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ImportReceipt", receiptID);

            connection.Open();
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine("Warehouse Receipt Information:");
                    Console.WriteLine($"ImportReceipt ID: {reader["ImportReceiptID"]}");
                    Console.WriteLine($"Product ID: {reader["ProductID"]}");
                    Console.WriteLine($"Import Date: {reader["ImportReceiptDate"]}");
                    Console.WriteLine($"Total Amount: {reader["TotalAmount"]}");
                }
                else
                {
                    Console.WriteLine("No import receipt found with this ID.");
                }
            }
        }
    }
}
