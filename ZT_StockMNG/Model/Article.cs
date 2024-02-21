using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZT_StockMNG.Model
{
    public class Article
    {
        [Required(ErrorMessage ="Codice articolo obbligatorio!")]
        public string ArticleCode { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Quantità obbligatoria!")]
        public decimal Qty { get; set; }
    }
}
