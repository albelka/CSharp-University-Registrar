using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Registrar
{
  public class CourseTest : IDisposable
  {
    public CourseTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=university_registrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Course.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      //Arrange, Act
      Course testCourse1 = new Course("History 101", "HST101");
      Course testCourse2 = new Course("History 101", "HST101");
      //Assert
      Assert.Equal(testCourse1, testCourse2);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Course testCourse = new Course("History 101", "HST101");
      //Act
      testCourse.Save();
      List<Course> result = Course.GetAll();
      //Assert
      List<Course> testList = new List<Course>{testCourse};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Find_FindsCourseInDatabase()
    {
      //Arrange
      Course testCourse = new Course("History 101", "HST101");
      testCourse.Save();
      //Act
      Course foundCourse = Course.Find(testCourse.GetId());
      //Assert
      Assert.Equal(testCourse, foundCourse);
    }

    [Fact]
    public void Update_UpdatesCourseName()
    {
    //Arrange
    Course testCourse = new Course("History 101", "HST101");
    testCourse.Save();
    Course expectedResult = new Course("World History 101", "HST101", testCourse.GetId());
    //Act
    testCourse.Update("name", "World History 101");
    //Assert
    Assert.Equal(expectedResult, testCourse);
    }

    [Fact]
    public void Update_UpdatesCourseNumber()
    {
    //Arrange
    Course testCourse = new Course("History 101", "HST101");
    testCourse.Save();
    Course expectedResult = new Course("History 101", "HIST101A", testCourse.GetId());
    //Act
    testCourse.Update("course_number", "HIST101A");
    //Assert
    Assert.Equal(expectedResult, testCourse);
    }

    [Fact]
    public void Test_Delete_DeletesTaskFromDatabase()
    {
      //Arrange
      Course testCourse1 = new Course("History 101", "HST101");
      testCourse1.Save();
      Course testCourse2 = new Course("English 101", "ENG101");
      testCourse2.Save();

      //Act
      testCourse1.Delete();
      List<Course> resultCourses = Course.GetAll();
      List<Course> testCourseList = new List<Course> {testCourse2};

      //Assert
      Assert.Equal(testCourseList, resultCourses);
    }
    [Fact]
    public void Test_GetStudents_RetrievesStudentsEnrolledInACourse()
    {
      //Arrange
      Course testCourse1 = new Course("History 101", "HST101");
      testCourse1.Save();
      Course testCourse2 = new Course("English 101", "ENG101");
      testCourse2.Save();

      Student testStudent = new Student("Lucille Ball", DateTime.Today);
      testStudent.Save();
      Student testStudent2 = new Student("Dezi Arnez", DateTime.Today);
      testStudent2.Save();

      Course.Enroll(testCourse1.GetId(), testStudent.GetId());
      Course.Enroll(testCourse1.GetId(), testStudent2.GetId());
      Course.Enroll(testCourse2.GetId(), testStudent2.GetId());

      //Act
      List<Student> enrolledStudents = testCourse1.GetStudents();
      List<Student> expectedStudents = new List<Student> {testStudent, testStudent2};

      //Assert
      Assert.Equal(expectedStudents, enrolledStudents);
    }


    public void Dispose()
    {
      Course.DeleteAll();
    }
  }
}
