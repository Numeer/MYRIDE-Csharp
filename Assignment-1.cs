using System;
using System.Collections.Generic;
using System.Linq;
class Program
{

    static void Main(string[] args)
    {
        Admin admin = new Admin();
        Driver driver = new Driver();


        while (true)
        {
            Console.WriteLine("Main Menu:");
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine("WELCOME TO MYRIDE");
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine("1. Book a Ride");
            Console.WriteLine("2. Enter as Driver");
            Console.WriteLine("3. Enter as Admin");
            Console.Write("Press 1 to 3 to select an option:");
            Console.ForegroundColor = ConsoleColor.Green;
            string choice = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;

            switch (choice)
            {
                case "1":
                    Passenger passenger = new Passenger();
                    passenger.BookRide(admin,driver);
                    break;
                case "2":

                    Console.Write("Enter ID: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    int driverId = int.Parse(Console.ReadLine());
                    Console.ForegroundColor = ConsoleColor.Gray;

                    Console.Write("Enter Name: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string driverName = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    driver.EnterAsDriver(driverId, driverName, admin);
                    break;
                case "3":
                    admin.DisplayAdminMenu();
                    break;
                default:
                    Console.WriteLine("Invalid choice!! Please try again:)");
                    break;
            }
        }
    }
}

class Vehicle
{
    public string Type { get; set; }
    public string Model { get; set; }
    public string LicensePlate { get; set; }
}

class Location
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public void SetLocation(string input)
    {
        string[] coordinates = input.Split(',');
        if (coordinates.Length == 2 && double.TryParse(coordinates[0], out double lat) && double.TryParse(coordinates[1], out double lon))
        {
            Latitude = lat;
            Longitude = lon;
        }
        else
        {
            Console.WriteLine("Invalid coordinates format!! Location not set:)");
        }
    }
}

class Driver
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
    public string PhoneNo { get; set; }
    public Location CurrLocation { get; set; }
    public Vehicle Vehicle { get; set; }
    public List<int> Rating { get; set; }
    public bool Availability { get; set; }

    public Driver()
    {
        Rating = new List<int>();
        Availability = true;
    }

    public void EnterAsDriver(int driverId, string name, Admin admin)
    {
        Driver registeredDriver = CheckDriverRegistration(driverId, name, admin);

        if (registeredDriver == null)
        {
            Console.WriteLine("Driver not registered!! Please go back to the main menu:)");
        }
        else
        {
            DisplayDriverMenu(registeredDriver, admin);
        }
    }

    private void DisplayDriverMenu(Driver driver, Admin admin)
    {
        while (true)
        {
            Console.WriteLine("\nDriver Menu:");
            Console.WriteLine("1. Change availability");
            Console.WriteLine("2. Change Location");
            Console.WriteLine("3. Exit as Driver");
            Console.Write("Select an option (1-3): ");
            Console.ForegroundColor = ConsoleColor.Green;
            int choice = int.Parse(Console.ReadLine());
            Console.ForegroundColor = ConsoleColor.Gray;

            switch (choice)
            {
                case 1:
                    UpdateAvailability(driver);
                    break;
                case 2:
                    UpdateLocation(driver);
                    break;
                case 3:
                    return;
                default:
                    Console.WriteLine("Invalid choice!! Please select a valid option:)");
                    break;
            }
        }
    }

    private void UpdateAvailability(Driver driver)
    {
        Console.Write("Change availability to Available (Y/N): ");
        Console.ForegroundColor = ConsoleColor.Green;
        char response = Console.ReadKey().KeyChar;
        Console.ForegroundColor = ConsoleColor.Gray;
        if (response == 'Y' || response == 'y')
        {
            driver.Availability = true;
        }
        else if (response == 'N' || response == 'n')
        {
            driver.Availability = false;
        }
        else
        {
            Console.WriteLine("Invalid input Availability not changed:) ");
        }
    }

    private void UpdateLocation(Driver driver)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Enter your new location (latitude,longitude): ");
        Console.ForegroundColor = ConsoleColor.Gray;
        string[] locationInput = Console.ReadLine().Split(',');

