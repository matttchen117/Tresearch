using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using Xunit;

namespace TrialByFire.Tresearch.Tests.IntegrationTests.DAL
{
    public class SQLDAOShould : IntegrationTestDependencies
    {

        public SQLDAOShould () : base()
        {

        }

        public static IEnumerable<object[]> GetMethodData()
        {
            get
            {
                IntegrationTestDependencies test = new IntegrationTestDependencies ();

            // Nodes Created
            yield return new object[] {
                new DateTime(2000, 1, 31),
                new Func<DateTime, List<NodesCreated>> (test.sqlDAO.GetNodesCreated),
                31
            };

            // Top Searches
            yield return new object[] {
                new DateTime(2000, 1, 31),
                new Func<DateTime, List<TopSearch>> (test.sqlDAO.GetTopSearch),
                31
            };

            // Daily Logins
            yield return new object[] {
                new DateTime(2000, 1, 31),
                new Func<DateTime, List<DailyLogin>> (test.sqlDAO.GetDailyLogin),
                31
            };

            // Daily Registrations
            yield return new object[] {
                new DateTime(2000, 1, 31),
                new Func<DateTime, List<DailyRegistration>> (test.sqlDAO.GetDailyRegistration),
                31
            };
        }

        public static IEnumerable<object[]> CreateMethodData()
        {
            get
            {
            IntegrationTestDependencies test = new IntegrationTestDependencies();

            // Nodes Created
            yield return new object[] {
                new NodesCreated(new DateTime(2000, 2, 2), 1),
                new Func<INodesCreated, string> (test.sqlDAO.CreateNodesCreated),
                test.messageBank.SuccessMessages["generic"]
            };

            yield return new object[] {
                new NodesCreated(new DateTime(2000, 1, 1), 1),
                new Func<INodesCreated, string> (test.sqlDAO.CreateNodesCreated),
                test.messageBank.ErrorMessages["createdNodesExists"]
            };

            // Top Searches
            yield return new object[] {
                new TopSearch(new DateTime(2000, 2, 2), "test1", 1),
                new Func<ITopSearch, string> (test.sqlDAO.CreateTopSearch),
                test.messageBank.SuccessMessages["generic"]
            };

            yield return new object[] {
                new TopSearch(new DateTime(2000, 1, 1), "test0", 1),
                new Func<ITopSearch, string> (test.sqlDAO.CreateTopSearch),
                test.messageBank.ErrorMessages["topSearchesExists"]
            };

            // Daily Login
            yield return new object[] {
                new DailyLogin(new DateTime(2000, 2, 2), 1),
                new Func<IDailyLogin, string> (test.sqlDAO.CreateDailyLogin),
                test.messageBank.SuccessMessages["generic"]
            };

            yield return new object[] {
                new DailyLogin(new DateTime(2000, 1, 1), 1),
                new Func<IDailyLogin, string> (test.sqlDAO.CreateDailyLogin),
                test.messageBank.ErrorMessages["dailyLoginsExists"]
            };

            // Daily Registration
            yield return new object[] {
                new DailyRegistration(new DateTime(2000, 2, 2), 1),
                new Func<IDailyRegistration, string> (test.sqlDAO.CreateDailyRegistration),
                test.messageBank.SuccessMessages["generic"]
            };

            yield return new object[] {
                new DailyRegistration(new DateTime(2000, 1, 1), 1),
                new Func<IDailyRegistration, string> (test.sqlDAO.CreateDailyRegistration),
                test.messageBank.ErrorMessages["dailyRegistrationsExists"]
            };
        }

        public static IEnumerable<object[]> UpdateMethodData()
        {
            IntegrationTestDependencies test = new IntegrationTestDependencies();

            // Nodes Created
            yield return new object[] {
                new NodesCreated(new DateTime(2000, 1, 1), 2),
                new Func<INodesCreated, string> (test.sqlDAO.UpdateNodesCreated),
                test.messageBank.SuccessMessages["generic"]
            };

            yield return new object[] {
                new NodesCreated(new DateTime(2000, 2, 2), 1),
                new Func<INodesCreated, string> (test.sqlDAO.UpdateNodesCreated),
                test.messageBank.ErrorMessages["createdNodesNotExists"]
            };

            // Top Searches
            yield return new object[] {
                new TopSearch(new DateTime(2000, 1, 1), "test1", 2),
                new Func<ITopSearch, string> (test.sqlDAO.UpdateTopSearch),
                test.messageBank.SuccessMessages["generic"]
            };

            yield return new object[] {
                new TopSearch(new DateTime(2000, 2, 2), "test0", 1),
                new Func<ITopSearch, string> (test.sqlDAO.UpdateTopSearch),
                test.messageBank.ErrorMessages["topSearchesNotExists"]
            };

            // Daily Login
            yield return new object[] {
                new DailyLogin(new DateTime(2000, 1, 1), 2),
                new Func<IDailyLogin, string> (test.sqlDAO.UpdateDailyLogin),
                test.messageBank.SuccessMessages["generic"]
            };

            yield return new object[] {
                new DailyLogin(new DateTime(2000, 2, 2), 1),
                new Func<IDailyLogin, string> (test.sqlDAO.UpdateDailyLogin),
                test.messageBank.ErrorMessages["dailyLoginsNotExists"]
            };

            // Daily Registration
            yield return new object[] {
                new DailyRegistration(new DateTime(2000, 1, 1), 2),
                new Func<IDailyRegistration, string> (test.sqlDAO.UpdateDailyRegistration),
                test.messageBank.SuccessMessages["generic"]
            };

            yield return new object[] {
                new DailyRegistration(new DateTime(2000, 2, 2), 1),
                new Func<IDailyRegistration, string> (test.sqlDAO.UpdateDailyRegistration),
                test.messageBank.ErrorMessages["dailyRegistrationsNotExists"]
            };
        }

        [Theory]
        [MemberData(nameof(CreateMethodData))]
        public void CreatedMethodsWork<T>(T kpiObject, Func<T, string> createFunc, string expected) where T : new()
        {
            // Act
            string result = createFunc(kpiObject);

            // Assert
            Assert.Equal(expected, result);
        }


        [Theory]
        [MemberData(nameof(GetMethodData))]
        public void GetMethodsWork<T>(DateTime date, Func<DateTime, List<T>> getFunc, int expected)
        {
            // Act
            List<T> kpiList = getFunc.Invoke(date);

            // Assert
            Assert.Equal(expected, kpiList.Count);
        }

        [Theory]
        [MemberData(nameof(UpdateMethodData))]
        public void UpdateMethodsWork<T>(T kpiObject, Func<T, string> updateFunc, string expected)
        {
            // Act
            string result = updateFunc.Invoke(kpiObject);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
