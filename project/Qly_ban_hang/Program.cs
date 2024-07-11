using System;
using System.Collections.Generic;
using System.Net;
using MySql.Data.MySqlClient;
using ManagermentSale.Models;
using System.Text.RegularExpressions;
using Spectre.Console;
class Program
{
    enum Gender
    {
        Male,
        Female,
        Others
    }
        
    public static string connectionString = "server=localhost;user=root;password=Binh6179;database=qly_ban_hang;";
    const int maxAttempts = 5;
    static int invalidAttempts = 0;

    static string PromptForValidInput(string prompt, Func<string, bool> isValid, Func<string, bool> isExists = null)
    {
        int attempts = 0;
        while (attempts < maxAttempts)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (isValid(input))
            {
                if (isExists != null && isExists(input))
                {
                    Console.WriteLine("The value already exists in the system. Please re-enter.");
                    attempts++;
                }
                else
                {
                    return input;
                }
            }
            else
            {
                attempts++;
            }
        }
        Console.WriteLine("You have entered incorrectly more than 7 times.");
        return null;
    }
    static void Main(string[] args)
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Unsigned[/]")
                    .AddChoices(new[]
                    {
                        "Login",
                        "Register",
                        "Forgot Password",
                        "Exit"
                    }));

            switch (choice)
            {
                case "Login":
                    Login();
                    break;
                case "Register":
                    Register();
                    break;
                case "Forgot Password":
                    ForgotPassword();
                    break;
                case "Exit":
                    return;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid selection. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static bool IsUserNameExists(string UserName)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM users WHERE UserName = @LoginName";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@LoginName", UserName);

            long count = (long)command.ExecuteScalar();
            return count > 0;
        }
    }
    static bool IsPhoneNumberExists(string PhoneNumber)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM users WHERE PhoneNumber = @PhoneNumber";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("PhoneNumber", PhoneNumber);

            long count = (long)command.ExecuteScalar();
            return count > 0;
        }
    }
    static bool IsEmailExists(string Email)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM users WHERE Email = @Email";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", Email);

            long count = (long)command.ExecuteScalar();
            return count > 0;
        }
    }
    static bool IsValidUserName(string UserName)
    {
        string pattern = @"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z0-9]{7,16}$";
        Regex regex = new Regex(pattern);

        if (string.IsNullOrWhiteSpace(UserName))
        {
            Console.Write("Loggin name cannot be blank.");
            return false;
        }
        else if (UserName.Length < 7 || UserName.Length > 16)
        {
            Console.Write("Loggin name must be between 7 and 16 characters long.");
            return false;
        }
        else if (!regex.IsMatch(UserName))
        {
            Console.Write("Login name must contain at least one letter and one number, no special characters and spaces.");
            return false;
        }

        return true;
    }
    static bool IsValidPassword(string Password)
    {
        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[^\s]{6,15}$";
        Regex regex = new Regex(pattern);

        if (string.IsNullOrWhiteSpace(Password))
        {
            Console.Write("Password can not be blank.");
            return false;
        }

        else if (Password.Length < 6 || Password.Length > 15)
        {
            Console.Write("Password must be 6-15 characters long.");
            return false;
        }

        else if (!Regex.IsMatch(Password, @"[a-z]"))
        {
            Console.Write("Password must contain at least one lowercase letter and one uppercase letter.");
            return false;
        }
        else if (!Regex.IsMatch(Password, @"[A-Z]"))
        {
            Console.Write("Password must contain at least one uppercase letter.");
            return false;
        }

        else if (!Regex.IsMatch(Password, @"\d"))
        {
            Console.Write("Password must contain at least one digit.");
            return false;
        }

        else if (!Regex.IsMatch(Password, @"[@$!%*?&]"))
        {
            Console.Write("Password must contain at least one special character (allowed characters: @$!%*?&).");
            return false;
        }

        else if (Regex.IsMatch(Password, @"\s"))
        {
            Console.Write("Password cannot contain spaces.");
            return false;
        }

        return true;
    }
    static bool IsValidDateOfBirth(string DateOfBirth)
    {
        // Kiểm tra rỗng
        if (string.IsNullOrWhiteSpace(DateOfBirth))
        {
            Console.Write("Date of birth can not be blank.");
            return false;
        }

        string[] formats = { "yyyy-M-d", "yyyy-MM-d", "yyyy-M-dd", "yyyy-MM-dd" };

        // Kiểm tra định dạng yyyy-MM-dd
        DateTime dateOfBirth;
        if (!DateTime.TryParseExact(DateOfBirth, formats, null, System.Globalization.DateTimeStyles.None, out dateOfBirth))
        {
            Console.Write("Date of birth must be in format (YYYY-MM-DD)");
            return false;
        }

        return true;
    }
    static bool IsValidEmail(string Email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);

        if (string.IsNullOrWhiteSpace(Email))
        {
            Console.WriteLine("Email can not be blank.");
            return false;
        }
        else if (!regex.IsMatch(Email))
        {
            Console.WriteLine("Email must be in the format example@example.com");
            return false;
        }

        return true;
    }
    static bool IsValidPhoneNumber(string PhoneNumber)
    {
        string pattern = @"^\d{10}$";
        Regex regex = new Regex(pattern);

        if (string.IsNullOrWhiteSpace(PhoneNumber))
        {
            Console.WriteLine("Phone number can not be blank.");
            return false;
        }
        else if (!regex.IsMatch(PhoneNumber))
        {
            Console.WriteLine("Phone number in 10 digit format.");
            return false;
        }

        return true;
    }
    static bool IsValidName(string Name)
    {
        if (Name.Length < 2 || Name.Length > 15)
        {
            Console.WriteLine("Invalid input. Full name must be between 2 and 15 characters.");
            return false;
        }

        // Tách tên thành các phần riêng biệt
        string[] nameParts = Name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        // Phải có ít nhất hai phần (họ và tên)
        if (nameParts.Length < 2)
        {
            Console.WriteLine("Name must contain full first and last name");
            return false;
        }

        foreach (char c in Name)
        {
            if (!char.IsLetter(c) && c != ' ')
            {
                Console.WriteLine("Invalid input. Full name should only contain letters and spaces.");
                return false;
            }
        }

        return true;
    }
    static bool IsValidGender(string Gender)
    {
        if (string.IsNullOrEmpty(Gender))
        {
            Console.WriteLine("Gender cannot be empty. Please enter 'male', 'female', or 'others'.");
            return false;
        }
        switch (Gender)
        {
            case "male":
            case "female":
            case "others":
                return true;
            default:
                Console.WriteLine("Invalid format. Please enter 'male', 'female', or 'others'.");
                return false;
        }
    }
    static bool VerifyUserInfo(string UserName, string Email, string PhoneNumber, string DateOfBirth)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM users WHERE UserName = @LoginName AND Email = @Email AND PhoneNumber = @PhoneNumber AND DateOfBirth = @DateOfBirth";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@LoginName", UserName);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);

            long count = (long)command.ExecuteScalar();
            return count > 0;
        }
    }
    static bool UpdatePassword(string UserName, string Password)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "UPDATE users SET Password = @Password WHERE UserName = @LoginName";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@LoginName", UserName);
            command.Parameters.AddWithValue("@Password", Password);

            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }
    }
    static void Login()
    {
        string UserName ="";
        string Password ="";


        Console.WriteLine("Login:");
        Console.Write("Enter your login name: ");
        UserName = Console.ReadLine();
        Console.Write("Enter your password: ");
        Password = Console.ReadLine();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM users WHERE UserName = @LoginName AND Password = @Password";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@LoginName", UserName);
            command.Parameters.AddWithValue("@Password", Password);

            var role = command.ExecuteScalar() as string;
                if (role == null)
                {
                    Console.WriteLine("Tên người dùng hoặc mật khẩu không hợp lệ.");
                    invalidAttempts++;
                    if (invalidAttempts >= maxAttempts)
                    {
                        Console.WriteLine("Quá nhiều lần nhập không hợp lệ. Thoát.");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Console.WriteLine($"Đăng nhập thành công. Chào mừng, {role}!");
                    if (role == "admin")
                    {
                        // Logic cho admin
                        AdminMenu();
                    }
                    else if (role == "customer")
                    {
                        // Logic cho khách hàng
                        CustomerMenu();
                    }
                }
        }
    }
    static void Register()
    {
        string Email = "";
        string UserName = "";
        string Password = "";
        string PhoneNumber = "";
        string DateOfBirth = "";
        string Name = "";
        string Gender = "";
        string Address = "";

        Console.WriteLine("Register: ");
        while(invalidAttempts < maxAttempts)
        {
            Console.Write("Enter your loggin name : ");
            UserName = Console.ReadLine();
            if (IsValidUserName(UserName))
            {
                if (IsUserNameExists(UserName))
                {
                    Console.WriteLine("Loggin name already exists on the system. Please re-enter.");
                    invalidAttempts++;
                }
                else
                {
                    invalidAttempts = 0;
                    break;
                }
            }
            else
            {
                invalidAttempts++;
            }
        }
        if (invalidAttempts >= maxAttempts)
        {
            Console.WriteLine("You have entered incorrectly more than 7 times.");
            invalidAttempts = 0;
            return;
        }

        while(invalidAttempts < maxAttempts)
        {
            Console.Write("Enter your pasword: ");
            Password = Console.ReadLine();
            if (IsValidPassword(Password))
            {
                break;
            }
            else
            {
                invalidAttempts++;
            }
        }
        if (invalidAttempts >= maxAttempts)
        {
            Console.WriteLine("You have entered incorrectly more than 7 times.");
            invalidAttempts = 0;
            return;
        }

        while(invalidAttempts < maxAttempts)
        {
            Console.Write("Enter your Full name: ");
            Name = Console.ReadLine();
            if (IsValidName(Name))
            {
                break;
            }
            else
            {
                invalidAttempts++;
            }
        }
        if (invalidAttempts >= maxAttempts)
        {
            Console.WriteLine("You have entered incorrectly more than 7 times.");
            invalidAttempts = 0;
            return;
        }

        while(invalidAttempts < maxAttempts)
        {
            Console.Write("Enter your gender: ");
            Gender = Console.ReadLine().Trim().ToLower();
            if (IsValidGender(Gender))
            {
                break;
            }
            else
            {
                invalidAttempts++;
            }
        }
        if (invalidAttempts >= maxAttempts)
        {
            Console.WriteLine("You have entered incorrectly more than 7 times.");
            invalidAttempts = 0;
            return;
        }

        while(invalidAttempts < maxAttempts)
        {
            Console.WriteLine("Enter your date of birth: ");
            DateOfBirth = Console.ReadLine();
            if (IsValidDateOfBirth(DateOfBirth))
            {
                invalidAttempts = 0;
                break;
            }
            else
            {
                invalidAttempts++;
            }
        }
        if (invalidAttempts >= maxAttempts)
        {
            Console.WriteLine("You have entered incorrectly more than 7 times.");
            invalidAttempts = 0;
            return;
        }

        while(invalidAttempts < maxAttempts)
        {
            Console.Write("Enter your email: ");
            Email = Console.ReadLine();
            if (IsValidEmail(Email))
            {
                if (IsEmailExists(Email))
                {
                    Console.WriteLine("Email already exists on the system. Please re-enter.");
                    invalidAttempts++;
                }
                else
                {
                    invalidAttempts = 0;
                    break; // Thoát khỏi vòng lặp
                }
            }
            else
            {
                invalidAttempts++;
            }
        }
        if (invalidAttempts >= maxAttempts)
        {
            Console.WriteLine("You have entered incorrectly more than 7 times.");
            invalidAttempts = 0;
            return;
        }

        while(invalidAttempts < maxAttempts)
        {
            Console.Write("Enter your phone number: ");
            PhoneNumber = Console.ReadLine();
            if (IsValidPhoneNumber(PhoneNumber))
            {
                if (IsPhoneNumberExists(PhoneNumber))
                {
                    Console.WriteLine("Phone number already exists on the system. Please re-enter.");
                    invalidAttempts++;
                }
                else
                {
                    invalidAttempts = 0;
                    break; // Thoát khỏi vòng lặp
                }
            }
            else
            {
                invalidAttempts++;
            }
        }
        if (invalidAttempts >= maxAttempts)
        {
            Console.WriteLine("You have entered incorrectly more than 7 times.");
            invalidAttempts = 0;
            return;
        }

        Console.Write("Enter address: ");
        Address = Console.ReadLine();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string query = "INSERT INTO users (UserName, Password, Name, Gender, DateOfBirth, Email, PhoneNumber, Address) VALUES (@LoginName, @Password, @Name, @Gender, @DateOfBirth, @Email, @PhoneNumber, @Address)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@LoginName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
            command.Parameters.AddWithValue("@Address", Address);

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("Register Successfully.");
            }
        }
    }
    static void ForgotPassword()
    {
        string Password = "";
        string DateOfBirth = "";
        string UserName = "";
        string Email = "";
        string PhoneNumber = "";

        Console.WriteLine("Forgot Pasword:");
        Console.Write("Enter your loggin name: ");
        UserName = Console.ReadLine();
        Console.Write("Enter your email: ");
        Email = Console.ReadLine();
        Console.Write("Enter your phone number: ");
        PhoneNumber = Console.ReadLine();
        Console.Write("Enter date of birth: ");
        DateOfBirth = Console.ReadLine();

        // Kiểm tra thông tin với cơ sở dữ liệu
        if (VerifyUserInfo(UserName, Email, PhoneNumber, DateOfBirth))
        {
            Console.WriteLine("Accurate information. Please enter a new password.");

            while(invalidAttempts < maxAttempts)
            {
                Console.Write("Enter your pasword: ");
                Password = Console.ReadLine();
                if (IsValidPassword(Password))
                {
                    break;
                }
                else
                {
                    invalidAttempts++;
                }
            }
            if (invalidAttempts >= maxAttempts)
            {
                Console.WriteLine("You have entered incorrectly more than 7 times.");
                invalidAttempts = 0; // Đặt lại số lần nhập sai
                return;
            }

            // Cập nhật mật khẩu mới vào cơ sở dữ liệu
            if (UpdatePassword(UserName, Password))
            {
                Console.WriteLine("Password has been reset successfully.");
            }
        }
        else
        {
            Console.WriteLine("Incorrect information. Unable to reset password");
        }
    }
    static void CustomerMenu()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]MENU[/]")
                    .AddChoices(new[]
                    {
                        "Category",
                        "Policies and Customer Support",
                        "Personal Information",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "Category":
                    Menu1();
                    break;
                case "Policies and Customer Support":
                    Menu2();
                    break;
                case "Personal Information":
                    Menu3();
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void Menu1()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]MENU 1[/]")
                    .AddChoices(new[]
                    {
                        "Customer Management",
                        "Product Management",
                        "Category Management",
                        "Warehouse Management",
                        "Coupon Management",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "Customer Management":
                    CustomersManagementMenu();
                    break;
                case "Product Management":
                    ProductsManagementMenu();
                    break;
                case "Category Management":
                    CategoryManagementMenu();
                    break;
                case "Warehouse Management":
                    WarehouseManagementMenu();
                    break;
                case "Coupon Management":
                    CouponsManagementMenu();
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void Menu2()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]MENU 1[/]")
                    .AddChoices(new[]
                    {
                        "Customer Management",
                        "Product Management",
                        "Category Management",
                        "Warehouse Management",
                        "Coupon Management",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "Customer Management":
                    CustomersManagementMenu();
                    break;
                case "Product Management":
                    ProductsManagementMenu();
                    break;
                case "Category Management":
                    CategoryManagementMenu();
                    break;
                case "Warehouse Management":
                    WarehouseManagementMenu();
                    break;
                case "Coupon Management":
                    CouponsManagementMenu();
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void Menu3()
    {
        string currentUser = "currentUserName"; // Lấy từ phiên đăng nhập
        while (true)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]MENU 1[/]")
                    .AddChoices(new[]
                    {
                        "Personal Information",
                        "Edit personal information",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "DisplayCustomerPersonalInfo":
                    DisplayCustomerPersonalInfo(currentUser, connectionString);
                    break;
                case "EditCustomerPersonalInfo":
                    EditCustomerPersonalInfo(currentUser, connectionString);
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    
    
    
    static void AdminMenu()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]MENU[/]")
                    .AddChoices(new[]
                    {
                        "Category",
                        "Policies and Customer Support",
                        "Documents",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "Category":
                    MainMenu1();
                    break;
                case "Policies and Customer Support":
                    MainMenu2();
                    break;
                case "Documents":
                    MainMenu3();
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void MainMenu1()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]MENU 1[/]")
                    .AddChoices(new[]
                    {
                        "Customer Management",
                        "Product Management",
                        "Category Management",
                        "Warehouse Management",
                        "Coupon Management",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "Customer Management":
                    CustomersManagementMenu();
                    break;
                case "Product Management":
                    ProductsManagementMenu();
                    break;
                case "Category Management":
                    CategoryManagementMenu();
                    break;
                case "Warehouse Management":
                    WarehouseManagementMenu();
                    break;
                case "Coupon Management":
                    CouponsManagementMenu();
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void MainMenu2()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]MENU 2[/]")
                    .AddChoices(new[]
                    {
                        "Policy Management",
                        "Customer Support",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "Policy Management":
                    PolicyManagementMenu();
                    break;
                case "Customer Support":
                    CustomerSupportMenu();
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void MainMenu3()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]MENU 3[/]")
                    .AddChoices(new[]
                    {
                        "Invoice Management",
                        "Orders Management",
                        "Export Receipts Management",
                        "Import Receipts Management",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "Invoice Management":
                    InvoiceManagementMenu();
                    break;
                case "Orders Management":
                    OrdersManagementMenu();
                    break;
                case "Export Receipts Management":
                    ExportReceiptsManagementMenu();
                    break;
                case "Import Receipts Management":
                    ImportReceiptsManagementMenu();
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void ExitProgram()
    {
        AnsiConsole.MarkupLine("[yellow]Exiting the program...[/]");
        Environment.Exit(0);
    }
    
    static void CustomersManagementMenu()
    {
        List<User> customerList = new List<User>();
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]=== Customer Management ===[/]")
                    .AddChoices(new[]
                    {
                        "View Customer Information List",
                        "Edit Customer Information",
                        "Search Customer",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "View Customer Information List":
                    DisplayCustomerInfo(customerList);
                    break;
                case "Edit Customer Information":
                    EditCustomerInfo(customerList, connectionString);
                    break;
                case "Search Customer":
                    SearchCustomerByUserName(customerList, connectionString);
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void ProductsManagementMenu()
    {
        List<Product> productList = LoadProductsFromDatabase(connectionString);
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Product Management[/]")
                    .AddChoices(new[]
                    {
                        "View Product List",
                        "Add Product",
                        "Edit Product Information",
                        "Delete Product Information",
                        "Search Product",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "View Product List":
                    DisplayProduct(productList);
                    break;
                case "Add Product":
                    AddProduct(productList);
                    break;
                case "Edit Product Information":
                    EditProductInfo(productList, connectionString);
                    break;
                case "Delete Product Information":
                    DeleteProductInfo(productList, connectionString);
                    break;
                case "Search Product":
                    SearchProductByID(productList, connectionString);
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void CategoryManagementMenu()
    {
        List<CategoryOfProduct> categoryList = new List<CategoryOfProduct>();
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Category Management[/]")
                    .AddChoices(new[]
                    {
                        "View Category List",
                        "Add Category",
                        "Edit Category Information",
                        "Delete Category",
                        "Search Category",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "View Category List":
                    DisplayCategory(categoryList);
                    break;
                case "Add Category":
                    AddCategory(categoryList);
                    break;
                case "Edit Category Information":
                    EditCategoryInfo(categoryList, connectionString);
                    break;
                case "Delete Category":
                    DeleteCategoryInfo(categoryList, connectionString);
                    break;
                case "Search Category":
                    SearchCategoryByID(categoryList, connectionString);
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void CouponsManagementMenu()
    {
        List<Coupon> couponList = new List<Coupon>();
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]=== Coupon Management ===[/]")
                    .AddChoices(new[]
                    {
                        "View Coupon List",
                        "Add Coupon",
                        "Edit Coupon Information",
                        "Delete Coupon",
                        "Search Coupon",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "1. View Coupon List":
                    DisplayCoupons(couponList);
                    break;
                case "2. Add Coupon":
                    AddCoupon(couponList);
                    break;
                case "3. Edit Coupon Information":
                    EditCouponInfo(couponList, connectionString);
                    break;
                case "4. Delete Coupon":
                    DeleteCoupon(couponList, connectionString);
                    break;
                case "5. Search Coupon":
                    SearchCouponByID(couponList, connectionString);
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void WarehouseManagementMenu()
    {
        List<Warehouse> warehouseList = new List<Warehouse>();
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Warehouse Management[/]")
                    .AddChoices(new[]
                    {
                        "View Warehouse List",
                        "Add Warehouse Information",
                        "Edit Warehouse Information",
                        "Delete Warehouse Information",
                        "Search Warehouse Information by ID",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "View Warehouse List":
                    DisplayWarehouse(warehouseList);
                    break;
                case "Add Warehouse Information":
                    AddWarehouseInfo(warehouseList, connectionString);
                    break;
                case "Edit Warehouse Information":
                    EditWarehouseInfo(warehouseList, connectionString);
                    break;
                case "Delete Warehouse Information":
                    DeleteWarehouseInfo(warehouseList, connectionString);
                    break;
                case "Search Warehouse Information by ID":
                    SearchWarehouseByID(warehouseList, connectionString);
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void PolicyManagementMenu()
    {
        List<Policy> policyList = new List<Policy>();
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Policy Management[/]")
                    .AddChoices(new[]
                    {
                        "Display Policies",
                        "Add Policy",
                        "Edit Policy",
                        "Delete Policy",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "Display Policies":
                    DisplayPolicy(policyList);
                    break;
                case "Add Policy":
                    AddPolicy(policyList);
                    break;
                case "Edit Policy":
                    EditPolicy(policyList, connectionString);
                    break;
                case "Delete Policy":
                    DeletePolicy(policyList, connectionString);
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void CustomerSupportMenu()
    {
        List<Issue> issueList = new List<Issue>();
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Customer Support Management[/]")
                    .AddChoices(new[]
                    {
                        "View List of Issues",
                        "Add Issue",
                        "Edit Issue",
                        "Delete Issue",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "View List of Issues":
                    DisplayIssues(issueList);
                    break;
                case "Add Issue":
                    AddIssue(issueList, connectionString);
                    break;
                case "Edit Issue":
                    EditIssue(issueList, connectionString);
                    break;
                case "Delete Issue":
                    DeleteIssue(issueList, connectionString);
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void InvoiceManagementMenu()
    {
        List<Invoice> invoiceList = new List<Invoice>();
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Invoice Management[/]")
                    .AddChoices(new[]
                    {
                        "View List of Invoices",
                        "Add Invoice",
                        "Edit Invoice",
                        "Delete Invoice",
                        "Search Invoice",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "View List of Invoices":
                    DisplayInvoice(invoiceList);
                    break;
                case "Add Invoice":
                    AddInvoice(invoiceList);
                    break;
                case "Edit Invoice":
                    EditInvoice(invoiceList, connectionString);
                    break;
                case "Delete Invoice":
                    DeleteInvoice(invoiceList, connectionString);
                    break;
                case "Search Invoice":
                    SearchInvoice(invoiceList, connectionString);
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void OrdersManagementMenu()
    {
        List<Order> orderList = new List<Order>();
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Order Management[/]")
                    .AddChoices(new[]
                    {
                        "View List of Orders",
                        "Add Order",
                        "Edit Order",
                        "Delete Order",
                        "Search Order",
                        "Apply Coupon to Order",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "View List of Orders":
                    DisplayOrder(orderList);
                    break;
                case "Add Order":
                    AddOrder(orderList, connectionString);
                    break;
                case "Edit Order":
                    EditOrderInfo(orderList, connectionString);
                    break;
                case "Delete Order":
                    DeleteOrderInfo(orderList, connectionString);
                    break;
                case "Search Order":
                    SearchOrderByID(orderList, connectionString);
                    break;
                case "Apply Coupon to Order":
                    Console.Write("Enter Order ID: ");
                    int orderID = int.Parse(Console.ReadLine());
                    Console.Write("Enter Coupon Code: ");
                    string couponCode = Console.ReadLine();
                    ApplyCouponToOrder(orderID, couponCode, connectionString);
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void ExportReceiptsManagementMenu()
    {
        List<ExportReceipt> exportReceiptList = new List<ExportReceipt>();
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Export Receipt Management[/]")
                    .AddChoices(new[]
                    {
                        "View List of Export Receipts",
                        "Add Export Receipt",
                        "Edit Export Receipt",
                        "Delete Export Receipt",
                        "Search Export Receipt",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "View List of Export Receipts":
                    DisplayExportReceipt(exportReceiptList);
                    break;
                case "Add Export Receipt":
                    AddExportReceipt(exportReceiptList);
                    break;
                case "Edit Export Receipt":
                    EditExportReceiptInfo(exportReceiptList, connectionString);
                    break;
                case "Delete Export Receipt":
                    DeleteExportReceiptInfo(exportReceiptList, connectionString);
                    break;
                case "Search Export Receipt":
                    SearchExportReceiptByID(exportReceiptList, connectionString);
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }
    static void ImportReceiptsManagementMenu()
    {
        List<ImportReceipt> importReceiptList = new List<ImportReceipt>();
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Import Receipt Management[/]")
                    .AddChoices(new[]
                    {
                        "View List of Import Receipts",
                        "Add Import Receipt",
                        "Edit Import Receipt",
                        "Delete Import Receipt",
                        "Search Import Receipt",
                        "Return",
                        "",
                        "Exit"
                    }));

            switch (choice)
            {
                case "View List of Import Receipts":
                    DisplayImportReceipt(importReceiptList);
                    break;
                case "Add Import Receipt":
                    AddImportReceipt(importReceiptList);
                    break;
                case "Edit Import Receipt":
                    EditImportReceiptInfo(importReceiptList, connectionString);
                    break;
                case "Delete Import Receipt":
                    DeleteImportReceiptInfo(importReceiptList, connectionString);
                    break;
                case "Search Import Receipt":
                    SearchImportReceiptByID(importReceiptList, connectionString);
                    break;
                case "Return":
                    return;
                case "Exit":
                    ExitProgram();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option. Please choose again.[/]");
                    AnsiConsole.Prompt(new TextPrompt<string>("Press any key to continue..."));
                    break;
            }
        }
    }

    static void DisplayCustomerPersonalInfo(string currentUser, string connectionString)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM users WHERE UserName = @CurrentUser";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CurrentUser", currentUser);

            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            var table = new Table();
            table.AddColumn("UserName");
            table.AddColumn("Name");
            table.AddColumn("Gender");
            table.AddColumn("Date of Birth");
            table.AddColumn("Email");
            table.AddColumn("Phone Number");
            table.AddColumn("Address");

            while (reader.Read())
            {
                table.AddRow(
                    reader["UserName"].ToString(),
                    reader["Name"].ToString(),
                    reader["Gender"].ToString(),
                    reader["DateOfBirth"].ToString(),
                    reader["Email"].ToString(),
                    reader["PhoneNumber"].ToString(),
                    reader["Address"].ToString()
                );
            }
            reader.Close();
            AnsiConsole.Write(table);
        }
    }
    static void EditCustomerPersonalInfo(string currentUser, string connectionString)
    {
        List<User> customerList = new List<User>();
        PopulateCustomerList(customerList, connectionString);

        User customer = customerList.FirstOrDefault();
        if (customer != null)
        {
            Console.WriteLine("Enter new information: ");
            Console.Write("Enter new name: ");
            string newName = Console.ReadLine();
            customer.Name = newName;

            // (Tiếp tục cập nhật thông tin như trong code ban đầu)

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE users SET Name = @NewName, Gender = @Gender, DateOfBirth = @DateOfBirth, Email = @Email, PhoneNumber = @PhoneNumber, Address = @Address WHERE UserName = @CurrentUser";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NewName", customer.Name);
                    command.Parameters.AddWithValue("@Gender", customer.Gender);
                    command.Parameters.AddWithValue("@DateOfBirth", customer.DateOfBirth);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                    command.Parameters.AddWithValue("@Address", customer.Address);
                    command.Parameters.AddWithValue("@CurrentUser", currentUser);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Customer information updated successfully!");
                    }
                    else
                    {
                        Console.WriteLine("No rows were updated. Please check the customer name.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("No customer found with this username.");
        }
    }

    static void DisplayCustomerInfo(List<User> customerList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM users";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn("UserName");
            table.AddColumn("Name");
            table.AddColumn("Gender");
            table.AddColumn("Date of Birth");
            table.AddColumn("Email");
            table.AddColumn("Phone Number");
            table.AddColumn("Address");

            // Add some rows
            while (reader.Read())
            {
                table.AddRow(
                    reader["UserName"].ToString(),
                    reader["Name"].ToString(),
                    reader["Gender"].ToString(),
                    reader["DateOfBirth"].ToString(),
                    reader["Email"].ToString(),
                    reader["PhoneNumber"].ToString(),
                    reader["Address"].ToString()
                );
            }
            reader.Close();

            // Render the table to the console
            AnsiConsole.Write(table);
        }
    }
    static void EditCustomerInfo(List<User> customerList, string connectionString)
    {
        // Populate customer list from database
        PopulateCustomerList(customerList, connectionString);

        Console.Write("Enter the customer name to edit: ");
        string customerName = Console.ReadLine();

        // Find the customer in the list
        User customer = customerList.Find(c => c.Name == customerName);

        if (customer != null)
        {
            Console.WriteLine("Enter new information: ");
            Console.Write("Enter new name: ");
            string newName = Console.ReadLine();
            customer.Name = newName; // Update customer's name
            
            Console.Write("Enter gender: ");
            customer.Gender = Console.ReadLine();
            
            Console.Write("Enter date of birth (yyyy-MM-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime dateOfBirth))
            {
                customer.DateOfBirth = dateOfBirth;
            }
            else
            {
                Console.WriteLine("Invalid date format. Update cancelled.");
                return;
            }
            
            Console.Write("Enter email address: ");
            customer.Email = Console.ReadLine();
            
            Console.Write("Enter phone number: ");
            customer.PhoneNumber = Console.ReadLine();
            
            Console.Write("Enter address: ");
            customer.Address = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE users SET Name = @NewName, Gender = @Gender, DateOfBirth = @DateOfBirth, Email = @Email, PhoneNumber = @PhoneNumber, Address = @Address WHERE Name = @CustomerName";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NewName", customer.Name); // New name parameter
                    command.Parameters.AddWithValue("@Gender", customer.Gender);
                    command.Parameters.AddWithValue("@DateOfBirth", customer.DateOfBirth);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                    command.Parameters.AddWithValue("@Address", customer.Address);
                    command.Parameters.AddWithValue("@CustomerName", customerName); // Customer's current name as condition

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Customer information updated successfully!");

                        // Update local customer list
                        PopulateCustomerList(customerList, connectionString);
                    }
                    else
                    {
                        Console.WriteLine("No rows were updated. Please check the customer name.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine($"Customer with name '{customerName}' not found in the list.");
        }
    }
    static void PopulateCustomerList(List<User> customerList, string connectionString)
    {
        customerList.Clear(); // Clear existing list before populating
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM users";
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User customer = new User();
                        customer.Name = reader.GetString("Name");
                        customer.Gender = reader.GetString("Gender");
                        customer.DateOfBirth = reader.GetDateTime("DateOfBirth");
                        customer.Email = reader.GetString("Email");
                        customer.PhoneNumber = reader.GetString("PhoneNumber");
                        customer.Address = reader.GetString("Address");

                        customerList.Add(customer);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while populating customer list: " + ex.Message);
        }
    }
    static void SearchCustomerByUserName(List<User> customerList, string connectionString)
    {
        string userName = AnsiConsole.Ask<string>("Enter [green]customer username[/] to find:");

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM users WHERE UserName = @UserName";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", userName);

            connection.Open();
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var table = new Table();
                    table.AddColumn("Full Name");
                    table.AddColumn("Gender");
                    table.AddColumn("Date of Birth");
                    table.AddColumn("Phone");
                    table.AddColumn("Email");
                    table.AddColumn("Address");

                    table.AddRow(
                        reader["Name"].ToString(),
                        reader["Gender"].ToString(),
                        reader["DateOfBirth"].ToString(),
                        reader["PhoneNumber"].ToString(),
                        reader["Email"].ToString(),
                        reader["Address"].ToString()
                    );

                    AnsiConsole.Write(table);
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]No customer found with this username.[/]");
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

            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn("Product ID");
            table.AddColumn("Category ID");
            table.AddColumn("Product Name");
            table.AddColumn("Description");
            table.AddColumn("Price");

            // Add some rows
            while (reader.Read())
            {
                table.AddRow(
                    reader["ProductID"].ToString(),
                    reader["CategoryID"].ToString(),
                    reader["ProductName"].ToString(),
                    reader["Description"].ToString(),
                    reader["Price"].ToString()
                );
            }
            reader.Close();

            // Render the table to the console
            AnsiConsole.Write(table);
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
            string query = "INSERT INTO product (ProductID, CategoryID, ProductName, Description, Price) VALUES (@ProductID, @CategoryID, @ProductName, @Description, @Price)";
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
    static List<Product> LoadProductsFromDatabase(string connectionString)
    {
        List<Product> products = new List<Product>();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ProductID, CategoryID, ProductName, Description, Price FROM product";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ProductID = reader.GetInt32("ProductID"),
                        CategoryID = reader.GetInt32("CategoryID"),
                        ProductName = reader.GetString("ProductName"),
                        Description = reader.GetString("Description"),
                        Price = reader.GetInt32("Price")
                    };
                    products.Add(product);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while loading products: " + ex.Message);
        }

        return products;
    }
    static void EditProductInfo(List<Product> productList, string connectionString)
    {
        Console.WriteLine("Enter product ID to edit: ");
        if (!int.TryParse(Console.ReadLine(), out int productID))
        {
            Console.WriteLine("Invalid product ID.");
            return;
        }

        Product product = productList.Find(p => p.ProductID == productID);

        if (product != null)
        {
            Console.WriteLine("Enter new information: ");
            Console.Write("Enter category ID: ");
            if (!int.TryParse(Console.ReadLine(), out int categoryID))
            {
                Console.WriteLine("Invalid category ID.");
                return;
            }
            Console.Write("Enter product name: ");
            string productName = Console.ReadLine();
            Console.Write("Enter description: ");
            string description = Console.ReadLine();
            Console.Write("Enter price: ");
            if (!int.TryParse(Console.ReadLine(), out int price))
            {
                Console.WriteLine("Invalid price.");
                return;
            }

            // Display the entered information for confirmation
            Console.WriteLine("\nYou have entered the following information:");
            Console.WriteLine($"Category ID: {categoryID}");
            Console.WriteLine($"Product Name: {productName}");
            Console.WriteLine($"Description: {description}");
            Console.WriteLine($"Price: {price}");

            // Ask for confirmation
            Console.Write("\nDo you want to update the product information? (yes/no): ");
            string confirmation = Console.ReadLine().Trim().ToLower();

            if (confirmation == "yes" || confirmation =="Yes" )
            {
                product.CategoryID = categoryID;
                product.ProductName = productName;
                product.Description = description;
                product.Price = price;

                try
                {
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
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Product information updated successfully!");
                        }
                        else
                        {
                            Console.WriteLine("No rows were updated. Please check the Product ID.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Update canceled.");
            }
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
        var criteria = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Choose search criteria:[/]")
                .AddChoices(new[]
                {
                    "Search by product ID",
                    "Search by product name",
                    "Search by category ID",
                    "Search by price range"
                }));

        string query = "";
        MySqlCommand command = new MySqlCommand();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            switch (criteria)
            {
                case "Search by product ID":
                    int productID = AnsiConsole.Ask<int>("Enter product ID:");
                    query = "SELECT * FROM product WHERE ProductID = @ProductID";
                    command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductID", productID);
                    break;
                case "Search by product name":
                    string productName = AnsiConsole.Ask<string>("Enter product name:");
                    query = "SELECT * FROM product WHERE ProductName LIKE @ProductName";
                    command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductName", "%" + productName + "%");
                    break;
                case "Search by category ID":
                    int categoryID = AnsiConsole.Ask<int>("Enter category ID:");
                    query = "SELECT * FROM product WHERE CategoryID = @CategoryID";
                    command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CategoryID", categoryID);
                    break;
                case "Search by price range":
                    int minPrice = AnsiConsole.Ask<int>("Enter minimum price:");
                    int maxPrice = AnsiConsole.Ask<int>("Enter maximum price:");
                    query = "SELECT * FROM product WHERE Price BETWEEN @MinPrice AND @MaxPrice";
                    command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MinPrice", minPrice);
                    command.Parameters.AddWithValue("@MaxPrice", maxPrice);
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid criteria.[/]");
                    return;
            }

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                var table = new Table().Expand();
                table.AddColumn("Product ID");
                table.AddColumn("Category ID");
                table.AddColumn("Product Name");
                table.AddColumn("Description");
                table.AddColumn("Price");

                while (reader.Read())
                {
                    table.AddRow(
                        reader["ProductID"].ToString(),
                        reader["CategoryID"].ToString(),
                        reader["ProductName"].ToString(),
                        reader["Description"].ToString(),
                        reader["Price"].ToString()

                    );
                }

                AnsiConsole.Write(table);
            }
        }
    }
    
    static List<CategoryOfProduct> LoadCategories(string connectionString)
    {
        List<CategoryOfProduct> categories = new List<CategoryOfProduct>();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM category";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    CategoryOfProduct category = new CategoryOfProduct
                    {
                        CategoryID = int.Parse(reader["CategoryID"].ToString()),
                        CategoryName = reader["CategoryName"].ToString()
                    };
                    categories.Add(category);
                }
                reader.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while loading categories: " + ex.Message);
        }

        return categories;
    }
    static void DisplayCategory(List<CategoryOfProduct> categoryList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM category";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn("Category ID");
            table.AddColumn("Category Name");

            // Add some rows
            while (reader.Read())
            {
                table.AddRow(
                    reader["CategoryID"].ToString(),
                    reader["CategoryName"].ToString()
                );
            }
            reader.Close();

            // Render the table to the console
            AnsiConsole.Write(table);
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
        // Fetch categories from the database if the list is empty
        if (categoryList == null || categoryList.Count == 0)
        {
            categoryList = new List<CategoryOfProduct>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT CategoryID, CategoryName FROM category";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categoryList.Add(new CategoryOfProduct
                            {
                                CategoryID = reader.GetInt32("CategoryID"),
                                CategoryName = reader.GetString("CategoryName")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching the categories: " + ex.Message);
                return;
            }
        }

        Console.Write("Enter category ID to edit: ");
        if (int.TryParse(Console.ReadLine(), out int categoryID))
        {
            // Find the category with the specified ID
            CategoryOfProduct category = categoryList.Find(c => c.CategoryID == categoryID);

            if (category != null)
            {
                Console.Write("Enter new category name: ");
                string newCategoryName = Console.ReadLine();

                if (!string.IsNullOrEmpty(newCategoryName))
                {
                    category.CategoryName = newCategoryName;

                    try
                    {
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
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred while updating the category: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("New category name cannot be empty.");
                }
            }
            else
            {
                Console.WriteLine($"Category with ID {categoryID} not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid category ID. Please enter a valid number.");
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
        int categoryID = AnsiConsole.Ask<int>("Enter [green]category ID[/] to find:");

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
                    var table = new Table();
                    table.AddColumn("Category ID");
                    table.AddColumn("Category Name");

                    table.AddRow(
                        reader["CategoryID"].ToString(),
                        reader["CategoryName"].ToString()
                    );

                    AnsiConsole.Write(table);
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]No category found with this ID.[/]");
                }
            }
        }
    }

    static List<Warehouse> LoadWarehouses(string connectionString)
    {
        List<Warehouse> warehouses = new List<Warehouse>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM warehouse";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Warehouse warehouse = new Warehouse
                {
                    WareHouseID = int.Parse(reader["WarehouseID"].ToString()),
                    ProductID = int.Parse(reader["ProductID"].ToString()),
                    Quantity = int.Parse(reader["quantity"].ToString())
                };
                warehouses.Add(warehouse);
            }
            reader.Close();
        }
        return warehouses;
    } 
    static void DisplayWarehouse(List<Warehouse> warehouseList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM warehouse";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn("Warehouse ID");
            table.AddColumn("Product ID");
            table.AddColumn("Quantity");

            // Add some rows
            while (reader.Read())
            {
                table.AddRow(
                    reader["WarehouseID"].ToString(),
                    reader["ProductID"].ToString(),
                    reader["quantity"].ToString()
                );
            }
            reader.Close();

            // Render the table to the console
            AnsiConsole.Write(table);
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
            warehouseList.Add(wh);
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
        int warehouseID = AnsiConsole.Ask<int>("Enter [green]warehouse ID[/] to find:");

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
                    var table = new Table();
                    table.AddColumn("Warehouse ID");
                    table.AddColumn("Product ID");
                    table.AddColumn("Quantity");

                    table.AddRow(
                        reader["WarehouseID"].ToString(),
                        reader["ProductID"].ToString(),
                        reader["Quantity"].ToString()
                    );

                    AnsiConsole.Write(table);
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]No warehouse found with this ID.[/]");
                }
            }
        }
    }
    
    static List<Coupon> LoadCoupons(string connectionString)
    {
        List<Coupon> coupons = new List<Coupon>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM coupon";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Coupon coupon = new Coupon
                {
                    CouponID = int.Parse(reader["id_coupon"].ToString()),
                    CouponCode = reader["coupon_code"].ToString(),
                    DiscountAmount = decimal.Parse(reader["discount_amount"].ToString()),
                    IsActive = bool.Parse(reader["is_active"].ToString())
                };
                coupons.Add(coupon);
            }
            reader.Close();
        }
        return coupons;
    }
    static void DisplayCoupons(List<Coupon> couponList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM coupon";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn("Coupon ID");
            table.AddColumn("Coupon Code");
            table.AddColumn("Discount Amount");
            table.AddColumn("Is Active");

            // Add some rows
            while (reader.Read())
            {
                table.AddRow(
                    reader["id_coupon"].ToString(),
                    reader["coupon_code"].ToString(),
                    reader["discount_amount"].ToString(),
                    reader["is_active"].ToString()
                );
            }
            reader.Close();

            // Render the table to the console
            AnsiConsole.Write(table);
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
            couponList.Add(coupon);
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
        int couponID = AnsiConsole.Ask<int>("Enter [green]coupon ID[/] to search:");

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
                    var table = new Table();
                    table.AddColumn("Coupon ID");
                    table.AddColumn("Coupon Code");
                    table.AddColumn("Discount Amount");
                    table.AddColumn("Is Active");

                    table.AddRow(
                        reader["id_coupon"].ToString(),
                        reader["coupon_code"].ToString(),
                        reader["discount_amount"].ToString(),
                        reader["is_active"].ToString()
                    );

                    AnsiConsole.Write(table);
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]No coupon found with this ID.[/]");
                }
            }
        }
    }
    
    static List<Policy> LoadPoliciesFromDatabase(string connectionString)
    {
        List<Policy> policies = new List<Policy>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM policy";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Policy policy = new Policy
                {
                    ID = reader.GetInt32("ID"),
                    Title = reader.GetString("Title"),
                    Content = reader.GetString("Content")
                };
                policies.Add(policy);
            }
            reader.Close();
        }

        return policies;
    }    
    static void DisplayPolicy(List<Policy> policyList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM policy";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn("Policy ID");
            table.AddColumn("Policy Title");
            table.AddColumn("Content");

            // Add some rows
            while (reader.Read())
            {
                table.AddRow(
                    reader["ID"].ToString(),
                    reader["Title"].ToString(),
                    reader["Content"].ToString()
                );
            }
            reader.Close();

            // Render the table to the console
            AnsiConsole.Write(table);
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
            policyList.Add(policy);
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
    
    static List<Issue> LoadIssuesFromDatabase(string connectionString)
    {
        List<Issue> issues = new List<Issue>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM customer_support";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Issue issue = new Issue
                {
                    ID = reader.GetInt32("ID"),
                    IssueTitle = reader.GetString("IssueTitle"),
                    IssueContent = reader.GetString("IssueContent"),
                    CreatedAt = reader.GetDateTime("CreatedAt")
                };
                issues.Add(issue);
            }
            reader.Close();
        }

        return issues;
    }
    static void DisplayIssues(List<Issue> issueList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM customer_support";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn("Issue ID");
            table.AddColumn("Issue Title");
            table.AddColumn("Issue Description");
            table.AddColumn("Issue Date");

            // Add some rows
            while (reader.Read())
            {
                table.AddRow(
                    reader["ID"].ToString(),
                    reader["IssueTitle"].ToString(),
                    reader["IssueContent"].ToString(),
                    reader["CreatedAt"].ToString()
                );
            }
            reader.Close();

            // Render the table to the console
            AnsiConsole.Write(table);
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
            issueList.Add(issue);
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
    
    static List<Invoice> LoadInvoicesFromDatabase(string connectionString)
    {
        List<Invoice> invoices = new List<Invoice>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM invoice";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Invoice invoice = new Invoice
                {
                    InvoiceID = reader.GetInt32("InvoiceID"),
                    ProductID = reader.GetInt32("ProductID"),
                    CustomerID = reader.GetInt32("CustomerID"),
                    InvoiceDate = reader.GetDateTime("InvoiceDate"),
                    TotalAmount = reader.GetInt32("TotalAmount")
                };
                invoices.Add(invoice);
            }
            reader.Close();
        }

        return invoices;
    }
    static void DisplayInvoice(List<Invoice> invoiceList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM invoice";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn("Invoice ID");
            table.AddColumn("Product ID");
            table.AddColumn("Customer ID");
            table.AddColumn("Invoice Date");
            table.AddColumn("Total Amount");

            // Add some rows
            while (reader.Read())
            {
                table.AddRow(
                    reader["InvoiceID"].ToString(),
                    reader["ProductID"].ToString(),
                    reader["CustomerID"].ToString(),
                    reader["InvoiceDate"].ToString(),
                    reader["TotalAmount"].ToString()
                );
            }
            reader.Close();

            // Render the table to the console
            AnsiConsole.Write(table);
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
    static void SearchInvoice(List<Invoice> invoiceList, string connectionString)
    {
        int invoiceID = AnsiConsole.Ask<int>("Enter [green]invoice ID[/] to search:");

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
                    var table = new Table();
                    table.AddColumn("Invoice ID");
                    table.AddColumn("Product ID");
                    table.AddColumn("Customer ID");
                    table.AddColumn("Invoice Date");
                    table.AddColumn("Total Amount");

                    table.AddRow(
                        reader["InvoiceID"].ToString(),
                        reader["ProductID"].ToString(),
                        reader["CustomerID"].ToString(),
                        reader["InvoiceDate"].ToString(),
                        reader["TotalAmount"].ToString()
                    );

                    AnsiConsole.Write(table);
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]No invoice found with this ID.[/]");
                }
            }
        }
    }

    static List<Order> LoadOrdersFromDatabase(string connectionString)
    {
        List<Order> orders = new List<Order>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM `order`";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Order order = new Order
                {
                    OrderID = reader.GetInt32("OrderID"),
                    UserName = reader.GetString("UserName"),
                    ProductID = reader.GetInt32("ProductID"),
                    CouponCode = reader.IsDBNull(reader.GetOrdinal("CouponCode")) ? null : reader.GetString("CouponCode"),
                    Quantity = reader.GetInt32("Quantity"),
                    OrderDate = reader.GetDateTime("OrderDate"),
                    ShippingAddress = reader.GetString("ShippingAddress"),
                    TotalAmount = reader.GetInt32("TotalAmount")
                };
                orders.Add(order);
            }
            reader.Close();
        }

        return orders;
    }
    static void DisplayOrder(List<Order> orderList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM `order`";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn("Order ID");
            table.AddColumn("User Name");
            table.AddColumn("Product ID");
            table.AddColumn("Coupon Code");
            table.AddColumn("Quantity");
            table.AddColumn("Order Date");
            table.AddColumn("Shipping Address");
            table.AddColumn("Total Amount");

            // Add some rows
            while (reader.Read())
            {
                table.AddRow(
                    reader["OrderID"].ToString(),
                    reader["UserName"].ToString(),
                    reader["ProductID"].ToString(),
                    reader["CouponCode"].ToString(),
                    reader["Quantity"].ToString(),
                    reader["OrderDate"].ToString(),
                    reader["ShippingAddress"].ToString(),
                    reader["TotalAmount"].ToString()
                );
            }
            reader.Close();

            // Render the table to the console
            AnsiConsole.Write(table);
        }
    }
    static void AddOrder(List<Order> orderList, string connectionString)
    {
        Order order = new Order();
        Console.Write("Enter order ID: ");
        order.OrderID = int.Parse(Console.ReadLine());
        Console.Write("Enter User Name: ");
        order.UserName = Console.ReadLine();
        Console.Write("Enter product ID: ");
        order.ProductID = int.Parse(Console.ReadLine());
        Console.Write("Enter Coupon: ");
        order.CouponCode = Console.ReadLine();
        Console.Write("Enter quantity ID: ");
        order.Quantity = int.Parse(Console.ReadLine());
        Console.Write("Enter order date (yyyy-MM-dd): ");
        order.OrderDate = DateTime.Parse(Console.ReadLine());
        Console.Write("Enter shipping address: ");
        order.ShippingAddress = Console.ReadLine();
        Console.Write("Enter total amount: ");
        order.TotalAmount = int.Parse(Console.ReadLine());

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO `order` (OrderID, UserName, ProductID, CouponCode,Quantity, OrderDate, ShippingAddress, TotalAmount) VALUES (@OrderID, @UserName, @ProductID, @CouponCode,@Quantity, @OrderDate, @ShippingAddress, @TotalAmount)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@OrderID", order.OrderID);
            command.Parameters.AddWithValue("@UserName", order.UserName);
            command.Parameters.AddWithValue("@ProductID", order.ProductID);
            command.Parameters.AddWithValue("@CouponCode", order.CouponCode);
            command.Parameters.AddWithValue("@Quantity", order.Quantity);
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
            order.UserName = Console.ReadLine();
            Console.Write("Enter product ID: ");
            order.ProductID = int.Parse(Console.ReadLine());
            Console.Write("Enter sale ID: ");
            order.CouponCode = Console.ReadLine();
            Console.Write("Enter quantity ID: ");
            order.Quantity = int.Parse(Console.ReadLine());
            Console.Write("Enter order date (yyyy-MM-dd): ");
            order.OrderDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter delivery address: ");
            order.ShippingAddress = Console.ReadLine();
            Console.Write("Enter total amount: ");
            order.TotalAmount = int.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE `order` SET UserName = @UserName, ProductID = @ProductID, Coupon Code = @CouponCode,Quantity = @Quantity, OrderDate = @OrderDate, ShippingAddress = @ShippingAddress, TotalAmount = @TotalAmount WHERE OrderID = @OrderID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrderID", order.OrderID);
                command.Parameters.AddWithValue("@UserName", order.UserName);
                command.Parameters.AddWithValue("@ProductID", order.ProductID);
                command.Parameters.AddWithValue("@SaleID", order.CouponCode);
                command.Parameters.AddWithValue("@Quantity", order.Quantity);
                command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                command.Parameters.AddWithValue("@ShippingAddress", order.ShippingAddress);
                command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                command.ExecuteNonQuery();
            }
            orderList.Add(order);
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
    int orderID = AnsiConsole.Ask<int>("Enter [green]order ID[/] to search:");

    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        string query = "SELECT * FROM `order` WHERE OrderID = @Order";
        MySqlCommand command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Order", orderID);

        connection.Open();
        using (MySqlDataReader reader = command.ExecuteReader())
        {
            if (reader.Read())
            {
                var table = new Table();
                table.AddColumn("Order ID");
                table.AddColumn("UserName");
                table.AddColumn("Product ID");
                table.AddColumn("Coupon Code");
                table.AddColumn("Quantity");
                table.AddColumn("Order Date");
                table.AddColumn("Shipping Address");
                table.AddColumn("Total Amount");

                table.AddRow(
                    reader["OrderID"].ToString(),
                    reader["UserName"].ToString(),
                    reader["ProductID"].ToString(),
                    reader["CouponCode"].ToString(),
                    reader["Quantity"].ToString(),
                    reader.GetDateTime("OrderDate").ToString("yyyy-MM-dd"),
                    reader["ShippingAddress"].ToString(),
                    reader["TotalAmount"].ToString()
                );

                AnsiConsole.Write(table);
            }
            else
            {
                AnsiConsole.MarkupLine("[red]No order found with this ID.[/]");
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
            string query = "SELECT TotalAmount FROM `order` WHERE OrderID = @OrderID";
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
                    query = "UPDATE `order` SET TotalAmount = @NewTotal WHERE OrderID = @OrderID";
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
    
    static List<ExportReceipt> LoadExportReceiptsFromDatabase(string connectionString)
    {
        List<ExportReceipt> exportReceipts = new List<ExportReceipt>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM export_receipt";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ExportReceipt exportReceipt = new ExportReceipt
                {
                    ExportReceiptID = reader.GetInt32("ExportReceiptID"),
                    ProductID = reader.GetInt32("ProductID"),
                    ExportDate = reader.GetDateTime("ExportReceiptDate"),
                    ShippingAddress = reader.GetString("ShippingAddress"),
                    TotalAmount = reader.GetInt32("TotalAmount")
                };
                exportReceipts.Add(exportReceipt);
            }
            reader.Close();
        }

        return exportReceipts;
    }
    static void DisplayExportReceipt(List<ExportReceipt> exportReceiptList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM export_receipt";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn("ExportReceipt ID");
            table.AddColumn("Product ID");
            table.AddColumn("Export Date");
            table.AddColumn("Shipping Address");
            table.AddColumn("Total Amount");

            // Add some rows
            while (reader.Read())
            {
                table.AddRow(
                    reader["ExportReceiptID"].ToString(),
                    reader["ProductID"].ToString(),
                    reader["ExportReceiptDate"].ToString(),
                    reader["ShippingAddress"].ToString(),
                    reader["TotalAmount"].ToString()
                );
            }
            reader.Close();

            // Render the table to the console
            AnsiConsole.Write(table);
        }
    }
    static void AddExportReceipt(List<ExportReceipt> exportReceiptList)
    {
        ExportReceipt er = new ExportReceipt();
        Console.Write("Enter ExportReceipt ID: ");
        er.ExportReceiptID = int.Parse(Console.ReadLine());
        Console.Write("Enter product ID: ");
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
            string query = "INSERT INTO export_receipt (ExportReceiptID,ProductID, ExportReceiptDate, ShippingAddress, TotalAmount) VALUES (@ExportReceiptID, @ProductID, @ExportReceiptDate, @ShippingAddress, @TotalAmount)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ExportReceiptID", er.ExportReceiptID);
            command.Parameters.AddWithValue("@ProductID", er.ProductID);
            command.Parameters.AddWithValue("@ExportReceiptDate", er.ExportDate);
            command.Parameters.AddWithValue("@ShippingAddress", er.ShippingAddress);
            command.Parameters.AddWithValue("@TotalAmount", er.TotalAmount);
            command.ExecuteNonQuery();
        }
        exportReceiptList.Add(er);
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
            exportReceiptList.Add(er);
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
    static void SearchExportReceiptByID(List<ExportReceipt> exportReceiptList, string connectionString)
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
                    var table = new Table();
                    table.AddColumn("Field");
                    table.AddColumn("Value");

                    table.AddRow("ExportReceipt ID", reader["ExportReceiptID"].ToString());
                    table.AddRow("Product ID", reader["ProductID"].ToString());
                    table.AddRow("Export Date", reader["ExportReceiptDate"].ToString());
                    table.AddRow("Delivery Address", reader["ShippingAddress"].ToString());
                    table.AddRow("Total Amount", reader["TotalAmount"].ToString());

                    AnsiConsole.Write(table);
                }
                else
                {
                    AnsiConsole.Markup("[red]No export receipt found with this ID.[/]");
                }
            }
        }
    }
    
    static List<ImportReceipt> LoadImportReceiptsFromDatabase(string connectionString)
    {
        List<ImportReceipt> importReceipts = new List<ImportReceipt>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM import_receipt";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ImportReceipt importReceipt = new ImportReceipt
                {
                    ImportReceiptID = reader.GetInt32("ImportReceiptID"),
                    ProductID = reader.GetInt32("ProductID"),
                    ImportDate = reader.GetDateTime("ImportReceiptDate"),
                    TotalAmount = reader.GetInt32("TotalAmount")
                };
                importReceipts.Add(importReceipt);
            }
            reader.Close();
        }

        return importReceipts;
    }
    static void DisplayImportReceipt(List<ImportReceipt> importReceiptList)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM import_receipt";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn("ImportReceipt ID");
            table.AddColumn("Product ID");
            table.AddColumn("Import Date");
            table.AddColumn("Total Amount");

            // Add some rows
            while (reader.Read())
            {
                table.AddRow(
                    reader["ImportReceiptID"].ToString(),
                    reader["ProductID"].ToString(),
                    reader["ImportReceiptDate"].ToString(),
                    reader["TotalAmount"].ToString()
                );
            }
            reader.Close();

            // Render the table to the console
            AnsiConsole.Write(table);
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
            string query = "INSERT INTO import_receipt (ImportReceiptID,ProductID, ImportReceiptDate,TotalAmount) VALUES (@ImportReceiptID, @ProductID, @ImportReceiptDate,@TotalAmount)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ImportReceiptID", ir.ImportReceiptID);
            command.Parameters.AddWithValue("@ProductID", ir.ProductID);
            command.Parameters.AddWithValue("@ImportReceiptDate", ir.ImportDate);
            command.Parameters.AddWithValue("@TotalAmount", ir.TotalAmount);
            command.ExecuteNonQuery();
        }
        importReceiptList.Add(ir);
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

            }
            Console.WriteLine("Import receipt information updated successfully!");
            importReceiptList.Add(ir);
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