        if (locationInput.Length == 2 && double.TryParse(locationInput[0], out double latitude) && double.TryParse(locationInput[1], out double longitude))
        {
            driver.CurrLocation = new Location { Latitude = latitude, Longitude = longitude };
            Console.WriteLine("Location updated successfully.");
        }
        else
        {
            Console.WriteLine("Invalid location input!! Location not updated:)");
        }
    }

    public Driver CheckDriverRegistration(int driverId, string name, Admin admin)
    {
        List<Driver> driversList = admin.Drivers;
        var registeredDriver = driversList.FirstOrDefault(driver => driver.Id == driverId && driver.Name == name);

        if (registeredDriver != null)
        {
            Console.Write($"Welcome back, {name}! Enter your current Location (latitude,longitude): ");
            Console.ForegroundColor = ConsoleColor.Green;
            string[] locationInput = Console.ReadLine().Split(',');
            Console.ForegroundColor = ConsoleColor.Gray;

            if (locationInput.Length == 2 && double.TryParse(locationInput[0], out double latitude) && double.TryParse(locationInput[1], out double longitude))
            {
                registeredDriver.CurrLocation = new Location { Latitude = latitude, Longitude = longitude };
            }
            else
            {
                Console.WriteLine("Invalid location input!! Please enter latitude and longitude separated by a comma:)");
            }
        }

        return registeredDriver;
    }

    public double GetRating()
    {
        if (Rating.Count == 0)
            return 0;
        return Rating.Average();
    }
}

class Passenger
{
    public string Name { get; set; }
    public string PhoneNo { get; set; }

    public Passenger()
    {
    }

