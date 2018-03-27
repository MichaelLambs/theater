using System;
using System.Collections.Generic;
using theater.Assets.Enums;

namespace theater.Models
{
    public class BoxOffice
    {
        public List<Movie> Movies = new List<Movie>();

        public Movie selectedMovie { get; set; }

        public void SetupBoxOffice()
        {
            Director ryan = new Director() { Name = "Ryan Coogler" };
            var bp = new Movie() { Title = "Black Panther", Runtime = 90, Rating = Ratings.PG13, Director = ryan };
            var creed = new Movie() { Title = "Creed", Runtime = 90, Rating = Ratings.PG13, Director = ryan };
            Movies.Add(bp);
            Movies.Add(creed);
        }

        public void ShowMovies()
        {
            int choice = -1;
            while (choice == -1)
            {
                var i = 1;
                Movies.ForEach(movie =>
                {
                    System.Console.WriteLine($@"
          -------------------------------------
          |{i}. {movie.Title} | {movie.Runtime} | {movie.Rating}
          -------------------------------------
          ");
                    i++;
                });
                System.Console.WriteLine("Pick a Movie to Add");
                var userInput = Console.ReadLine();
                int.TryParse(userInput, out choice);
            }
            selectedMovie = Movies[choice - 1];
            System.Console.WriteLine($"You are adding {selectedMovie.Title}");
        }


    }
}