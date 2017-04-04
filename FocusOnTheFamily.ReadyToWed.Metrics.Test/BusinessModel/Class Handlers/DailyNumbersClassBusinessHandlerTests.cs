using System.Linq;

using Xunit;

using FocusOnTheFamily.ReadyToWed.Metrics.DataModel;
using FocusOnTheFamily.ReadyToWed.Metrics.BusinessModel;

namespace FocusOnTheFamily.ReadyToWed.Metrics.Test {
  public class DailyNumbersClassBusinessHandlerTests {
    public class GetAverageDailyInstallsMethod {
      [Theory]
      [InlineData(5, 20)]
      [InlineData(10, 15)]
      public void AnyValues_Calculated(int numberOfInstalls1, int numberOfInstalls2) {
        //Arrange:
        double AVERAGE_NUMBER_OF_INSTALLS = (numberOfInstalls1 + numberOfInstalls2) / 2.0;

        var dailyNumbers = (
          new DailyNumbers[] {
            new DailyNumbers {
              Installs = numberOfInstalls1
            },
            new DailyNumbers {
              Installs = numberOfInstalls2
            }
          }
        ).AsQueryable();


        //Act:
        double calculatedAverageNumberOfInstalls = DailyNumbersClassBusinessHandler.GetAverageDailyInstalls(dailyNumbers);

        //Assert:
        Assert.Equal(AVERAGE_NUMBER_OF_INSTALLS, calculatedAverageNumberOfInstalls);
      }
    }

    public class GetAverageDailyLoginsMethod {
      [Theory]
      [InlineData(30, 25)]
      [InlineData(45, 50)]
      public void AnyValues_Calculated(int numberOfLogins1, int numberOfLogins2) {
        //Arrange:
        double AVERAGE_NUMBER_OF_LOGINS = (numberOfLogins1 + numberOfLogins2) / 2.0;

        var dailyNumbers = (
          new DailyNumbers[] {
            new DailyNumbers {
              Logins = numberOfLogins1
            },
            new DailyNumbers {
              Logins = numberOfLogins2
            }
          }
        ).AsQueryable();


        //Act:
        double calculatedAverageNumberOfLogins = DailyNumbersClassBusinessHandler.GetAverageDailyLogins(dailyNumbers);
        
        //Assert:
        Assert.Equal(AVERAGE_NUMBER_OF_LOGINS, calculatedAverageNumberOfLogins);
      }
    }

    public class GetAverageDailyCompletionsMethod {
      [Theory]
      [InlineData(55, 35)]
      [InlineData(65, 75)]
      public void AnyValues_Calculated(int numberOfCompletions1, int numberOfCompletions2) {
        //Arrange:
        double AVERAGE_NUMBER_OF_COMPLETIONS = (numberOfCompletions1 + numberOfCompletions2) / 2.0;

        var dailyNumbers = (
          new DailyNumbers[] {
            new DailyNumbers {
              Completions = numberOfCompletions1
            },
            new DailyNumbers {
              Completions = numberOfCompletions2
            }
          }
        ).AsQueryable();


        //Act:
        double calculatedAverageNumberOfCompletions = DailyNumbersClassBusinessHandler.GetAverageDailyCompletions(dailyNumbers);

        //Assert:
        Assert.Equal(AVERAGE_NUMBER_OF_COMPLETIONS, calculatedAverageNumberOfCompletions);
      }
    }
  }
}