    public void BookRide(Admin admin, Driver driver)
    {
        Console.WriteLine("Book a Ride");

        Console.Write("Enter Name: ");
        Console.ForegroundColor = ConsoleColor.Green;
        Name = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        while (string.IsNullOrEmpty(Name))
        {
            Console.WriteLine("Name cannot be empty!! Please enter a valid name :)");
            Console.Write("Enter Name: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Name = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        Console.Write("Enter Phone no: ");
        Console.ForegroundColor = ConsoleColor.Green;
        PhoneNo = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        while (!IsValidPhoneNumber(PhoneNo))
        {
            Console.WriteLine("Invalid phone number!! Please enter a valid 11-digit phone number starting with '03':)");
            Console.Write("Enter Phone no: ");
            Console.ForegroundColor = ConsoleColor.Green;
            PhoneNo = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        Location startLocation = GetLocationInput("Enter Start Location (latitude,longitude): ");
        Location endLocation = GetLocationInput("Enter End Location (latitude,longitude): ");

        Console.Write("Enter Ride Type (Car, Bike, Rickshaw): ");
        Console.ForegroundColor = ConsoleColor.Green;
        string rideType = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        while (string.IsNullOrEmpty(rideType) || !IsValidRideType(rideType))
        {
            Console.WriteLine("Invalid ride type!! Please enter Car, Bike, or Rickshaw :)");
            Console.Write("Enter Ride Type (Car, Bike, Rickshaw): ");
            Console.ForegroundColor = ConsoleColor.Green;
            rideType = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        Console.WriteLine("-------------------- THANK YOU ----------------------");

        Ride ride = new Ride
        {
            StartLocation = startLocation,
            EndLocation = endLocation,
            Passenger = this
        };

        if (ride.AssignDriver(admin,driver))
        {
            int price = ride.CalculatePrice(rideType);
            Console.WriteLine($"Price for this ride is: {price}");

            Console.Write("Enter 'Y' if you want to Book the ride, enter 'N' if you want to cancel operation: ");
            char bookChoice;
            Console.ForegroundColor = ConsoleColor.Green;
            while (!char.TryParse(Console.ReadKey().KeyChar.ToString().ToLower(), out bookChoice) || (bookChoice != 'y' && bookChoice != 'n'))
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\nInvalid input!! Please enter 'Y' or 'N' :)");
                Console.Write("Enter 'Y' if you want to Book the ride, enter 'N' if you want to cancel operation: ");
                Console.ForegroundColor = ConsoleColor.Green;
            }

            if (bookChoice == 'y')
            {
                Console.WriteLine("\nHappy Travel...!");
                int rating = GiveRating();
                Console.WriteLine($"You have given a rating of {rating} out of 5 for this ride.");
            }
            else
            {
                Console.WriteLine("\nRide booking canceled :( ");
            }
        }
    }
    private bool IsValidPhoneNumber(string phoneNumber)
    {
        return phoneNumber.Length == 11 && phoneNumber.StartsWith("03");
    }
    private bool IsValidRideType(string rideType)
    {
        string[] validRideTypes = { "car", "bike", "rickshaw" };
        return validRideTypes.Contains(rideType.ToLower());
    }

    private Location GetLocationInput(string prompt)
    {
        Console.Write(prompt);
        Console.ForegroundColor = ConsoleColor.Green;
        string[] locationInput = Console.ReadLine().Split(',');
        Console.ForegroundColor = ConsoleColor.Gray;

        if (locationInput.Length != 2 || !double.TryParse(locationInput[0], out double latitude) || !double.TryParse(locationInput[1], out double longitude))
        {
            Console.WriteLine("Invalid location input!! Please enter latitude and longitude separated by a comma :)");
            return GetLocationInput(prompt);
        }

        return new Location { Latitude = latitude, Longitude = longitude };
    }

    private int GiveRating()
    {
        Console.Write("Give rating out of 5: ");
        Console.ForegroundColor = ConsoleColor.Green;
        int rating = int.Parse(Console.ReadLine());
        Console.ForegroundColor = ConsoleColor.Gray;
        if (rating < 1 || rating > 5)
        {
            Console.WriteLine("Invalid rating!! Please enter a rating between 1 and 5 :)");
            return GiveRating();
        }
        return rating;
    }
}

class Ride
{
    public Location StartLocation { get; set; }
    public Location EndLocation { get; set; }
    public int Price { get; set; }
    public Driver Driver { get; set; }
    public Passenger Passenger { get; set; }

    public void AssignPassenger(Passenger passenger)
    {
        Passenger = passenger;
    }

    public bool AssignDriver(Admin admin,Driver driver)
    {
        List<Driver> availableDrivers = admin.Drivers;
        Driver closestDriver = FindClosestDriver(availableDrivers, StartLocation);

        if (closestDriver != null)
        {
            closestDriver.Availability = false;
            Driver = closestDriver;
            Console.WriteLine($"Assigned Driver: {closestDriver.Name}");
            return true;
        }
        else
        {
            Console.WriteLine("No available drivers at the moment :( ");
            return false;
        }
    }

    private Driver FindClosestDriver(List<Driver> drivers, Location startLocation)
    {
        double minDistance = double.MaxValue;
        Driver closestDriver = null;

        foreach (var driver in drivers)
        {
            double distance = CalculateDistance(startLocation, driver.CurrLocation);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestDriver = driver;
            }
        }

        return closestDriver;
    }

    public void GetLocations(string startLocation, string endLocation)
    {
        StartLocation = new Location();
        EndLocation = new Location();
        StartLocation.SetLocation(startLocation);
        EndLocation.SetLocation(endLocation);
    }

    public int CalculatePrice(string rideType)
    {
        double fuelPrice = 332;
        double commission;

        double distance = CalculateDistance(StartLocation, EndLocation);

        double fuelAverage;
        switch (rideType.ToLower())
        {
            case "car":
                fuelAverage = 15;
                commission = 0.20;
                break;
            case "bike":
                fuelAverage = 50;
                commission = 0.05;
                break;
            case "rickshaw":
                fuelAverage = 35;
                commission = 0.10;
                break;
            default:
                Console.WriteLine("Invalid ride type!! Please choose Car, Bike, or Rickshaw :)");
                return 0;
        }

        double price = (distance * fuelPrice / fuelAverage);
        price += price * commission;

        Price = (int)Math.Round(price);
        return Price;
    }

    private double CalculateDistance(Location startLocation, Location endLocation)
    {
        if (startLocation == null || endLocation == null)
        {
            throw new ArgumentNullException("Both startLocation and endLocation must be non-null.");
        }
        double latDiff = endLocation.Latitude - startLocation.Latitude;
        double longDiff = endLocation.Longitude - startLocation.Longitude;
        return Math.Sqrt(latDiff * latDiff + longDiff * longDiff);
    }
}


class Admin
{
    public List<Driver> Drivers { get; set; }

    public Admin()
    {
        Drivers = new List<Driver>();
    }
    public void DisplayAdminMenu()
    {
        while (true)
        {
            Console.WriteLine("\nEnter as Admin:");
            Console.WriteLine("1. Add Driver");
            Console.WriteLine("2. Remove Driver");
            Console.WriteLine("3. Update Driver");
            Console.WriteLine("4. Search Driver");
            Console.WriteLine("5. Exit as Admin");
            Console.Write("Select an option (1-5): ");

            Console.ForegroundColor = ConsoleColor.Green;
            int adminChoice = int.Parse(Console.ReadLine());
            Console.ForegroundColor = ConsoleColor.Gray;

            switch (adminChoice)
            {
                case 1:
                    AddDriver();
                    break;
                case 2:
                    RemoveDriver();
                    break;
                case 3:
                    UpdateDriver();
                    break;
                case 4:
                    SearchDriver();
                    break;
                case 5:
                    Console.WriteLine("Exiting as Admin. Redirecting to Main Menu.");
                    return;
                default:
                    Console.WriteLine("Invalid choice!! Please select a valid option :)");
                    break;
            }
        }
    }

