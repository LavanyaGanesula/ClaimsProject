using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaimsProjectFinal.Models
{

    public class FinalClaimsViewModel
    {
               
        public int OriginYear { get; set; }
        public int NoOfYears { get; set; }
        public List<FinalClaimsList> FinalClaimsList { get; set; }
       public List<ProductList> ProductList { get; set; }

    }

    public class FinalClaimsList
    {
        public string Product { get; set; }
    }

    public class ProductList
    {
        public string Product { get; set; }
        public int OriginYear { get; set; }
        public int DevelopmentYear { get; set; }
        public decimal IncrementalValue { get; set; }
    }
    public class ClaimsOriginYear
    {
        public int OriginYear { get; set; }
    }
    public class ClaimsNoOfYears
    {
        public int NoOfYears { get; set; }
    }
}
