using DALlab3.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace lab3.Test.UnitTests
{
    public class Person_Should
    {
        private Person person;

        public Person_Should()
        {
            person = new Person();
            person.FirstName = "Cristian";
            person.LastName = "Petculescu";
        }

        [Fact]
        public void SecretHashHasValue()
        {
            Assert.Equal("?\fH?X\f??t??RE\b??", person.SecretHash);
        }
    }
}