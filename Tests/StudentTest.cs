using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Registrar
{
  public class StudentTest : IDisposable
  {
    public StudentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=university_registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Student.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      //Arrange, Act
      Student testStudent1 = new Student("Lucille Ball", DateTime.Today);
      Student testStudent2 = new Student("Lucille Ball", DateTime.Today);
      //Assert
      Assert.Equal(testStudent1, testStudent2);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Student testStudent = new Student("Lucille Ball", DateTime.Today);
      //Act
      testStudent.Save();
      List<Student> result = Student.GetAll();
      //Assert
      List<Student> testList = new List<Student>{testStudent};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Find_FindsStudentInDatabase()
    {
      //Arrange
      Student testStudent = new Student("Lucille Ball", DateTime.Today);
      testStudent.Save();
      //Act
      Student foundStudent = Student.Find(testStudent.GetId());
      //Assert
      Assert.Equal(testStudent, foundStudent);
    }

    [Fact]
    public void UpdateName_UpdatesStudentName()
    {
    //Arrange
    Student testStudent = new Student("Lucille Ball", DateTime.Today);
    testStudent.Save();
    Student expectedResult = new Student("Lucy Ball", testStudent.GetEnrollmentDate(), testStudent.GetId());
    //Act
    testStudent.UpdateName("Lucy Ball");
    //Assert
    Assert.Equal(expectedResult, testStudent);
    }

    [Fact]
    public void UpdateEnrollmentDate_UpdatesStudentEnrollmentDate()
    {
    //Arrange
    Student testStudent = new Student("Lucille Ball", DateTime.Today);
    testStudent.Save();
    DateTime testDate = new DateTime(2016, 12, 1);
    Student expectedResult = new Student("Lucille Ball", testDate, testStudent.GetId());
    //Act
    testStudent.UpdateEnrollmentDate(testDate);
    //Assert
    Assert.Equal(expectedResult, testStudent);
    }

    [Fact]
    public void Test_Delete_DeletesStudentFromDatabase()
    {
      //Arrange
      Student testStudent1 = new Student("Lucille Ball", DateTime.Today);
      testStudent1.Save();
      Student testStudent2 = new Student("Desi Arnez", DateTime.Today);
      testStudent2.Save();

      //Act
      testStudent1.Delete();
      List<Student> resultStudents = Student.GetAll();
      List<Student> testStudentList = new List<Student> {testStudent2};

      //Assert
      Assert.Equal(testStudentList, resultStudents);
    }

    public void Dispose()
    {
      Student.DeleteAll();
    }
  }
}
