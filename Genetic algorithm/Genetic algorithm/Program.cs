using System;
using System.Collections.Generic;
using System.Linq;

public class GeneticAlgorithm
{
    private static readonly Random random = new Random();
    private const int PopulationSize = 100;
    private const int Generations = 1000;
    private const double MutationRate = 0.05;

    public static void Main()
    {
        List<int[]> population = InitializePopulation();

        for (int generation = 0; generation < Generations; generation++)
        {
            population = population.OrderBy(ComputeFitness).ToList();
            List<int[]> newPopulation = new List<int[]>();

            while (newPopulation.Count < PopulationSize)
            {
                int[] parent1 = TournamentSelection(population);
                int[] parent2 = TournamentSelection(population);

                int[] offspring1, offspring2;
                Crossover(parent1, parent2, out offspring1, out offspring2);

                if (random.NextDouble() < MutationRate)
                    Mutate(offspring1);

                if (random.NextDouble() < MutationRate)
                    Mutate(offspring2);

                newPopulation.Add(offspring1);
                newPopulation.Add(offspring2);
            }

            population = newPopulation;

            // Выводим лучшую особь текущего поколения
            int[] bestIndividual = population.OrderBy(ComputeFitness).First();
            Console.WriteLine($"Поколение {generation + 1}: Лучший порядок - {string.Join(", ", bestIndividual)}, Сумма расстояний: {-ComputeFitness(bestIndividual)}");
        }
    }

    private static List<int[]> InitializePopulation()
    {
        List<int[]> population = new List<int[]>();
        for (int i = 0; i < PopulationSize; i++)
        {
            int[] individual = Enumerable.Range(1, 10).OrderBy(x => random.Next()).ToArray();
            population.Add(individual);
        }
        return population;
    }

    private static int ComputeFitness(int[] individual)
    {
        int fitness = 0;
        for (int i = 0; i < individual.Length - 1; i++)
        {
            fitness -= Math.Abs(individual[i + 1] - individual[i]);
        }
        return fitness; // Отрицательная сумма для минимизации
    }

    private static int[] TournamentSelection(List<int[]> population)
    {
        int tournamentSize = 5;
        int[] best = null;

        for (int i = 0; i < tournamentSize; i++)
        {
            int[] individual = population[random.Next(population.Count)];
            if (best == null || ComputeFitness(individual) > ComputeFitness(best))
                best = individual;
        }

        return best;
    }

    private static void Crossover(int[] parent1, int[] parent2, out int[] offspring1, out int[] offspring2)
    {
        offspring1 = new int[parent1.Length];
        offspring2 = new int[parent2.Length];

        int start = random.Next(parent1.Length);
        int end = random.Next(start, parent1.Length);

        Array.Copy(parent1, start, offspring1, start, end - start + 1);
        Array.Copy(parent2, start, offspring2, start, end - start + 1);

        FillOffspring(offspring1, parent2, start, end);
        FillOffspring(offspring2, parent1, start, end);
    }

    private static void FillOffspring(int[] offspring, int[] parent, int start, int end)
    {
        HashSet<int> used = new HashSet<int>(offspring.Skip(start).Take(end - start + 1));

        int index = 0;
        for (int i = 0; i < parent.Length; i++)
        {
            if (!used.Contains(parent[i]))
            {
                while (index >= start && index <= end)
                    index++;

                offspring[index++] = parent[i];
            }
        }
    }

    private static void Mutate(int[] individual)
    {
        int index1 = random.Next(individual.Length);
        int index2 = random.Next(individual.Length);
        int temp = individual[index1];
        individual[index1] = individual[index2];
        individual[index2] = temp;
    }
}