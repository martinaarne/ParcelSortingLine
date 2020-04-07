using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SortingLine.Interfaces;
using SortingLine.Models;
using SortingLine.Services;

namespace SortingLine
{
    class Program
    {
        private const string EXIT = "exit";
        static void Main(string[] args)
        {
            var serviceProvider = SetupDI();

            Console.WriteLine("Welcome to The Parcel Sorting Line program!");
            Console.WriteLine("You need to input comma-separated data for the program to tell you if your parcel can fit through the sorting line.");
            Console.WriteLine("The first two integers of the input will be the width and length of the package, and the following integers will be the width of the sorting line parts.");
            Console.WriteLine($"Please input the data about the parcel and sorting line:");

            StartSortingLine(serviceProvider);

            serviceProvider.Dispose();
        }

        private static void StartSortingLine(ServiceProvider serviceProvider)
        {
            Console.Write("Input data: ");
            var inputData = Console.ReadLine();

            do
            {
                var package = GetPackage(inputData);
                var sortingLine = GetSortingLine(inputData);

                if (package == null || sortingLine == null)
                {
                    Console.WriteLine(
                        "Invalid input! Please see above error(s) to see what went wrong. Please try again with valid input");
                }
                else
                {
                    var sortingLineService = serviceProvider.GetService<IPackageSortingLineService>();

                    var result = sortingLineService.CanPackagePassThrough(package, sortingLine);

                    if (result.Success)
                    {
                        Console.WriteLine("Congratulations! Your Parcel can fit through the Sorting Line");
                    }
                    else
                    {
                        Console.WriteLine("I'm sorry, Your Parcel can not fit through the Sorting Line. Here's why:");
                        foreach (var brokenRule in result.BrokenRules)
                        {
                            Console.WriteLine(brokenRule.Message);
                        }
                    }
                }

                Console.WriteLine($"To exit, please type \"{EXIT}\" and hit Enter. To try again, you should simply enter new comma-separated data");
                Console.Write("Input data: ");
                inputData = Console.ReadLine();
            } while (!string.Equals(inputData, EXIT, StringComparison.InvariantCultureIgnoreCase));
        }

        private static PackageSortingLine GetSortingLine(string inputData)
        {
            try
            {
                var splitInput = inputData.Split(',');

                var lineParts = new List<PackageSortingLineSegment>(splitInput.Length - 2);
                for (int i = 2; i < splitInput.Length; i++)
                {
                    var widthStr = splitInput[i].Trim();

                    if (!int.TryParse(widthStr, out var pipeWidth))
                    {
                        Console.WriteLine(
                            $"Invalid Width of Sorting Line! Cannot convert \"{widthStr}\" at position {i + 1} to an integer");
                        return null;
                    }

                    lineParts.Add(new PackageSortingLineSegment {Width = pipeWidth});
                }

                return new PackageSortingLine {Segments = lineParts};
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not get data for Sorting Line due to unknown error " + e);
                return null;
            }
        }

        private static Package GetPackage(string inputData)
        {
            try
            {
                var splitInput = inputData.Split(',');

                var widthStr = splitInput[0].Trim();
                var lengthStr = splitInput[1].Trim();

                if (!int.TryParse(widthStr, out var width))
                {
                    Console.WriteLine($"Invalid Width! Cannot convert \"{widthStr}\" to an integer");
                    return null;
                }

                if (!int.TryParse(lengthStr, out var length))
                {
                    Console.WriteLine($"Invalid Length! Cannot convert \"{lengthStr}\" to an integer");
                    return null;
                }

                return new Package {Length = length, Width = width};
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not get data for Package due to unknown error " + e);
                return null;
            }
        }

        private static ServiceProvider SetupDI()
        {
            return new ServiceCollection()
                .AddSingleton<IPackageSortingLineService, PackageSortingLineService>()
                .BuildServiceProvider();
        }
    }
}
