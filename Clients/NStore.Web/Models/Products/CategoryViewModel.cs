namespace NStore.Web.Models.Products
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}
