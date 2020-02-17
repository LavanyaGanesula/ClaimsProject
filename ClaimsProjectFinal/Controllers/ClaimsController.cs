using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClaimsProjectFinal.Services;
using ClaimsProjectFinal.Models;
using System.Configuration;

namespace ClaimsProjectFinal.Controllers
{
    public class ClaimsController : Controller
    {        
        private readonly IClaimsService _claimsService;        

        public ClaimsController()
        {
            
        }
        public ClaimsController(IClaimsService claimsService)
        {
            _claimsService = claimsService;            
        }

        public ActionResult Index()
        {   
            try
            {
                string inputClaimsFile = Server.MapPath(ConfigurationManager.AppSettings["InputClaimsFile"].ToString());
                var startLine = Convert.ToInt32(ConfigurationManager.AppSettings["StartLine"].ToString());
                var lineCount = Convert.ToInt32(ConfigurationManager.AppSettings["LineCount"].ToString());

                ClaimsService _claimsService = new ClaimsService();
                var productList = _claimsService.ReadFilesFromCSV(inputClaimsFile, startLine, lineCount);

                var model = new FinalClaimsViewModel
                {
                    OriginYear = _claimsService.CalculateInitialYear(productList),
                    NoOfYears = _claimsService.CalcualteNoOfYears(productList),
                    ProductList = productList,
                    FinalClaimsList = _claimsService.ProcessClaims(productList)
                };
                return View(model);
            }
            catch(Exception ex)
            {
                //Log Exception into the LogFile
                

            }
            return null;            
        }
        
    }
}