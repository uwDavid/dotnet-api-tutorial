using WebApp.Models;

namespace WebApp.Repository;

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

    public static List<Shirt> GetShirts()
    {
        return shirts;
    }

    public static bool ShirtExists(int id)
    {
        return shirts.Any(x => x.ShirtId == id);
    }

    public static Shirt? GetShritById(int id)
    {
        return shirts.FirstOrDefault(x => x.ShirtId == id);
    }

    public static void AddShirt(Shirt shirt)
    {
        // we need to add ShirtId to this obj
        int maxId = shirts.Max(x => x.ShirtId);
        shirt.ShirtId = maxId + 1;

        shirts.Add(shirt);
    }

    // Find shirts with similar features
    public static Shirt? GetShritByProperties(string? brand, string? gender, string? color, int? size)
    {
        // note the ? corresponds to Shirt Model
        return shirts.FirstOrDefault(x =>
            !string.IsNullOrWhiteSpace(brand) &&
            !string.IsNullOrWhiteSpace(x.Brand) &&
            x.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase) &&

            !string.IsNullOrWhiteSpace(gender) &&
            !string.IsNullOrWhiteSpace(x.Gender) &&
            x.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase) &&

            !string.IsNullOrWhiteSpace(color) &&
            !string.IsNullOrWhiteSpace(x.Color) &&
            x.Color.Equals(color, StringComparison.OrdinalIgnoreCase) &&

            size.HasValue &&
            x.Size.HasValue &&
            size.Value == x.Size.Value
        );
    }


    public static void UpdateShirt(Shirt shirt)
    {
        var shirtToUpdate = shirts.First(x => x.ShirtId == shirt.ShirtId);
        shirtToUpdate.Brand = shirt.Brand;
        shirtToUpdate.Price = shirt.Price;
        shirtToUpdate.Size = shirt.Size;
        shirtToUpdate.Color = shirt.Color;
        shirtToUpdate.Gender = shirt.Gender;
    }

    public static void DeleteShirt(int shirtId)
    {
        var shirt = GetShritById(shirtId);
        if (shirt is not null)
        {
            shirts.Remove(shirt);
        }
    }
}