    private void AddDriver()
    {
        Console.WriteLine("\nAdd Driver:");

        int driverId;
        do
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Enter Driver ID: ");
            Console.ForegroundColor = ConsoleColor.Green;
        } while (!int.TryParse(Console.ReadLine(), out driverId) || driverId <= 0);
        Console.ForegroundColor = ConsoleColor.Gray;

        Console.Write("Enter Name: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string name = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        while (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Invalid input!! Please enter a non-empty name :)");
            Console.Write("Enter Name: ");
            Console.ForegroundColor = ConsoleColor.Green;
            name = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        int age;
        do
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Enter Age: ");
            Console.ForegroundColor = ConsoleColor.Green;
        } while (!int.TryParse(Console.ReadLine(), out age) || age < 18 || age > 70);
        Console.ForegroundColor = ConsoleColor.Gray;

        Console.Write("Enter Gender: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string gender = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        while (string.IsNullOrWhiteSpace(gender) || (gender.ToLower() != "male" && gender.ToLower() != "female"))
        {
            Console.WriteLine("Invalid input!! Please enter 'Male' or 'Female' for gender :)");
            Console.Write("Enter Gender: ");
            Console.ForegroundColor = ConsoleColor.Green;
            gender = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        Console.Write("Enter Address: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string address = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        while (string.IsNullOrWhiteSpace(address))
        {
            Console.WriteLine("Invalid input!! Please enter a non-empty address :)");
            Console.Write("Enter Address: ");
            Console.ForegroundColor = ConsoleColor.Green;
            address = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        Console.Write("Enter Vehicle Type (Car, Bike, Rickshaw): ");
        Console.ForegroundColor = ConsoleColor.Green;
        string vehicleType = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;

        while (string.IsNullOrWhiteSpace(vehicleType) || !IsVehicleTypeValid(vehicleType))
        {
            Console.WriteLine("Invalid input!! Please enter a valid vehicle type (Car, Bike, Rickshaw) :) ");
            Console.Write("Enter Vehicle Type: ");
            Console.ForegroundColor = ConsoleColor.Green;
            vehicleType = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        Console.Write("Enter Vehicle Model: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string vehicleModel = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        while (string.IsNullOrWhiteSpace(vehicleModel))
        {
            Console.WriteLine("Invalid input!! Please enter a non-empty vehicle model :)");
            Console.Write("Enter Vehicle Model: ");
            Console.ForegroundColor = ConsoleColor.Green;
            vehicleModel = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        Console.Write("Enter Vehicle License Plate: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string licensePlate = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        while (string.IsNullOrWhiteSpace(licensePlate))
        {
            Console.WriteLine("Invalid input!! Please enter a non-empty license plate :)");
            Console.Write("Enter Vehicle License Plate: ");
            Console.ForegroundColor = ConsoleColor.Green;
            licensePlate = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        Driver newDriver = new Driver
        {
            Id = driverId,
            Name = name,
            Age = age,
            Gender = gender,
            Address = address,
            Vehicle = new Vehicle { Type = vehicleType, Model = vehicleModel, LicensePlate = licensePlate },
            CurrLocation = new Location { Latitude = 0, Longitude = 0 },
            Rating = new List<int>(),
            Availability = false,
        };

        Drivers.Add(newDriver);
        Console.WriteLine("Driver added successfully.");
    }

    private static bool IsVehicleTypeValid(string input)
    {
        string[] validTypes = { "car", "bike", "rickshaw" };
        return validTypes.Contains(input.ToLower());
    }
    private void RemoveDriver()
    {
        Console.Write("Enter Driver ID to remove :");
        int driverId;
        Console.ForegroundColor = ConsoleColor.Green;
        while (!int.TryParse(Console.ReadLine(), out driverId) || driverId <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Invalid input!! Please enter a valid driver ID :)");
            Console.Write("Enter Driver ID to remove: ");
            Console.ForegroundColor = ConsoleColor.Green;
        }

        Driver driverToRemove = Drivers.FirstOrDefault(d => d.Id == driverId);

        if (driverToRemove != null)
        {
            Drivers.Remove(driverToRemove);
            Console.WriteLine("Driver removed successfully.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Driver not found with the given ID.");
        }
    }

    private void UpdateDriver()
    {
        Console.Write("\nEnter Driver ID to update: ");
        int driverId;
        Console.ForegroundColor = ConsoleColor.Green;
        while (!int.TryParse(Console.ReadLine(), out driverId) || driverId <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Invalid input!! Please enter a valid driver ID :)");
            Console.Write("Enter Driver ID to update: ");
            Console.ForegroundColor = ConsoleColor.Green;
        }
        Console.ForegroundColor = ConsoleColor.Gray;

        Driver driverToUpdate = Drivers.FirstOrDefault(d => d.Id == driverId);

        if (driverToUpdate != null)
        {
            Console.WriteLine($"\n-------------Driver with ID {driverId} exists-------------");

            Console.Write("Enter Name: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string name = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;

            if (!string.IsNullOrWhiteSpace(name))
            {
                driverToUpdate.Name = name;
            }

            Console.Write("Enter Age: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string ageInput = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            
            if (!string.IsNullOrWhiteSpace(ageInput))
            driverToUpdate.Age = int.Parse(ageInput);

            Console.Write("Enter Gender: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string gender = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            
            if(!string.IsNullOrEmpty(gender))
            driverToUpdate.Gender = gender;

            Console.Write("Enter Address: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string address = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;

            if(!string.IsNullOrEmpty(address))
            driverToUpdate.Address = address;

            Console.Write("Enter Vehicle Type (Car, Bike, Rickshaw): ");
            Console.ForegroundColor = ConsoleColor.Green;
            string vehicleType = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;

            if(!string.IsNullOrEmpty(vehicleType))
            driverToUpdate.Vehicle.Type = vehicleType;

            Console.Write("Enter Vehicle Model: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string vehicleModel = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;

            if(!string.IsNullOrEmpty(vehicleModel))
            driverToUpdate.Vehicle.Model = vehicleModel;

            Console.Write("Enter Vehicle License Plate: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string licensePlate = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;

            if(!string.IsNullOrEmpty(licensePlate))
            driverToUpdate.Vehicle.LicensePlate = licensePlate;

            Console.WriteLine("Driver updated successfully.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Driver not found with the given ID.");
        }
    }
    private void SearchDriver()
    {
        Console.WriteLine("\nSearch Driver:");

        Console.Write("Enter Driver ID: ");
        Console.ForegroundColor = ConsoleColor.Green;
        int searchDriverId = int.Parse(Console.ReadLine());
        Console.ForegroundColor = ConsoleColor.Gray;

        Console.Write("Enter Name: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string searchName = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;

        Console.Write("Enter Age: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string searchAgeInput = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;

        Console.Write("Enter Gender: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string searchGender = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;

        Console.Write("Enter Address: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string searchAddress = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;

        Console.Write("Enter Vehicle Type: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string searchVehicleType = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;

        Console.Write("Enter Vehicle Model: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string searchVehicleModel = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;


        Console.Write("Enter Vehicle License Plate: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string searchLicensePlate = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Gray;

        var searchResults = Drivers.Where(d =>
                (searchDriverId == 0 || d.Id == searchDriverId) &&
                (string.IsNullOrEmpty(searchName) || d.Name.Contains(searchName)) &&
                (string.IsNullOrEmpty(searchAgeInput) || d.Age.ToString() == searchAgeInput) &&
                (string.IsNullOrEmpty(searchGender) || d.Gender.Contains(searchGender)) &&
                (string.IsNullOrEmpty(searchAddress) || d.Address.Contains(searchAddress)) &&
                (string.IsNullOrEmpty(searchVehicleType) || d.Vehicle.Type.Contains(searchVehicleType)) &&
                (string.IsNullOrEmpty(searchVehicleModel) || d.Vehicle.Model.Contains(searchVehicleModel)) &&
                (string.IsNullOrEmpty(searchLicensePlate) || d.Vehicle.LicensePlate.Contains(searchLicensePlate)))
            .ToList();

        Console.WriteLine("\nSearch Results:");
        DisplaySearchResults(searchResults);
    }

    private void DisplaySearchResults(List<Driver> searchResults)
    {
        Console.WriteLine("--------------------------------------------------------------------------------------------------------");
        Console.WriteLine("Name     Age     Gender   V.Type   V.Model   V.License");
        Console.WriteLine("--------------------------------------------------------------------------------------------------------");

        foreach (var driver in searchResults)
        {
            Console.WriteLine($"{driver.Name,-8} {driver.Age,-7} {driver.Gender,-8} {driver.Vehicle.Type,-8} {driver.Vehicle.Model,-9} {driver.Vehicle.LicensePlate}");
        }

        Console.WriteLine("--------------------------------------------------------------------------------------------------------");
    }

}
