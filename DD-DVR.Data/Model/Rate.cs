using System;
using System.ComponentModel.DataAnnotations;

namespace DD_DVR.Data.Model
{
    [Serializable]
    public class Rate : BaseData
    {
        private decimal _price;

        [Display(Name = "Цена")]
        [Range(minimum: 0.0, maximum: 1000.00, ErrorMessage = "Введите значени в диапазоне от 0.00 до 1000.00")]
        public decimal Price { get => _price; set { _price = value; OnPropertyChanged("Text"); } }


        public string Text
        {
            get
            {
                return String.Format("{0:0.00}",Price)+" грн.";
            }
        }

       
    }
}
