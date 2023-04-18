using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using ReProServices.Application.AutoFill;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AutoFill
{
    public class FillForm26Q
    {
        private static void WaitForReady(IWebDriver webDriver)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(120);
            WebDriverWait wait = new WebDriverWait(webDriver, timeSpan);
            wait.Until(driver=>{
                bool isAjaxFinished = (bool)((IJavaScriptExecutor)driver).
                    ExecuteScript("return jQuery.active == 0");
                try
                {
                    var loader=driver.FindElement(By.ClassName("loader-mask")).GetAttribute("style");
                    Console.WriteLine(loader);
                    return loader.Split(':')[1] == " none;";        ;
                }
                catch
                {
                    return isAjaxFinished;
                }
            });
        }

        private static void WaitFor(IWebDriver webDriver,int inSeconds = 0) {
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(inSeconds);
        }

        public static void AutoFillForm26QB(AutoFillDto autoFillDto)
        {
            try
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-infobars");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--start-maximized");
                options.AddArgument("--remote-debugging-port=9222");

                ChromeDriver driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), options);

                driver.Navigate().GoToUrl("https://onlineservices.tin.egov-nsdl.com/etaxnew/tdsnontds.jsp");
                WaitForReady(driver);
                // var proceedBtn = driver.FindElement(By.XPath("//a[@href='javascript:sendRequest(\'PropertyTaxForm\');']"));
                driver.FindElement(By.XPath("//*[@id='selectform']/div[3]/div[1]/section/div/div/a")).Click(); //todo improve xpath
                //
                WaitForReady(driver);
                FillTaxPayerInfo(driver, autoFillDto.tab1);

                WaitForReady(driver);
                FillAddress(driver, autoFillDto.tab2);

                WaitForReady(driver);
                FillPropertyinfo(driver, autoFillDto.tab3);

                WaitForReady(driver);
                FillPaymentinfo(driver, autoFillDto.tab4);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        private static void FillTaxPayerInfo(IWebDriver webDriver,Tab1 tab1) {
            var taxApplicable = webDriver.FindElement(By.Id(tab1.TaxApplicable));
            taxApplicable.Click();

            var resident = "";
            if (tab1.StatusOfPayee)
                resident = "Indian";
            else
                resident = "NRI";
            var statusOfPayer = webDriver.FindElement(By.Id(resident));
            statusOfPayer.Click();

            var pan = webDriver.FindElement(By.Id("PAN_purchaser"));
            pan.SendKeys(tab1.PanOfPayer);
            var confirmPan = webDriver.FindElement(By.Id("ConfirmTransferee"));
            confirmPan.SendKeys(tab1.PanOfPayer);
            var sellerPan = webDriver.FindElement(By.Id("PAN_seller"));
            sellerPan.SendKeys(tab1.PanOfTransferor);
            var confirmSellerPan = webDriver.FindElement(By.Id("ConfirmTransferers"));
            confirmSellerPan.SendKeys(tab1.PanOfTransferor);
            WaitForReady(webDriver);

            webDriver.FindElement(By.XPath("//a[@href='#next']")).Click();
        }

        private static void FillAddress(IWebDriver webDriver,Tab2 tab2) {
            if (tab2.AddressPremisesOfTransferee != "" && tab2.AddressPremisesOfTransferee != null)
            {
                var address1 = webDriver.FindElement(By.Id("Add_Line2"));
                address1.SendKeys(tab2.AddressPremisesOfTransferee);
            }
            if (tab2.AdressLine1OfTransferee != "" && tab2.AdressLine1OfTransferee != null)
            {
                var flot = webDriver.FindElement(By.Name("Add_Line1"));
                flot.SendKeys(tab2.AdressLine1OfTransferee);
            }
            if (tab2.AddressLine2OfTransferee != "" && tab2.AddressLine2OfTransferee != null)
            {
                var road = webDriver.FindElement(By.Name("Add_Line3"));
                road.SendKeys(tab2.AddressLine2OfTransferee);
            }
            var city = webDriver.FindElement(By.Name("Add_Line5"));
            city.SendKeys(tab2.CityOfTransferee);
            var state = webDriver.FindElement(By.Name("Add_State"));
            var stateDDl = new SelectElement(state);
            stateDDl.SelectByText(tab2.StateOfTransferee);
            var pin = webDriver.FindElement(By.Name("Add_PIN"));
            pin.SendKeys(tab2.PinCodeOfTransferee);
            var email = webDriver.FindElement(By.Name("Add_EMAIL"));
            email.SendKeys(tab2.EmailOfOfTransferee);
            if (tab2.MobileOfOfTransferee != "" && tab2.MobileOfOfTransferee != null)
            {
                var mobile = webDriver.FindElement(By.Name("Add_MOBILE"));
                mobile.SendKeys(tab2.MobileOfOfTransferee);
            }
            var moreThanOeBuyer = "";
            if (tab2.IsCoTransferee)
                moreThanOeBuyer = "Yes";
            else moreThanOeBuyer = "No";
            var coBuyer = webDriver.FindElement(By.Name("Buyer"));
            var coBuyerDDl = new SelectElement(coBuyer);
            coBuyerDDl.SelectByText(moreThanOeBuyer);


            if (tab2.AddressPremisesOfTransferor != "" && tab2.AddressPremisesOfTransferor != null)
            {
                var address1Trans = webDriver.FindElement(By.Name("transferer_Add_Line2"));
                address1Trans.SendKeys(tab2.AddressPremisesOfTransferor);
            }
            if (tab2.AddressLine1OfTransferor != "" && tab2.AddressLine1OfTransferor != null)
            {
                var flotTrans = webDriver.FindElement(By.Name("transferer_Add_Line1"));
                flotTrans.SendKeys(tab2.AddressLine1OfTransferor);
            }
            if (tab2.AddressLine2OfTransferor != "" && tab2.AddressLine2OfTransferor != null)
            {
                var roadTrans = webDriver.FindElement(By.Name("transferer_Add_Line3"));
                roadTrans.SendKeys(tab2.AddressLine2OfTransferor);
            }
            var cityTrans = webDriver.FindElement(By.Name("transferer_Add_Line5"));
            cityTrans.SendKeys(tab2.CityOfTransferor);
            var stateTrans = webDriver.FindElement(By.Name("transferer_Add_State"));
            var stateDDlTrans = new SelectElement(stateTrans);
            stateDDlTrans.SelectByText(tab2.StateOfTransferor);
            var pinTrans = webDriver.FindElement(By.Name("transferer_Add_PIN"));
            pinTrans.SendKeys(tab2.PinCodeOfTransferor);
            if (tab2.EmailOfOfTransferor != "" && tab2.EmailOfOfTransferor != null)
            {
                var emailTrans = webDriver.FindElement(By.Name("transferer_Add_EMAIL"));
                emailTrans.SendKeys(tab2.EmailOfOfTransferor);
            }
            if (tab2.MobileOfOfTransferor != "" && tab2.MobileOfOfTransferor != null)
            {
                var mobiletrans = webDriver.FindElement(By.Name("transferer_Add_MOBILE"));
                mobiletrans.SendKeys(tab2.MobileOfOfTransferor);
            }

            var sellerOpt = "";
            if (tab2.IsCoTransferor)
                sellerOpt = "Yes";
            else sellerOpt = "No";

            var Seller = webDriver.FindElement(By.Name("Seller"));
            var SellerDDl= new SelectElement(Seller);
            SellerDDl.SelectByText(sellerOpt);

            webDriver.FindElement(By.XPath("//a[@href='#next']")).Click();
        }

        private static void FillPropertyinfo(IWebDriver webDriver,Tab3 tab3)
        {
            var propType = "";
            if (tab3.TypeOfProperty == "1")
                propType = "Land";
            else
                propType = "Building";
            var property = webDriver.FindElement(By.Name("propertyType"));
            var propertyDDl = new SelectElement(property);
            propertyDDl.SelectByValue(propType);

            if (tab3.AddressPremisesOfProperty != "" && tab3.AddressPremisesOfProperty != null)
            {
                var address1 = webDriver.FindElement(By.Name("p_Add_Line2"));
                address1.SendKeys(tab3.AddressPremisesOfProperty);
            }
            if (tab3.AddressLine1OfProperty != "" && tab3.AddressLine1OfProperty != null)
            {
                var flot = webDriver.FindElement(By.Name("p_Add_Line1"));// Note : this should be hide  pased on property type
                flot.SendKeys(tab3.AddressLine1OfProperty);
            }
            if (tab3.AddressLine2OfProperty != "" && tab3.AddressLine2OfProperty != null)
            {
                var road = webDriver.FindElement(By.Name("p_Add_Line3"));
                road.SendKeys(tab3.AddressLine2OfProperty);
            }
            var city = webDriver.FindElement(By.Name("p_Add_Line5"));
            city.SendKeys(tab3.CityOfProperty);
            var state = webDriver.FindElement(By.Name("p_Add_State"));
            var stateDDl = new SelectElement(state);
            stateDDl.SelectByText(tab3.StateOfProperty);
            var pin = webDriver.FindElement(By.Name("p_Add_PIN"));
            pin.SendKeys(tab3.PinCodeOfProperty);

            var day = webDriver.FindElement(By.Name("agmt_day"));
            day.Click();
            var dayDDl = new SelectElement(day);
            var opts = dayDDl.Options;
            var daysOpt = dayDDl.Options.Where(x => x.Text.Trim() == tab3.DateOfAgreement.Day.ToString()).FirstOrDefault();
            dayDDl.SelectByText(daysOpt.Text);

            var month = webDriver.FindElement(By.Name("agmt_month"));
            var monthDDl = new SelectElement(month);
            var monthOpt = monthDDl.Options.Where(x => x.Text.Trim().ToLower() == tab3.DateOfAgreement.Month.ToLower()).FirstOrDefault();
            monthDDl.SelectByText(monthOpt.Text);

            var year = webDriver.FindElement(By.Name("agmt_year"));          
            var yearDDl = new SelectElement(year);
            var yearOpt = yearDDl.Options.Where(x => x.Text.Trim() == tab3.DateOfAgreement.Year.ToString()).FirstOrDefault();
            yearDDl.SelectByText(yearOpt.Text);

            var totalValue = webDriver.FindElement(By.Name("totalPropertyValue"));
            totalValue.SendKeys(tab3.TotalAmount.ToString());

            var paymentType = webDriver.FindElement(By.Name("paymentType"));
            var paymentTypeDDl = new SelectElement(paymentType);
            paymentTypeDDl.SelectByIndex(1);

            // AssignAmount(webDriver, "111111111");
            var ones = webDriver.FindElement(By.Name("Ones"));
            var onesDDl = new SelectElement(ones);
            onesDDl.SelectByText(tab3.AmountPaidParts.Ones.ToString());

            var ten = webDriver.FindElement(By.Name("Tens"));
            var tenDDl = new SelectElement(ten);
            tenDDl.SelectByText(tab3.AmountPaidParts.Tens.ToString());

            var hundreds = webDriver.FindElement(By.Name("Hundreds"));
            var hundredsDDl = new SelectElement(hundreds);
            hundredsDDl.SelectByText(tab3.AmountPaidParts.Hundreds.ToString());

            var thousands = webDriver.FindElement(By.Name("Thousands"));
            var thousandsDDl = new SelectElement(thousands);
            thousandsDDl.SelectByText(tab3.AmountPaidParts.Thousands.ToString());

            var lakh = webDriver.FindElement(By.Name("Lakh"));
            var lakhDDl = new SelectElement(lakh);
            lakhDDl.SelectByText(tab3.AmountPaidParts.Lakhs.ToString());

            var crores = webDriver.FindElement(By.Name("Crores"));
            var croresDDl = new SelectElement(crores);
            croresDDl.SelectByText(tab3.AmountPaidParts.Crores.ToString());

            var totalpPaid = webDriver.FindElement(By.Name("value_entered_user"));
            totalpPaid.SendKeys(tab3.AmountPaid.ToString());

            var tdsAmount = webDriver.FindElement(By.Name("TDS_amt"));
            tdsAmount.SendKeys(tab3.BasicTax.ToString());
            if (tab3.Interest != 0 )
            {
                var interest = webDriver.FindElement(By.Name("interest"));
                interest.SendKeys(tab3.Interest.ToString());
            }
            if (tab3.LateFee != 0)
            {
                var fee = webDriver.FindElement(By.Name("fee"));
                fee.SendKeys(tab3.LateFee.ToString());
            }
            webDriver.FindElement(By.XPath("//a[@href='#next']")).Click();
        }

        private static void FillPaymentinfo(IWebDriver webDriver, Tab4 tab4)
        {
            var modePofPay = "";
            if (tab4.ModeOfPayment == "modeBankSelection")
                modePofPay = "onlineRadio";
            else
                modePofPay = "offlineRadio";
            var address1 = webDriver.FindElement(By.Id(modePofPay));
            address1.Click();
            if (modePofPay == "onlineRadio") {
                var bank = webDriver.FindElement(By.Id("NetBank_Name_c"));
                var bankDDl = new SelectElement(bank);
                bankDDl.SelectByText("ICICI Bank");
            }

            var day = webDriver.FindElement(By.Name("pymntDay"));
            day.Click();
            var dayDDl = new SelectElement(day);
            dayDDl.SelectByText(tab4.DateOfPayment.Day.ToString());
            var month = webDriver.FindElement(By.Name("pymntMonth"));
            var monthDDl = new SelectElement(month);
            monthDDl.SelectByText(tab4.DateOfPayment.Month.ToString());
            var year = webDriver.FindElement(By.Name("pymntYear"));
            var yearDDl = new SelectElement(year);
            yearDDl.SelectByText(tab4.DateOfPayment.Year.ToString());

            var dayDeduction = webDriver.FindElement(By.Name("deductionDay"));
            dayDeduction.Click();
            var dayDeductionDDl = new SelectElement(dayDeduction);
            dayDeductionDDl.SelectByText(tab4.DateOfTaxDeduction.Day.ToString());
            var monthDeduction = webDriver.FindElement(By.Name("deductionMonth"));
            var monthDeductionDDl = new SelectElement(monthDeduction);
            monthDeductionDDl.SelectByText(tab4.DateOfTaxDeduction.Month.ToString());
            var yearDeduction = webDriver.FindElement(By.Name("deductionYear"));
            var yearDeductionDDl = new SelectElement(yearDeduction);
            yearDeductionDDl.SelectByText(tab4.DateOfTaxDeduction.Year.ToString());

        }

        private static void AssignAmount(IWebDriver webDriver, string amount) {

            if (amount.Length == 1)
            {
                var ones = webDriver.FindElement(By.Name("Ones"));
                var onesDDl = new SelectElement(ones);
                onesDDl.SelectByText(amount);
            }
            else if (amount.Length == 2) {
                var ones = webDriver.FindElement(By.Name("Ones"));
                var onesDDl = new SelectElement(ones);
                onesDDl.SelectByText(amount.Substring(1,1));

                var ten = webDriver.FindElement(By.Name("Tens"));
                var tenDDl = new SelectElement(ten);
                tenDDl.SelectByText(amount.Substring(0, 1));
            }
            else if (amount.Length == 3)
            {
                var ones = webDriver.FindElement(By.Name("Ones"));
                var onesDDl = new SelectElement(ones);
                onesDDl.SelectByText(amount.Substring(2, 1));

                var ten = webDriver.FindElement(By.Name("Tens"));
                var tenDDl = new SelectElement(ten);
                tenDDl.SelectByText(amount.Substring(1, 1));

                var hundreds = webDriver.FindElement(By.Name("Hundreds"));
                var hundredsDDl = new SelectElement(hundreds);
                hundredsDDl.SelectByText(amount.Substring(0, 1));
            }
            else if (amount.Length >3 && amount.Length <6)
            {
                var ones = webDriver.FindElement(By.Name("Ones"));
                var onesDDl = new SelectElement(ones);
                onesDDl.SelectByText(amount.Substring(3, 1));

                var ten = webDriver.FindElement(By.Name("Tens"));
                var tenDDl = new SelectElement(ten);
                tenDDl.SelectByText(amount.Substring(2, 1));

                var hundreds = webDriver.FindElement(By.Name("Hundreds"));
                var hundredsDDl = new SelectElement(hundreds);
                hundredsDDl.SelectByText(amount.Substring(1, 1));

                var lngth = amount.Length == 4 ? 1 : 2;
                var thousands = webDriver.FindElement(By.Name("Thousands"));
                var thousandsDDl = new SelectElement(thousands);
                thousandsDDl.SelectByText(amount.Substring(0, lngth));

            }
            else if (amount.Length > 5 && amount.Length < 8)
            {
                var ones = webDriver.FindElement(By.Name("Ones"));
                var onesDDl = new SelectElement(ones);
                onesDDl.SelectByText(amount.Substring(4, 1));

                var ten = webDriver.FindElement(By.Name("Tens"));
                var tenDDl = new SelectElement(ten);
                tenDDl.SelectByText(amount.Substring(3, 1));

                var hundreds = webDriver.FindElement(By.Name("Hundreds"));
                var hundredsDDl = new SelectElement(hundreds);
                hundredsDDl.SelectByText(amount.Substring(2, 1));

               
                var thousands = webDriver.FindElement(By.Name("Thousands"));
                var thousandsDDl = new SelectElement(thousands);
                thousandsDDl.SelectByText(amount.Substring(1, 2));

                var lngth = amount.Length == 6 ? 1 : 2;
                var lakh = webDriver.FindElement(By.Name("Lakh"));
                var lakhDDl = new SelectElement(lakh);
                lakhDDl.SelectByText(amount.Substring(0, lngth));
            }

            else if (amount.Length > 7)
            {
                var ones = webDriver.FindElement(By.Name("Ones"));
                var onesDDl = new SelectElement(ones);
                onesDDl.SelectByText(amount.Substring(amount.Length-(amount.Length-1), 1));

                var ten = webDriver.FindElement(By.Name("Tens"));
                var tenDDl = new SelectElement(ten);
                tenDDl.SelectByText(amount.Substring(amount.Length - (amount.Length - 2), 1));

                var hundreds = webDriver.FindElement(By.Name("Hundreds"));
                var hundredsDDl = new SelectElement(hundreds);
                hundredsDDl.SelectByText(amount.Substring(amount.Length - (amount.Length - 3), 1));


                var thousands = webDriver.FindElement(By.Name("Thousands"));
                var thousandsDDl = new SelectElement(thousands);
                thousandsDDl.SelectByText(amount.Substring(amount.Length - (amount.Length - 5), 2));

               
                var lakh = webDriver.FindElement(By.Name("Lakh"));
                var lakhDDl = new SelectElement(lakh);
                lakhDDl.SelectByText(amount.Substring(amount.Length - 7, 2));

                var lngth = amount.Length -7;
                var crores = webDriver.FindElement(By.Name("Crores"));
                var croresDDl = new SelectElement(crores);
                croresDDl.SelectByText(amount.Substring(0, lngth));
            } 
            
        }
    }
}
