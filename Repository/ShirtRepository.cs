using ApiDemo.Models;

namespace ApiDemo.Repository;

// in-memory db
public static class ShirtRepository
{
    private static List<Shirt> shirts = new List<Shirt>()
    {
        new Shirt { ShirtId = 1, Brand = "Levi's", Color = "Blue", Gender = "Men", Price = 30, Size=10},
        new Shirt { ShirtId = 2, Brand = "MLH", Color = "Red", Gender = "Men", Price = 35, Size=12},
        new Shirt { ShirtId = 3, Brand = "Apple", Color = "White", Gender = "Women", Price = 22, Size=8},
        new Shirt { ShirtId = 4, Brand = "Nike", Color = "Black", Gender = "Women", Price = 33, Size=9},

    };

    public static bool ShirtExists(int id)
    {
        return shirts.Any(x => x.ShirtId == id);
    }

    public static Shirt? GetShritById(int id)
    {
        return shirts.FirstOrDefault(x => x.ShirtId == id);
    }
}