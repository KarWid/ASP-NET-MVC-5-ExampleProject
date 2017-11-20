using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Models;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            // przygotowanie - utworzenie imitacji dostawcy uwierzytelnienia
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "sekret")).Returns(true);

            // przygotowanie - utworzenie modelu widoku
            LoginViewModel model = new LoginViewModel
            {
                UserName = "admin",
                Password = "sekret"
            };

            // przygotowanie - utworzenie kontrolera
            AccountController target = new AccountController(mock.Object);

            // działanie - uwierzytelnianie z użyciem prawidłowych danych
            ActionResult result = target.Login(model, "/MyURL");

            // assercje
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials()
        {
            // przygotowanie - utworzenie imitacji dostawcy uwierzytelniania
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("nieprawidlowyUzytkownik", "nieprawidloweHaslo")).Returns(false);

            // przygotowanie - utworzenie modelu widoku
            LoginViewModel model = new LoginViewModel
            {
                UserName = "nieprawidlowyUzytkownik",
                Password = "nieprawidloweHaslo"
            };

            // przygotowanie - utworzenie kontrolera
            AccountController target = new AccountController(mock.Object);

            // działanie - uwierzytelnianie z użyciem nieprawidłowych danych
            ActionResult result = target.Login(model, "/MyURL");

            // asercje
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
