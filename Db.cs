namespace PizzaStore.DB;

public record Pizza
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class PizzaDB
{
    private static List<Pizza> _pizzas = new List<Pizza>()
    {
        new Pizza{ Id=1, Name="Montemagno, Pizza shaped like a great mountain" },
     new Pizza{ Id=2, Name="The Galloway, Pizza shaped like a submarine, silent but deadly"},
     new Pizza{ Id=3, Name="The Noring, Pizza shaped like a Viking helmet, where's the mead"}
    };

        public static Pizza GetPizza(int id) { 
        return _pizzas?.FirstOrDefault(p => p.Id == id);
     }

   public static List<Pizza> GetPizzas() 
   {
     return _pizzas;
   } 
    public static Pizza CreatePizza(Pizza pizza)
    {

        _pizzas.Add(pizza);
        return pizza;
    }
    public static Pizza EditPizza(Pizza updatePizza)
    {
        _pizzas = _pizzas.Select(p =>
        {
            if(p.Id == updatePizza.Id)
            {
                p.Name = updatePizza.Name;
            }
            return p;
        }).ToList();

        return updatePizza;
    }

    public static void RemovePizza(int id) { 
        _pizzas = _pizzas.FindAll(p => p.Id != id).ToList();
     }


}