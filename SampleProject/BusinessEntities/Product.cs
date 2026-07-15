using Common.Exceptions;

namespace BusinessEntities
{
    public class Product : IdNameObject
    {
        private string _name;
        private string _category;
        private string _sku;
        private decimal _price;

        public string Category
        {
            get => _category;
            private set => _category = value;
        }        

        public string Sku
        {
            get => _sku;
            private set => _sku = value;
        }

        public decimal Price
        {
            get => _price;
            private set => _price = value;
        }

        public void SetCategory(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                throw new ValidationException("category was not provided.");
            }
            _category = category;
        }

        public void SetPrice(decimal price)
        {
            if (price < 0)
            {
                throw new ValidationException("Price cannot be negative value.");
            }
            _price = price;
        }

        public void SetSku(string sku)
        {
            if (string.IsNullOrEmpty(sku))
            {
                throw new ValidationException("SKU was not provided.");
            }
            _sku = sku.Trim();
        }
    }
}
