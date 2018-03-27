using System;
using System.Collections.Generic;
using theater.Models;

namespace theater
{
    public class Application
    {
        public Dictionary<string, User> UsersTable = new Dictionary<string, User>();
        private Dictionary<User, List<Cinema>> UserCinemas = new Dictionary<User, List<Cinema>>();

        private User _activeUser { get; set; } // CREATES A PRIVATE USER FOR THIS CLASS - USING USER MODEL
        private Cinema _activeCinema { get; set; } // CREATE A PRIVE CINEMA FOR THIS CLASS - USING CINEMA MODEL
        private BoxOffice _bx = new BoxOffice(); // CREATES A PRIVATE BOXOFFICE WHICH IS A LIST OF MOVIES
        public void Start()
        {
            _bx.SetupBoxOffice();
            Console.Clear();
            _activeUser = GetUser(); // SETS ACTIVE USER TO RETURN VALUE OF GETUSER FUNCTION BELOW
            CinemaManagerMenu();
        }

        public void CinemaManagerMenu()
        {
            System.Console.WriteLine("Welcome to My Cinema Manager");
            System.Console.WriteLine("What would you like to do?");
            System.Console.WriteLine(@"
            1 - Create Cinema
            2 - Manage Cinema
        ");
            var userInput = Console.ReadLine();
            switch (userInput)
            {
                case "1":
                    CreateCinema();
                    break;
                case "2":
                    ManageCinemas();
                    break;
                default:
                    CinemaManagerMenu();
                    break;
            }
        }

        public User GetUser() // VALIDATES USER LOGIN
        {
            var invalid = true;
            while (invalid)
            {
                //Get username
                Console.Write("Welcome Please Provide UserName: ");
                string usernameInput = Console.ReadLine();

                //Validate user on table
                if (UsersTable.ContainsKey(usernameInput)) // USERSTABLE IS A DICTIONARY OF USERS
                {
                    //Get Password
                    Console.Write("Password: ");
                    string passwordInput = Console.ReadLine();
                    var user = UsersTable[usernameInput];
                    //Validate Password
                    if (user.ValidatePassword(passwordInput))
                    {
                        Console.WriteLine("Welcome " + usernameInput);
                        invalid = false;
                        return user;
                    }
                    else
                    {
                        Console.Clear();
                        System.Console.WriteLine("Invalid Password");
                        GetUser();
                    }

                }
                else
                {
                    Console.Clear();
                    System.Console.WriteLine("User not found.");
                }
            }
            return GetUser();
        }


        public void ManageCinemas()
        {
            if (!UserCinemas.ContainsKey(_activeUser))
            {
                CinemaManagerMenu();
                return;
            }
            int c = -1;
            while (c == -1)
            {

                Console.Clear();
                var i = 1;
                UserCinemas[_activeUser].ForEach(ci =>
                {
                    System.Console.WriteLine($"{i}: {ci.Name} - {ci.Address.StreetAddress}");
                    i++;
                });
                System.Console.WriteLine("Which Cinema?");
                var userInput = Console.ReadLine();
                int.TryParse(userInput, out c);
            }
            _activeCinema = UserCinemas[_activeUser][c - 1];
            ManageCinema();
        }

        private void ManageCinema()
        {
            Console.Clear();
            System.Console.WriteLine($"Managing {_activeCinema.Name}");
            System.Console.WriteLine("What would you like to do?");
            Console.WriteLine(@"
        1 - Display Showtimes
        2 - Add Showtime
        3 - Add Theater
        4 - Back
      ");

            switch (Console.ReadLine())
            {
                case "1":
                    _activeCinema.PrintShowtimes();
                    System.Console.WriteLine($@"
                    1 - Go Back To Home
                    2 - Quit Application
                    ");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            ManageCinema();
                            break;
                        default:
                            break;
                    }
                    break;
                case "2":
                    _activeCinema.ShowTheaters();
                    _bx.ShowMovies();
                    System.Console.WriteLine("What showtime do you want?");
                    var time = Console.ReadLine();
                    // Select from a list of theaters of the _activeCinema
                    // select from a list of Movies from the _bx
                    // set the time from the userInput
                    _activeCinema.CreateShowtime(_activeCinema.activeTheater, _bx.selectedMovie, time);
                    ManageCinema();
                    break;
                case "3":
                    _activeCinema.CreateTheaterDetails();
                    System.Console.WriteLine("Good Jorb");
                    ManageCinema();
                    break;
                case "4":
                    ManageCinemas();
                    break;
                default:
                    ManageCinema();
                    break;
            }

        }

        public void CreateCinema()
        {
            Console.Clear();
            System.Console.WriteLine("Okay let's create your Cinema");
            System.Console.WriteLine("What is the name of your Cinema?");
            var cinemaName = Console.ReadLine();
            Console.Clear();
            System.Console.WriteLine("What is the address of your Cinema?");
            var cinemaAddress = Console.ReadLine();
            Console.Clear();

            double tp = -1;
            while (tp < 0)
            {
                System.Console.WriteLine("What about the default ticket price?");
                var cinemaTP = Console.ReadLine();
                double.TryParse(cinemaTP, out tp);
            }

            Console.Clear();

            System.Console.WriteLine("Is the following correct?");
            System.Console.WriteLine($@"
      Name: {cinemaName},
      Address: {cinemaAddress},
      Default Ticket Price: ${tp}
      ");
            System.Console.WriteLine("Y/N: ");
            var userInput = Console.ReadLine().ToLower();
            switch (userInput)
            {
                case "y":
                    Cinema cinema = new Cinema()
                    {
                        Name = cinemaName,
                        Address = new Address() { StreetAddress = cinemaAddress },
                        DefaultTicketPrice = tp
                    };

                    if (UserCinemas.ContainsKey(_activeUser))
                    {
                        //always at least the second one
                        UserCinemas[_activeUser].Add(cinema);
                    }
                    else
                    {
                        UserCinemas.Add(_activeUser, new List<Cinema>());
                        UserCinemas[_activeUser].Add(cinema);
                    }
                    Console.WriteLine("Successfully added a new Cinema");
                    CinemaManagerMenu();
                    break;
                default:
                    CreateCinema();
                    break;
            }
        }
    }
}