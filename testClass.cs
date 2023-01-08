using Moq;
using TeraClouds.Interfaces;
using TeraClouds.Managements;
using Xunit;
using TeraClouds.Enums;

public class TestHasHighPressureGuard
{
 private readonly ComprManagement _sut;

 private Mock<ComprManagement> _ComprManagementMock = new Mock<ComprManagement>();
 private Mock<ICircuit> _C1Mock = new Mock<ICircuit>();
 private readonly Mock<ICircuit> _C2Mock = new Mock<ICircuit>();
 private readonly CircuitType C1;




 public TestHasHighPressureGuard()
 {
  _sut = new ComprManagement(_C1Mock.Object, _C2Mock.Object);
 }

 [Fact]
 public void PassingConverterTest()
 {
  Assert.Equal(0.05,
   TeraClouds.Utils.PressureConverter.converIntoKiloPascal(50, TeraClouds.Enums.PressureUnits.pa));

 }

 [Fact]
 public void FailingConverterTest()
 {
  Assert.NotEqual(30,
   TeraClouds.Utils.PressureConverter.converIntoKiloPascal(50, TeraClouds.Enums.PressureUnits.pa));

 }

 [Fact]
 public void PassingHasHighPressureGuard()
 {
  //Arrange
  //Act
  _C1Mock.Setup(x => x.Pressure).Returns(6);
  //Assert
  Assert.True(_sut.HasHighPressureGuard(C1));
 }

 [Fact]
 public void FailingHasHighPressureGuard()
 {
  //Arrange
  //Act
  _C1Mock.Setup(x => x.Pressure).Returns(11);
  //Assert
  Assert.False(_sut.HasHighPressureGuard(C1));
 }

 [Fact]
 public void FailingCanStartComprC1Ca()
 {
  DateTime minTime = DateTime.MinValue;
  //Arrange
  //Act
  _C1Mock.Setup(x => x.CA.Enabled).Returns(true);
  _C1Mock.Setup(x => x.CA.Running).Returns(false);
  _C1Mock.Setup(x => x.CA.Alarm).Returns(false);
  _C1Mock.Setup(x => x.Alarm).Returns(false);
  _C1Mock.Setup(x => x.Pressure).Returns(6);
  _C1Mock.Setup(x => x.CA.StopTime).Returns(minTime);
  _C1Mock.Setup(x => x.CB.StartTime).Returns(minTime);
  _C1Mock.Setup(x => x.CB.StopTime).Returns(minTime);
  _C2Mock.Setup(x => x.Pressure).Returns(6);
  _C2Mock.Setup(x => x.CA.StartTime).Returns(minTime);
  _C2Mock.Setup(x => x.CB.StartTime).Returns(minTime);

  //Assert
  Assert.False(_sut.CanStartComprC1CA());
 }


}