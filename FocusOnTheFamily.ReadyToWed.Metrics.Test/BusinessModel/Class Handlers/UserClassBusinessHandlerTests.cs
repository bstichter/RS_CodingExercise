using System.Linq;
using System.Collections.Generic;

using Xunit;

using FocusOnTheFamily.ReadyToWed.Metrics.DataModel;
using FocusOnTheFamily.ReadyToWed.Metrics.BusinessModel;

namespace FocusOnTheFamily.ReadyToWed.Metrics.Test {
  public class UserClassBusinessHandlerTests {
    public class GetNumberOfUsersByGenderMethod {
      public static IEnumerable<object[]> GoodUserData {
        get {
          return new[] {
            new object[] { (new User[] { new User { Gender = Gender.Male }, new User { Gender = Gender.Male } }).AsQueryable(), 2, 0 },
            new object[] { (new User[] { new User { Gender = Gender.Male }, new User { Gender = Gender.Female } }).AsQueryable(), 1, 1 },
            new object[] { (new User[] { new User { Gender = Gender.Female }, new User { Gender = Gender.Female } }).AsQueryable(), 0, 2 }
          };
        }
      }

      [Theory]
      [MemberData(nameof(GoodUserData))]
      public void AnyData_Calculated(IQueryable<User> users, int NUMBER_OF_MALE_USERS, int NUMBER_OF_FEMALE_USERS) {
        //Arrange:

        //Nothing To Do

        //Act:
        var genderMetrics = UserClassBusinessHandler.GetNumberOfUsersByGender(users);

        int calculatedNumberOfMaleUsers = (
          from x in genderMetrics
          where x.Gender == Gender.Male
          select x.Count
        ).SingleOrDefault();

        int calculatedNumberOfFemaleUsers = (
          from x in genderMetrics
          where x.Gender == Gender.Female
          select x.Count
        ).SingleOrDefault();

        //Assert:
        Assert.Equal(NUMBER_OF_MALE_USERS, calculatedNumberOfMaleUsers);
        Assert.Equal(NUMBER_OF_FEMALE_USERS, calculatedNumberOfFemaleUsers);
      }
    }

    public class GetAverageAgeOfUsersMethod {
      [Theory]
      [InlineData(30, 60)]
      [InlineData(25, 50)]
      public void AnyValues_Calculated(int age1, int age2) {
        //Arrange:
        double AVERAGE_AGE = (age1 + age2) / 2.0;

        var users = (
          new User[] {
            new User {
              Age = age1
            },
            new User {
              Age = age2
            }
          }
        ).AsQueryable();


        //Act:
        double calculatedAverageAge = UserClassBusinessHandler.GetAverageAgeOfUsers(users);

        //Assert:
        Assert.Equal(AVERAGE_AGE, calculatedAverageAge);
      }
    }
  }
}
