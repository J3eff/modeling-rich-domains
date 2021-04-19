using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests
{
    [TestClass]
    public class DocumetTests
    {
        //[TestMethod]
        public void ShoulReturnErrorWhenCNPJIsInvalid()
        {
            var doc = new Document("123", EDocumentType.CNPJ);
            //Assert.IsTrue(doc.Invalid);
        }

        [TestMethod]
        public void ShoulReturnSucessWhenCNPJIsValid()
        {
            var doc = new Document("05939279000197", EDocumentType.CNPJ);
            Assert.IsTrue(doc.IsValid);
        }

        //[TestMethod]
        public void ShoulReturnErrorWhenCPFIsInvalid()
        {
            var doc = new Document("123", EDocumentType.CPF);
            //Assert.IsTrue(doc.Invalid);
            
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow("47136322851")]
        [DataRow("44136322851")]
        [DataRow("00011133322")]
        public void ShoulReturnSucessWhenCPFJIsValid(string cpf)
        {
            var doc = new Document(cpf, EDocumentType.CPF);
            Assert.IsTrue(doc.IsValid);
        }
    }
}
