﻿using System;
using System.Collections.Generic;
using WildFarm.Core.Contracts;
using WildFarm.Exceptions;
using WildFarm.Factories;
using WildFarm.IO;
using WildFarm.Models.Animals;
using WildFarm.Models.Animals.Contracts;
using WildFarm.Models.Foods.Contracts;

namespace WildFarm.Core
{
    public class Engine : IEngine
    {
        private ConsoleReader reader;
        private ConsoleWriter writer;
        private ICollection<IAnimal> animals;
        private FoodFactory foodFactory;

        private Engine()
        {
            this.animals = new List<IAnimal>();
            this.foodFactory = new FoodFactory();
        }

        public Engine(ConsoleReader reader, ConsoleWriter writer)
        : this()
        {
            this.reader = reader;
            this.writer = writer;
        }

        public void Run()
        {
            string command;

            while ((command = reader.ReadLine()) != "End")
            {
                var animalArgs = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var foodArgs = reader.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var animal = ProduceAnimal(animalArgs);
                this.animals.Add(animal);

                var food = ProduceFood(foodArgs);

                writer.WriteLine(animal.ProduceSound());

                try
                {
                    animal.Feed(food);
                }
                catch (UneatableFoodException ufe)
                {
                    writer.WriteLine(ufe.Message);
                }
            }

            foreach (var animal in this.animals)
            {
                writer.WriteLine(animal.ToString());
            }

        }

        private IFood ProduceFood(string[] foodArgs)
        {
            IFood food;
            var type = foodArgs[0];
            var quantity = int.Parse(foodArgs[1]);
            food = foodFactory.ProduceFood(type, quantity);
            return food;
        }

        private static IAnimal ProduceAnimal(string[] animalArgs)
        {
            IAnimal animal = null;

            var animalType = animalArgs[0];
            var name = animalArgs[1];
            var weight = double.Parse(animalArgs[2]);

            if (animalType == "Owl")
            {
                var wingSize = double.Parse(animalArgs[3]);
                animal = new Owl(name, weight, wingSize);
            }
            else if (animalType == "Hen")
            {
                var wingSize = double.Parse(animalArgs[3]);
                animal = new Hen(name, weight, wingSize);
            }
            else
            {
                var livingRegion = animalArgs[3];
                if (animalType == "Dog")
                {
                    animal = new Dog(name, weight, livingRegion);
                }
                else if (animalType == "Mouse")
                {
                    animal = new Mouse(name, weight, livingRegion);
                }
                else
                {
                    var breed = animalArgs[4];
                    if (animalType == "Tiger")
                    {
                        animal = new Tiger(name, weight, livingRegion, breed);
                    }
                    else if (animalType == "Cat")
                    {
                        animal = new Cat(name, weight, livingRegion, breed);
                    }
                }
            }

            return animal;
        }
    }
}
