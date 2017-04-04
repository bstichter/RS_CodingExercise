using System;
using System.Collections.Generic;
using Xunit;
using System.Text;
using FocusOnTheFamily.ReadyToWed.Metrics.DataModel;
using System.Linq;
using FocusOnTheFamily.ReadyToWed.Metrics.BusinessModel;
using Microsoft.EntityFrameworkCore;

namespace FocusOnTheFamily.ReadyToWed.Metrics.Test {
  public class MarketingDomainBusinessHandlerTests {
    public class GetUserMetricsMethod {
      public static IEnumerable<object[]> GoodUserData {
        get {
          return new[] {
            new object[] { (new User[] { new User { Gender = Gender.Male, Age = 20 }, new User { Gender = Gender.Male, Age = 30 } }).AsQueryable(), 2, 0, 25 },
            new object[] { (new User[] { new User { Gender = Gender.Male, Age = 50 }, new User { Gender = Gender.Female, Age = 30 } }).AsQueryable(), 1, 1, 40 },
            new object[] { (new User[] { new User { Gender = Gender.Female, Age = 40 }, new User { Gender = Gender.Female, Age = 50 } }).AsQueryable(), 0, 2, 45 },
          };
        }
      }

      [Theory]
      [MemberData(nameof(GoodUserData))]
      public void AnyData_Calculated(IQueryable<User> users, int NUMBER_OF_MALE_USERS, int NUMBER_OF_FEMALE_USERS, double AVERAGE_AGE_OF_USERS) {
        //Arrange:

        //TODO: This really should be Mocked
        //The in-memory database is shared across all instances of this context.
        //If tests run in parallel this could cause interferences.
        var contextOptions = new DbContextOptionsBuilder<ReadyToWedMetricsContext>();

        //The name parameter will avoid parallelization issues for now
        contextOptions.UseInMemoryDatabase("GetUserMetricsMethod");

        var context = new ReadyToWedMetricsContext(contextOptions.Options);

        context.Users.AddRange(users);
        context.SaveChanges();

        var businessHandler = new MarketingDomainBusinessHandler();
        businessHandler.Init(context);


        //Act:
        var userMetrics = businessHandler.GetUserMetrics();

        int calculatedNumberOfMaleUsers = (
          from x in userMetrics.NumberOfUsersByGender
          where x.Gender == Gender.Male
          select x.Count
        ).SingleOrDefault();

        int calculatedNumberOfFemaleUsers = (
          from x in userMetrics.NumberOfUsersByGender
          where x.Gender == Gender.Female
          select x.Count
        ).SingleOrDefault();

        double calculatedAverageAgeOfUsers = userMetrics.AverageAgeOfUsers;


        //Assert:
        Assert.Equal(NUMBER_OF_MALE_USERS, calculatedNumberOfMaleUsers);
        Assert.Equal(NUMBER_OF_FEMALE_USERS, calculatedNumberOfFemaleUsers);
        Assert.Equal(AVERAGE_AGE_OF_USERS, calculatedAverageAgeOfUsers);

        context.Database.EnsureDeleted(); //Cleanup
      }
    }

    public class GetDailyMetricsMethod {
      public static IEnumerable<object[]> GoodDailyNumbersData {
        get {
          return new[] {
            new object[] { (new DailyNumbers[] { new DailyNumbers { Installs = 10, Logins = 15 }, new DailyNumbers { Installs = 20, Logins = 25 } }).AsQueryable(), 15, 20 },
            new object[] { (new DailyNumbers[] { new DailyNumbers { Installs = 30, Logins = 40 }, new DailyNumbers { Installs = 36, Logins = 46 } }).AsQueryable(), 33, 43 },
            new object[] { (new DailyNumbers[] { new DailyNumbers { Installs = 50, Logins = 60 }, new DailyNumbers { Installs = 66, Logins = 56 } }).AsQueryable(), 58, 58 },
          };
        }
      }

      [Theory]
      [MemberData(nameof(GoodDailyNumbersData))]
      public void AnyData_Calculated(IQueryable<DailyNumbers> dailyNumbers, double AVERAGE_NUMBER_OF_INSTALLS, double AVERAGE_NUMBER_OF_LOGINS) {
        //Arrange:

        //TODO: This really should be Mocked
        //The in-memory database is shared across all instances of this context.
        //If tests run in parallel this could cause interferences.
        var contextOptions = new DbContextOptionsBuilder<ReadyToWedMetricsContext>();

        //The name parameter will avoid parallelization issues for now
        contextOptions.UseInMemoryDatabase("GetDailyMetricsMethod");

        var context = new ReadyToWedMetricsContext(contextOptions.Options);

        context.DailyNumbers.AddRange(dailyNumbers);
        context.SaveChanges();

        var businessHandler = new MarketingDomainBusinessHandler();
        businessHandler.Init(context);


        //Act:
        var dailyMetrics = businessHandler.GetDailyMetrics();

        double calculatedAverageNumberOfInstalls = dailyMetrics.AverageInstalls;
        double calculatedAverageNumberOfLogins = dailyMetrics.AverageLogins;

        //Left this out because it is intentionally broken and isn't used in the application
        //double calculatedAverageNumberOfCompletions = dailyMetrics.AverageCompletions; //Left this out 


        //Assert:
        Assert.Equal(AVERAGE_NUMBER_OF_INSTALLS, calculatedAverageNumberOfInstalls);
        Assert.Equal(AVERAGE_NUMBER_OF_LOGINS, calculatedAverageNumberOfLogins);

        context.Database.EnsureDeleted(); //Cleanup
      }
    }
  }
}
