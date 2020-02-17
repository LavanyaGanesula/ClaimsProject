using ClaimsProjectFinal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClaimsProjectFinal.Services
{
    public interface IClaimsService
    {
        /*get*/
        List<ProductList> ReadFilesFromCSV(string inputClaimsFile, int startLine, int lineCount);
        List<FinalClaimsList> ProcessClaims(List<ProductList> claimsList);
        FinalClaimsList BuildFinalClaimString(string productName, List<ProductList> claimsList, ClaimsOriginYear initialYear, ClaimsNoOfYears noOfYears, int finalYear);
        int CalculateInitialYear(List<ProductList> claimsList);
        int CalcualteNoOfYears(List<ProductList> claimsList);
        int CalculateFinalYear(List<ProductList> claimsList);        
    }

    public class ClaimsService : IClaimsService
    {        
        public ClaimsService()
        {
            
        }
     
        public List<ProductList> ReadFilesFromCSV(string inputClaimsFile, int startLine, int lineCount)
        {
            var fileLines = File.ReadAllLines(inputClaimsFile).Skip((startLine)).Take(lineCount).ToList();
            List<ProductList> lst = new List<ProductList>();
            foreach (string line in fileLines)
            {
                string[] temp = line.Split(',');
                try
                {
                    if (temp.Length >= 3)
                    {
                        ProductList Product = new ProductList();
                        Product.Product = temp[0].Trim();
                        Product.OriginYear = Convert.ToInt32(temp[1].Trim());
                        Product.DevelopmentYear = Convert.ToInt32(temp[2].Trim());
                        Product.IncrementalValue = Convert.ToDecimal(temp[3].Trim());

                        lst.Add(Product);
                    }
                }
                catch
                {
                    Console.WriteLine("Incorrect Format" + line);
                }
            }
            return lst;
        }

        public List<FinalClaimsList> ProcessClaims(List<ProductList> claimsList)
        {
            List<FinalClaimsList> finalClaimsList = new List<FinalClaimsList>();
            List<ProductList> tempClaimsList = new List<ProductList>();

            ClaimsOriginYear claimsOriginYear = new ClaimsOriginYear();
            claimsOriginYear.OriginYear = CalculateInitialYear(claimsList);

            ClaimsNoOfYears claimsNoOfYears = new ClaimsNoOfYears();
            claimsNoOfYears.NoOfYears = CalcualteNoOfYears(claimsList);

            int finalYear = CalculateFinalYear(claimsList);            

            var distinctProductList = claimsList.Select(x => x.Product).Distinct();

            foreach (var productName in distinctProductList)
            {
                tempClaimsList = claimsList.FindAll(x => x.Product == productName);
                var finalclaim = BuildFinalClaimString(productName, tempClaimsList, claimsOriginYear, claimsNoOfYears, finalYear);
                finalClaimsList.Add(finalclaim);

            }
            return finalClaimsList;
        }

        public FinalClaimsList BuildFinalClaimString(string productName, List<ProductList> claimsList, ClaimsOriginYear initialYear, ClaimsNoOfYears noOfYears, int finalYear)
        {
            FinalClaimsList finalCalimSting = new FinalClaimsList();
            string strNoValue = "0";
            finalCalimSting.Product = productName.ToString();
            decimal runningTotal = 0;
            int tempInitialYear = initialYear.OriginYear;
            int tempDevelopmentYear = 0;

            while (tempInitialYear <= finalYear)
            {
                var tempClaimsList = claimsList.FindAll(x => x.OriginYear == tempInitialYear);
                if (tempClaimsList.Count > 0)
                {
                    runningTotal = 0;
                    tempDevelopmentYear = tempInitialYear;
                    foreach (var temp in tempClaimsList)
                    {
                        var tempdevlopmentList = claimsList.FindAll(x => (x.DevelopmentYear == tempDevelopmentYear) && (x.OriginYear == tempInitialYear));
                        if (tempdevlopmentList.Count > 0)
                        {
                            runningTotal = runningTotal + tempdevlopmentList[0].IncrementalValue;
                            finalCalimSting.Product = finalCalimSting.Product + ",  " + runningTotal;
                        }
                        else
                        {
                            finalCalimSting.Product = finalCalimSting.Product + ",  " + runningTotal;
                        }
                        tempDevelopmentYear = tempDevelopmentYear + 1;
                    }
                }
                else
                {
                    int temp2intialYear = tempInitialYear;
                    while (temp2intialYear <= finalYear)
                    {
                        finalCalimSting.Product = finalCalimSting.Product + ",  " + strNoValue;
                        temp2intialYear = temp2intialYear + 1;
                    }

                }
                tempInitialYear = tempInitialYear + 1;
            }

            return finalCalimSting;
        }
        public int CalculateInitialYear(List<ProductList> claimsList)
        {
            int intial_Year = claimsList.Min(x => x.OriginYear);
            return intial_Year;
        }
        public int CalcualteNoOfYears(List<ProductList> claimsList)
        {
            var no_Of_Years = claimsList.Select(x => x.OriginYear).Distinct();
            return Convert.ToInt32(no_Of_Years.Count());
        }
        public int CalculateFinalYear(List<ProductList> claimsList)
        {
            int final_Year = claimsList.Max(x => x.OriginYear);
            return final_Year;
        }
    }
}
