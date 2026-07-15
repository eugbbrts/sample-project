using Common.Exceptions;
using System;

namespace BusinessEntities
{
    public class OrderItem : IdObject
    {
        private Product _product;
        private decimal _unitPrice;
        private int _quantity;

        public Product Product
        {
            get => _product;
            private set => _product = value;
        }

        public decimal UnitPrice
        {
            get => _unitPrice;
            private set => _unitPrice = value;
        }

        public int Quantity
        {
            get => _quantity;
            private set => _quantity = value;
        }

        public decimal TotalPrice => Quantity * UnitPrice;

        public void SetProductAndQuantity(Product product, int quantity)
        {
            SetProduct(product);
            SetQuantity(quantity);
            SetUnitPrice(product.Price);
        }

        public void SetQuantity(int quantity)
        {
            if (quantity < 0)
            {
                throw new ValidationException("Stock Quantity cannot be negative value.");
            }

            _quantity = quantity;
        }

        private void SetUnitPrice(decimal unitPrice)
        {
            if (unitPrice < 0)
            {
                throw new ValidationException("Unit Price cannot be negative value.");
            }

            _unitPrice = unitPrice;
        }

        private void SetProduct(Product product)
        {
            if (product == null)
            {
                throw new ValidationException("Product cannot be null.");
            }

            _product = product;
        }
    }
}
