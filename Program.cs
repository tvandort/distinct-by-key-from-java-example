using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        var random = new Random();

        Console.WriteLine("Hello World!");

        var companies = Enumerable.Range(0, 10)
            .Select(id => new Company(id)).ToList();

        var orders = Enumerable.Range(0, 10)
            .Select(id => new Order(companies[id])).ToList();

        var order1 = new Order(companies.First());
        var order2 = new Order(companies.First());

        orders.AddRange(new[] { order1, order2 });

        var distinct = orders
            .Where(DistinctByKey((Order order) => order.Company.Id))
            .ToList();

        var result = distinct
            .Select(order => order.Share)
            .Aggregate(0, (accumulation, share) => accumulation + share);

        Console.WriteLine($"Result: {result}");
        Console.WriteLine($"Companies Count: {companies.Count}");
        Console.WriteLine($"Orders Count: {orders.Count}");
        Console.WriteLine($"Distinct Keys Count: {distinct.Count}");
    }

    public static Func<T, bool> DistinctByKey<T, U>(Func<T, U> keyExtractor)
    {
        ConcurrentDictionary<U, bool> seen = new ConcurrentDictionary<U, bool>();
        return t => seen.TryAdd(keyExtractor(t), true);
    }
}

public class Order
{
    public Order(Company company)
    {
        Company = company;
    }

    public Company Company { get; private set; }

    public int Share
    {
        get
        {
            return 1;
        }
    }
}

public class Company
{
    public Company(int id)
    {
        Id = id;
    }

    public int Id
    {
        set;
        get;
    }
}



