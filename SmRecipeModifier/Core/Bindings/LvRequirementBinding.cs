namespace SmRecipeModifier.Core.Bindings
{

    public class LvRequirementBinding
    {

        public LvRequirementBinding(int quantity, string name, string id)
        {
            Quantity = quantity;
            Name = name;
            Id = id;
        }

        public int Quantity { get; }

        public string Name { get; }

        public string Id { get; }

    }

}