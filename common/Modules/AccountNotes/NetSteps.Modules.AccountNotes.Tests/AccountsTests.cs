using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.AccountNotes.Common;

namespace NetSteps.Modules.AccountNotes.Tests
{
    [TestClass]
    public class AccountNotesTests
    {
        private TestContext testContextInstance;

        public AccountNotesTests() { }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        private INote CreateTestAccountNote(int accountID, int noteID)
        {
            var accountNote = Create.Mutation(Create.New<INote>(), it =>
            {
                it.AccountID = accountID;
                it.NoteID = noteID;
            });

            return accountNote;
        }

        [TestMethod]
        public void TestLoadAccountNotes()
        {
            // Arrange
            int count = 3;
            int accountID = 1000;

            IList<INote> accountNotes = new List<INote>();

            var repository = new Mock<IAccountNotesRepositoryAdapter>();

            for (int i = 0; i < count; i++)
            {
                Random random = new Random();
                int noteID = random.Next(1, 10000);
                accountNotes.Add(CreateTestAccountNote(accountID, noteID));
            }

            repository.Setup(x => x.LoadAccountNotes(accountID)).Returns(accountNotes.AsEnumerable());

            var testAccountNote = new DefaultAccount(repository.Object);

            // Act
            var result = testAccountNote.LoadAccountNotes(accountID);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<INote>));
            Assert.AreEqual(result.Count(), count);
        }

        [TestMethod]
        public void TestCreateAccountNote()
        {
            // Arrange
            int accountID = 1000;
            Random random = new Random();
            int noteID = random.Next(1, 10000);
            INote accountNote = CreateTestAccountNote(accountID, noteID);
            string subject = "Test Subject";
            string noteText = "Test Note";

            var createNoteResult = new Mock<ICreateAccountNoteResult>();
            createNoteResult.Setup(x => x.Success).Returns(true);
            createNoteResult.Setup(x => x.AccountID).Returns(accountID);
            createNoteResult.Setup(x => x.NoteID).Returns(noteID);

            //Act
            var repository = new Mock<IAccountNotesRepositoryAdapter>();
            repository.Setup<ICreateAccountNoteResult>(x => x.CreateAccountNote(accountID, subject, noteText, 1, null, false)).Returns(createNoteResult.Object);
            var testAccountNote = new DefaultAccount(repository.Object);
            var result = testAccountNote.CreateAccountNote(accountID, subject, noteText);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ICreateAccountNoteResult));
            Assert.AreEqual(result.AccountID, accountID);
            Assert.AreEqual(result.NoteID, noteID);
            Assert.AreEqual(result.Success, true);
        }

        [TestMethod]
        public void TestCreateInternalAccountNote()
        {
            // Arrange
            int accountID = 1000;
            Random random = new Random();
            int noteID = random.Next(1, 10000);
            INote accountNote = CreateTestAccountNote(accountID, noteID);
            string subject = "Test Subject";
            string noteText = "Test Note";

            var createNoteResult = new Mock<ICreateAccountNoteResult>();
            createNoteResult.Setup(x => x.Success).Returns(true);
            createNoteResult.Setup(x => x.AccountID).Returns(accountID);
            createNoteResult.Setup(x => x.NoteID).Returns(noteID);

            //Act
            var repository = new Mock<IAccountNotesRepositoryAdapter>();
            repository.Setup<ICreateAccountNoteResult>(x => x.CreateAccountNote(accountID, subject, noteText, 1, null, true)).Returns(createNoteResult.Object);
            var testAccountNote = new DefaultAccount(repository.Object);
            var result = testAccountNote.CreateInternalAccountNote(accountID, subject, noteText);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ICreateAccountNoteResult));
            Assert.AreEqual(result.AccountID, accountID);
            Assert.AreEqual(result.NoteID, noteID);
            Assert.AreEqual(result.Success, true);
        }
    }
}
